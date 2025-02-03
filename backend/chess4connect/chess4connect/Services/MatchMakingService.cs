using chess4connect.Enums;
using chess4connect.Models.Database.Entities;

namespace chess4connect.Services
{
    public class MatchMakingService
    {
        UnitOfWork _unitOfWork;

        public MatchMakingService(UnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }




    }
}
