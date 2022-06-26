using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QRCoder;
using WebApplication.Api.Models;
using WebApplication.Api.Models.DeviceAuth;
using WebApplication.Entities;
using WebApplication.Generators;
using WebApplication.Options;
using WebApplication.Repositories;

namespace WebApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("app_access", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DeviceAuthController : ControllerBase
    {
        private readonly IDeviceAuthRepository _deviceAuthRepository;
        private readonly DeviceUserAuthOptions _options;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IUserAuthRepository _userAuthRepository;

        public DeviceAuthController(
            IOptions<DeviceUserAuthOptions> optionsAccessor,
            IPasswordGenerator passwordGenerator,
            IDeviceAuthRepository deviceAuthRepository,
            IUserAuthRepository userAuthRepository)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _passwordGenerator = passwordGenerator ?? throw new ArgumentNullException(nameof(passwordGenerator));
            _deviceAuthRepository = deviceAuthRepository ?? throw new ArgumentNullException(nameof(deviceAuthRepository));
            _userAuthRepository = userAuthRepository ?? throw new ArgumentNullException(nameof(userAuthRepository));
        }

        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> Generate()
        {
            var deviceCode = GenerateDeviceCode();
            var userCode = GenerateUserCode();
            var (verificationUriComplete, verificationUri) = GenerateVerificationUri(userCode);
            var qrCodeBitmapData = GenerateQRCode(verificationUriComplete);

            var deviceUserAuthEntity = new DeviceUserAuthEntity
            {
                DeviceCode = deviceCode,
                UserCode = userCode
            };

            await _deviceAuthRepository.SaveDeviceAuth(deviceCode, deviceUserAuthEntity);
            await _userAuthRepository.SaveUserAuth(userCode, deviceUserAuthEntity);

            var responseBodyModel = new GenerateDeviceAuthResponseBodyModel
            {
                DeviceCode = deviceCode,
                QRCodeBitmapData = Convert.ToBase64String(qrCodeBitmapData),
                UserCode = userCode,
                VerificationUri = verificationUri.AbsoluteUri,
                VerificationUriComplete = verificationUriComplete.AbsoluteUri
            };

            return Ok(responseBodyModel);
        }

        [HttpPost]
        [Route("verify")]
        public async Task<IActionResult> Verify(VerifyDeviceAuthRequestBodyModel requestBodyModel)
        {
            var deviceUserAuthEntity = await _deviceAuthRepository.GetDeviceAuth(requestBodyModel.DeviceCode);

            if (deviceUserAuthEntity == null || string.IsNullOrEmpty(deviceUserAuthEntity.UserId))
            {
                var errorResponseBodyModel = new ConflictErrorResponseBodyModel
                {
                    UserMessage = "Invalid device code."
                };

                return Conflict(errorResponseBodyModel);
            }

            await _deviceAuthRepository.DeleteDeviceAuth(deviceUserAuthEntity.DeviceCode);
            await _userAuthRepository.DeleteUserAuth(deviceUserAuthEntity.UserCode);

            var responseBodyModel = new VerifyDeviceAuthResponseBodyModel
            {
                UserId = deviceUserAuthEntity.UserId
            };

            return Ok(responseBodyModel);
        }

        private string GenerateDeviceCode()
        {
            var deviceCodeGeneratorSettings = new PasswordGeneratorSettings(true, true, true, false, 128);
            var deviceCode = _passwordGenerator.GeneratePassword(1, deviceCodeGeneratorSettings);
            return deviceCode;
        }

        private byte[] GenerateQRCode(Uri verificationUriComplete)
        {
            using var qrCodeGenerator = new QRCodeGenerator();
            using var qrCodeData = qrCodeGenerator.CreateQrCode(verificationUriComplete.AbsoluteUri, QRCodeGenerator.ECCLevel.L);
            using var qrCodeBitmapGenerator = new BitmapByteQRCode(qrCodeData);
            var qrCodeBitmapData = qrCodeBitmapGenerator.GetGraphic(4);
            return qrCodeBitmapData;
        }

        private string GenerateUserCode()
        {
            var userCodeGeneratorSettings = new PasswordGeneratorSettings(true, false, true, false, 8);
            var userCode = _passwordGenerator.GeneratePassword(1, userCodeGeneratorSettings);
            return userCode;
        }


        private (Uri, Uri) GenerateVerificationUri(string userCode)
        {
            var verificationUriComplete = new Uri(string.Format(_options.VerificationUriFormat, userCode));
            var verificationUri = new Uri(verificationUriComplete.GetLeftPart(UriPartial.Path));
            return (verificationUriComplete, verificationUri);
        }
    }
}
