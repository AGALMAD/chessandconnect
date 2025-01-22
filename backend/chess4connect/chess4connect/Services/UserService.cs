using chess4connect.Models.Database.Entities;

namespace chess4connect.Services;

public class UserService
{
    private readonly UnitOfWork _unitOfWork;

    public UserService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User> GetUserByStringId(string id)
    {
        return await _unitOfWork.UserRepository.GetAllInfoButOrdersById(Int32.Parse(id));

    }

}
