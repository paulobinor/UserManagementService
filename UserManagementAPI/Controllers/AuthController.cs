using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Core.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
       
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
       // private readonly UserManagement.Service.Email.IEmailService _emailService;

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
           // _emailService = emailService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterUser registerUser, string role)
        {
           var userExists = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExists != null)
            {
                return NotFound("Cannot create an account with this email. ");
            }
            if (!(await _roleManager.RoleExistsAsync(role)))
            {
                return BadRequest("Invalid parameters");
            }
            IdentityUser identityUser = new IdentityUser
            {
                Email = registerUser.Email,
                SecurityStamp = System.Guid.NewGuid().ToString(),
                UserName = registerUser.UserName
                
            };
            var res = await _userManager.CreateAsync(identityUser, registerUser.Password);
            if (res.Succeeded)
            {
                var roleRes = await _userManager.AddToRoleAsync(identityUser, role);
                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                //if (token != null)
                //{
                //    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new {token, identityUser.Email});
                //    var message = new Message(new string[] { identityUser.Email! }, "Confirmation email link", confirmationLink);
                //    _emailService.SendEmail(message);
                //}
                return Created("", new { Message = "User created successfully", StatusCode = "201" });

            }
           

            string errors = string.Empty;
            foreach (var item in res.Errors)
            {
                errors += $"Error code:{item.Code} Description: {item.Description} | ";
            }
            return BadRequest(errors);
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            //Check if user exists
            var validUser = await _userManager.FindByNameAsync(userLogin.UserName);
            if (validUser == null)
            {
                return NotFound(new { StatusCode = "404", Message = "User not found" });

            }
            var validPass = await _userManager.CheckPasswordAsync(validUser, userLogin.Password);
            if (!validPass)
            {
                return Unauthorized(new { StatusCode = "401", Message = "You have entered an invalid username or password" });
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userLogin.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var roles = await _userManager.GetRolesAsync(validUser);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = GetToken(claims);
            return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token), Expiry = token.ValidTo});
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(

                issuer: _configuration["JWT:Secret"],
                audience: _configuration["JWT:Secret"],
                expires: DateTime.Now.AddHours(1),
                claims : authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
    }
}