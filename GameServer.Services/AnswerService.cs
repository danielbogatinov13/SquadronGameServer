using GameServer.Data.Repositories.DomainRepositories.Interfaces;
using GameServer.Data.UnitOfWork;
using GameServer.Services.Interfaces;

namespace GameServer.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AnswerService(IAnswerRepository answerRepository,
            IUnitOfWork unitOfWork)
        {
            _answerRepository = answerRepository;
            _unitOfWork = unitOfWork;
        }
    }
}
