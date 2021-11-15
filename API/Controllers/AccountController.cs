using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dto;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        ITokenService tokenService, IMapper mapper)
        {
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.signInManager = signInManager;
            this.userManager = userManager;

        }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {

        var user = await userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

        return new UserDto
        {
            Email = user.Email,
            Token = tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }

    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await userManager.FindByEmailAsync(email) != null;
    }

    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {

        var user = await userManager.FindByClaimsPrinciplesWithAddressAsync(HttpContext.User);

        return mapper.Map<Address,AddressDto>(user.Adress );
    }
    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto addressDto)
    {
        var user = await userManager.FindByClaimsPrinciplesWithAddressAsync(HttpContext.User);

        user.Adress = mapper.Map<AddressDto, Address>(addressDto);

        var result = await userManager.UpdateAsync(user);

        if(result.Succeeded) return Ok(mapper.Map<Address,AddressDto>(user.Adress));
        else return BadRequest("Problem updating user"); 
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);

        if (user == null)
        {
            return Unauthorized(new ApiResponse(401));
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new ApiResponse(401));

        return new UserDto
        {
            Email = user.Email,
            Token = tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = userManager.CreateAsync(user, registerDto.Password);

        if (!result.IsCompletedSuccessfully)
            return BadRequest(new ApiResponse(400));

        return new UserDto
        {
            DisplayName = user.DisplayName,
            Token = tokenService.CreateToken(user),
            Email = user.Email
        };


    }

}
}