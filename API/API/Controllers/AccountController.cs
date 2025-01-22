using API.Dtos;
using API.Errors;
using Core.Interfaces;
using Core.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }





        // login 

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Validate the incoming loginDto
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, "Invalid login data."));
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                // Log the failed login attempt
                return Unauthorized(new ApiResponse(401, "Invalid email or password."));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                // Log the failed login attempt
                return Unauthorized(new ApiResponse(401, "Invalid email or password."));
            }

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.FirstName
            };
        }


        //register 

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, "Invalid register data."));
            }
            // check email exists ...we will return bad request 
            //CheckEmailExistsAsync is a method down below in the code

            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                { Errors = new[] { "Email address is in use" } });
            }

            var user = new ApplicationUser
            {
                FirstName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                DisplayName = user.FirstName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }


        // check email exists
        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }




        // get current user for any information form client angular 
        
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            // Retrieve the email from the claims
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return Unauthorized(new ApiResponse(401, "User not found."));

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.FirstName
            };
        }


    





    }







}

