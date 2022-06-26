using WebApplication.Entities;

namespace WebApplication.Repositories
{
    public interface IUserAuthRepository
    {
        Task DeleteUserAuth(string userCode);

        Task<DeviceUserAuthEntity> GetUserAuth(string userCode);

        Task SaveUserAuth(string deviceCode, DeviceUserAuthEntity entity);
    }
}
