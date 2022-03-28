using AutoMapper;
using GameServer.Data.Repositories.DomainRepositories.Interfaces;
using GameServer.Data.UnitOfWork;
using GameServer.Models;
using GameServer.Services.Dtos.Game;
using GameServer.Services.Dtos.Round;
using GameServer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GameServer.Services
{
    public class RoundService : IRoundService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IRoundRepository _roundRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoundService(IMemoryCache memoryCache,
            IRoundRepository roundRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _memoryCache = memoryCache;
            _roundRepository = roundRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public RoundDto CreateRound()
        {
            var cacheKey = "ActivePlayers";
            var activePlayers = _memoryCache.Get<ActivePlayersDto>(cacheKey);

            var gameRounds = activePlayers.GameIds.Select(x => new GameRound
            {
                GameId = x
            })
                .ToList();

            var round = new Round
            {
                Question = new Question(),
                Games = gameRounds
            };

            _roundRepository.Add(round);
            _unitOfWork.Commit();
            return _mapper.Map<RoundDto>(round);
        }

        public RoundDto AnswerRound(RoundDto roundDto, string userId)
        {
            var round = _roundRepository.GetAll()
                .Include(x => x.Answers)
                .Include(x => x.Question)
                .FirstOrDefault(x => x.Id == roundDto.Id);
            if (round is null)
            {
                throw new Exception("No round found with the Id");
            }

            var isCorrect = round.Question.QuestionAnswer == roundDto.AnswerBody;

            var answer = new Answer();
            answer.UserId = userId;
            answer.IsCorrect = isCorrect;
            round.IsFinished = isCorrect;
            round.Answers.Add(answer);
            _unitOfWork.Commit();
            var roundResult = _mapper.Map<RoundDto>(round);
            roundResult.IsAnswerCorrect = isCorrect;
            roundResult.AnswerBody = roundDto.AnswerBody;
            roundResult.AnswerMessage = isCorrect ? "OK" : "FAILED";
            return roundResult;
        }

        public RoundDto GetActiveRound()
        {
            var activeRound = _roundRepository.GetAll().Include(x => x.Question).FirstOrDefault(x => !x.IsFinished);
            return activeRound is null ? CreateRound() : _mapper.Map<RoundDto>(activeRound);
        }
    }
}
