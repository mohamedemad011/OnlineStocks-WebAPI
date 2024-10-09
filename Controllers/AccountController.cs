using FinShark.Dtos.Account;
using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace FinShark.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(ITokenService tokenService,UserManager<AppUser>userManager,SignInManager<AppUser>signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var appUser = new AppUser
                {
                    UserName=registerDto.Username,
                    Email=registerDto.Email,
                };
                var createdUser=await _userManager.CreateAsync(appUser,registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(
                                new NewUserDto
                                {
                                    UserName = appUser.UserName,
                                    Email = appUser.Email,
                                    Token = _tokenService.CreateToken(appUser)
                                }
                            );
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500,createdUser.Errors);
                }
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult>Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)return BadRequest(ModelState);
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if(user== null)return Unauthorized("Invalid UserName");
            var res = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if (!res.Succeeded) return Unauthorized("Username Or Password Incorrect");
            return Ok(
                    new NewUserDto
                    {
                        UserName=user.UserName,
                        Email=user.Email,
                        Token=_tokenService.CreateToken(user)
                    }
                );
        }

    }
}
