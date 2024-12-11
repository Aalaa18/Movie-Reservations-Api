using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieApi.DTO;
using MovieApi.Models;
using MovieApi.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UserController : ControllerBase
    {
        private IuserServices _iuser;
        private readonly UserManager<Appuser> _userManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IConfiguration _configuration;

        public UserController(IuserServices iuserServices,UserManager<Appuser> userManager, JwtOptions JwtOptions, IConfiguration configuration)
        {
            _iuser=iuserServices;
            _userManager=userManager;
            _jwtOptions = JwtOptions;
            _configuration = configuration;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(DtoUser dtoUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input data." });

            // Check if the user exists
            var user = await _userManager.FindByNameAsync(dtoUser.name);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password." });

            // Verify the password
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, dtoUser.password);
            if (!isPasswordValid)
                return Unauthorized(new { message = "Invalid username or password." });



            // Creates a new symmetric security key from the JWT key specified in the app configuration.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:SigningKey"]));
            // Sets up the signing credentials using the above security key and specifying the HMAC SHA256 algorithm.
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Defines a set of claims to be included in the token.
            var claims = new List<Claim>
            {
                // Custom claim using the user's ID.
                new Claim("Myapp_User_Id", user.Id.ToString()),
                // Standard claim for user identifier, using username.
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                // Standard claim for user's email.
                new Claim(ClaimTypes.Email, user.Email),
                // Standard JWT claim for subject, using user ID.
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

                new Claim(ClaimTypes.Role,user.role)
            };

            // Adds a role claim for each role associated with the user.
            //user.role.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

            // Creates a new JWT token with specified parameters including issuer, audience, claims, expiration time, and signing credentials.
            var token = new JwtSecurityToken(
                issuer: _configuration["jwt:Issuer"],
                audience: _configuration["jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiration set to 1 hour from the current time.
                signingCredentials: credentials);

            // Serializes the JWT token to a string and returns it.
            var _token= new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(_token);
        }


        [HttpPost("SignUp User")]
        public async Task<IActionResult> CreateAccount([FromBody] sigUpDto sign) {

           if(ModelState.IsValid)
            {
                Appuser user = new Appuser
                {
                    UserName = sign.UserName,
                    Email = sign. Email,
                    pass = sign.Password,
                    role= sign.Code == "CodersAdmin"?"admin":"ordinary",   
                    
                  

                };
               var result= await _userManager.CreateAsync(user, sign.Password);
                if(result.Succeeded)
                {
                    
                    return Ok("Success");

                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

        }



        [HttpDelete("Delete Account/{name}")]
        [Authorize(Roles = "admin,ordinary")]
        public async Task<IActionResult> Remove( string name)
        {
            var result=await _iuser.RemoveUser(name);
            if( result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpPut("Upadte Details")]
        [Authorize(Roles = "admin,ordinary")]

        public async Task<IActionResult> upadte([FromBody]DtoNewuser dtoNewuser)
        {
            var result = await _iuser.upadteDetails(dtoNewuser);
            if (result == null)
            {
                return NotFound();
            }
            return Ok($"upadted {result}");
        }


        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<IEnumerable<Appuser>>> GetAll() 
        {
            var result = await _iuser.GetAllUsers();

            if (result == null) return NotFound();

            return Ok(result);
        }

    }
}
