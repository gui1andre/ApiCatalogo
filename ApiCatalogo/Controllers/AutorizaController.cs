
using ApiCatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ApiCatalogo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutorizaController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _singInManager;
    private readonly IConfiguration _configuration;

    public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _singInManager = signInManager;
        _configuration = configuration;
    }


    [HttpGet]
    public ActionResult<string> Get()
    {
        return "AutorizaController ::  Acessado em  : "
          + DateTime.Now.ToLongDateString();
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO usuarioDTO)
    {
        var user = new IdentityUser
        {
            UserName = usuarioDTO.Email,
            Email = usuarioDTO.Email,
            EmailConfirmed = true

        };

        var result = await _userManager.CreateAsync(user, usuarioDTO.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _singInManager.SignInAsync(user, false);
        return Ok(GerarToken(usuarioDTO));
    }

    [HttpPost("login")]
    public async Task<ActionResult<UsuarioToken>> Login([FromBody] UsuarioDTO usuarioDTO)
    {
        var result = await _singInManager.PasswordSignInAsync(usuarioDTO.Email, usuarioDTO.Password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            UsuarioToken token = GerarToken(usuarioDTO);
            return Ok(new
            {
                token.Token,
                token.Authenticated,
                token.Message,
                token.Expiration
            });
        }
        ModelState.AddModelError(string.Empty, "Login Invalido");
        return BadRequest(ModelState);
    }

    private UsuarioToken GerarToken(UsuarioDTO usuarioDTO)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, usuarioDTO.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiracao = _configuration["TokenConfig:ExpiredHours"];
        var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["TokenConfig:Issuer"],
            audience: _configuration["TokenConfig:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new UsuarioToken()
        {
            Authenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Message = "Token JWT OK"
        };

    }
}
