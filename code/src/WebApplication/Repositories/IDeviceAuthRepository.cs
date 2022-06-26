using WebApplication.Entities;

namespace WebApplication.Repositories
{
    public interface IDeviceAuthRepository
    {
        Task DeleteDeviceAuth(string deviceCode);

        Task<DeviceUserAuthEntity> GetDeviceAuth(string deviceCode);

        Task SaveDeviceAuth(string deviceCode, DeviceUserAuthEntity entity);
    }
}
