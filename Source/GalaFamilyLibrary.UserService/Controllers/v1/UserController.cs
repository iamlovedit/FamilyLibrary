using AutoMapper;
using GalaFamilyLibrary.Infrastructure.Common;
using GalaFamilyLibrary.UserService.DataTransferObjetcts;
using GalaFamilyLibrary.UserService.Models;
using GalaFamilyLibrary.UserService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalaFamilyLibrary.UserService.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class UserController : ApiControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageModel<string>> Regsiter([FromBody] LibraryUserCreationDto userCreationDto)
        {
            if ((await _userService.GetFirstByExpressionAsync(u => u.Username == userCreationDto.Username) != null))
            {
                return Failed("用户名已存在");
            }
            var user = _mapper.Map<LibraryUser>(userCreationDto);
            var id = await _userService.AddAsync(user);
            if (id > 0)
            {
                return Success("注册成功");
            }
            return Failed("注册失败");
        }
    }
}