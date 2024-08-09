

using ApiDemo.Dtos;
using ApiDemo.ResponseModule;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiDemo.Controllers
{
    
    public class AccountController : BaseController
    {
        private readonly ITokenService tokenService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public AccountController(ITokenService tokenService
            , SignInManager<AppUser> signInManager
            , UserManager<AppUser> userManager
            ,IMapper mapper)
        {
          
            this.tokenService = tokenService;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            
            var user =await  userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
                return NotFound(Unauthorized(401));
            var result= await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if(!result.Succeeded)
                return NotFound(Unauthorized(401));

            return Ok(new UserDto
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = tokenService.CreateToken(user)
            });

        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if( CheckEmailExist(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "Email Is Already Exists" }
                });
            }
            var user = new AppUser
            {
                Email = registerDto.Email,
                DisplayName = registerDto.Name,
                UserName = registerDto.Email
            };
            var result=await userManager.CreateAsync(user,registerDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    return BadRequest(new ApiResponse(400, error.Description));

                }
            }
            return Ok(new UserDto
            {
                
                Email = user.Email,
                DisplayName=user.DisplayName,
                Token = tokenService.CreateToken(user)
            });
            
            
        }
        [HttpGet]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        => await userManager.FindByEmailAsync(email) != null;
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
         {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user =await userManager.FindByEmailAsync(email);
            if (user == null) 
            {
                return NotFound(new ApiResponse(404));
            }
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = email,
                Token = tokenService.CreateToken(user)
            };
            
        }
        [Authorize]
        [HttpGet("GetAddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = userManager.Users.Include(u => u.Address)
                .SingleOrDefault(u=>u.Email == email);
            return Ok(mapper.Map<AddressDto>(user.Address));
        }
        [Authorize]
        [HttpPost("UpdateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = userManager.Users.Include(u => u.Address)
                .SingleOrDefault(u => u.Email == email);
            user.Address = mapper.Map<Address>(addressDto);
            var result=await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Problen in Updating The Address"));
            return Ok(mapper.Map<AddressDto>(user.Address));
           
        }

    }
}
