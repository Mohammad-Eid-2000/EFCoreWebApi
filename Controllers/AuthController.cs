using EFCoreWebApi.Models;
using EFCoreWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IRepository<User> _userRepository;

    public AuthController(IConfiguration configuration, IRepository<User> userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
 
 
 
        // Validate user credentials (e.g., check against database)
        // Replace this with a proper user validation mechanism (e.g., using Identity or a service)
        var user = await _userRepository.SearchSingle(model.Username);

        // Hash the provided password with the stored salt

        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            return Unauthorized("Invalid username or password.");

        // Generate JWT token
        try
        {
            var token = GenerateJwtToken(model.Username, user);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private string GenerateJwtToken(string username, User user)
    {
        // Get the JWT key from configuration
        var key = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(key) || Encoding.UTF8.GetByteCount(key) < 32)
        {
            throw new ArgumentException("JWT Key must be at least 256 bits (32 bytes) long.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim("userId", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
