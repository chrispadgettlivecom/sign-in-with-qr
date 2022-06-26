using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Api.Models;
using WebApplication.Api.Models.UserAuth;
using WebApplication.Repositories;

namespace WebApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("app_access", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserAuthController : ControllerBase
    {
        private readonly IDeviceAuthRepository _deviceAuthRepository;
        private readonly IUserAuthRepository _userAuthRepository;

        public UserAuthController(IDeviceAuthRepository deviceAuthRepository, IUserAuthRepository userAuthRepository)
        {
            _deviceAuthRepository = deviceAuthRepository ?? throw new ArgumentNullException(nameof(deviceAuthRepository));
            _userAuthRepository = userAuthRepository ?? throw new ArgumentNullException(nameof(userAuthRepository));
        }

        [HttpPost]
        [Route("complete")]
        public async Task<IActionResult> Complete(CompleteUserAuthRequestBodyModel requestBodyModel)
        {
            var deviceUserAuthEntity = await _userAuthRepository.GetUserAuth(requestBodyModel.UserCode);

            if (deviceUserAuthEntity == null)
            {
                var errorResponseBodyModel = new ConflictErrorResponseBodyModel
                {
                    UserMessage = "Invalid user code."
                };

                return Conflict(errorResponseBodyModel);
            }

            deviceUserAuthEntity.UserId = requestBodyModel.UserId;
            await _deviceAuthRepository.SaveDeviceAuth(deviceUserAuthEntity.DeviceCode, deviceUserAuthEntity);
            await _userAuthRepository.SaveUserAuth(deviceUserAuthEntity.UserCode, deviceUserAuthEntity);
            return Ok();
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IActionResult> Validate(ValidateUserAuthRequestBodyModel requestBodyModel)
        {
            var deviceUserAuthEntity = await _userAuthRepository.GetUserAuth(requestBodyModel.UserCode);

            if (deviceUserAuthEntity == null)
            {
                var errorResponseBodyModel = new ConflictErrorResponseBodyModel
                {
                    UserMessage = "Invalid user code."
                };

                return Conflict(errorResponseBodyModel);
            }

            return Ok();
        }
    }
}
