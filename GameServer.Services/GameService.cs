using GameServer.Data.Repositories.DomainRepositories.Interfaces;
using GameServer.Data.UnitOfWork;
using GameServer.Models;
using GameServer.Services.Dtos.Game;
using GameServer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace GameServer.Services
{
    public class GameService : IGameService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IGameRepository _gameRepository;
        private readonly IUnitOfWork _unitOfWork;
        public GameService(
            IMemoryCache memoryCache,
            IGameRepository gameRepository,
            IUnitOfWork unitOfWork)
        {
            _memoryCache = memoryCache;
            _gameRepository = gameRepository;
            _unitOfWork = unitOfWork;
        }

        public int EnterGame(string userId)
        {
            var cacheKey = "ActivePlayers";
            var hasActivePlayers = _memoryCache.TryGetValue(cacheKey, out ActivePlayersDto activePlayers);

            //check if limit is reached

            var game = new Game
            {
                UserId = userId
            };
            //check if firsy entry
            if (!hasActivePlayers)
            {
                activePlayers = new ActivePlayersDto();
                activePlayers.NumberOfActivePlayers = 1;
                activePlayers.GameIds = new List<int>();

                _memoryCache.CreateEntry(cacheKey);
            }
            else if (activePlayers.NumberOfActivePlayers == 5)
            {
                throw new Exception("There are already five players, please try later.");
            }
            else
            {
                activePlayers.NumberOfActivePlayers += 1;
            }
            _gameRepository.Add(game);
            _unitOfWork.Commit();
            activePlayers.GameIds.Add(game.Id);
            //setting cache entries
            _memoryCache.Set(cacheKey, activePlayers);
            return game.Id;
        }
        public void ExitGame(string userId)
        {
            var cacheKey = "ActivePlayers";
            var activePlayers = _memoryCache.Get<ActivePlayersDto>(cacheKey);

            var game = _gameRepository.GetAll().FirstOrDefault(x => x.UserId == userId && !x.IsFinished);
            if (game is null)
            {
                throw new Exception("There is not game started for this user");
            }
            game.IsFinished = true;

            activePlayers.NumberOfActivePlayers -= 1;
            activePlayers.GameIds.Remove(game.Id);

            _unitOfWork.Commit();
            //setting cache entries
            _memoryCache.Set(cacheKey, activePlayers);
        }
        public ActivePlayersDto GetPlayers()
        {
            var cacheKey = "ActivePlayers";
            var hasActivePlayers = _memoryCache.TryGetValue(cacheKey, out ActivePlayersDto activePlayers);
            if (hasActivePlayers)
            {
                return activePlayers;
            }
            throw new Exception("There are no active players");
        }
    }
}
