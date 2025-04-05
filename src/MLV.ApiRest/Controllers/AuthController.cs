using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MLV.ApiRest.Extensions;
using MLV.ApiRest.ViewModels;
using MLV.Business.Commands;
using MLV.Core.Mediator;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MLV.ApiRest.Controllers;

[Authorize]
[ApiController]
[Route("api/auth")]
public class AuthController(SignInManager<IdentityUser> signInManager,
                      UserManager<IdentityUser> userManager,
                      IMediatorHandler mediatorHandler,
                      IOptions<JwtSettings> jwtSettings) : MainController
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    [HttpGet("listar")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<IdentityUser>))]
    public async Task<ActionResult> ObterUsuario()
    {
        var result = await userManager.Users.ToListAsync();

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioRespostaLogin))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
    public async Task<ActionResult> Login(UsuarioLogin userLogin)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await signInManager.PasswordSignInAsync(userLogin.UserName, userLogin.Senha,
            false, true);

        if (result.Succeeded)
        {
            var user = await userManager.GetUserAsync(User);

            var token = await GerarJwt(userLogin.UserName);

            var userResponse = await CarregarUserResponse(user, token);

            return Ok(userResponse);
        }

        if (result.IsLockedOut)
        {
            return BadRequest("Usuário bloqueado por muitas tentativas.");
        }

        return BadRequest("Usuário ou senha incorretos.");
    }

    [AllowAnonymous]
    [HttpPost("registrar")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioRespostaLogin))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
    public async Task<ActionResult> Registrar(UsuarioRegistro newUser)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = new IdentityUser
        {
            UserName = newUser.UserName,
            Email = newUser.UserName,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, newUser.Senha);

        if (result.Succeeded)
        {
            var token = await GerarJwt(user.UserName);

            var usuarioResposta = await CarregarUserResponse(user, token);

            var resultVendedor = await mediatorHandler.EnviarComando(new VendedorCommand
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email,
                NomeFantasia = user.UserName
            });

            if (!resultVendedor.IsValid)
                return CustomResponse(resultVendedor);

            return Ok(usuarioResposta);
        }

        return BadRequest(result.Errors.Select(e => e.Description));
    }

    [HttpDelete("{userName}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<ActionResult> Remover(string userName)
    {
        var userIdentity = await userManager.Users.FirstOrDefaultAsync(a => a.UserName == userName);

        if (userIdentity == null)
            return NotFound();

        var roles = await userManager.GetRolesAsync(userIdentity);
        var result = await userManager.RemoveFromRolesAsync(userIdentity, roles);

        if (result.Succeeded)
        {
            result = await userManager.DeleteAsync(userIdentity);

            if (result.Succeeded) return NoContent();
        }

        return BadRequest(result.Errors.Select(e => e.Description));

    }

    private async Task<UsuarioRespostaLogin> CarregarUserResponse(IdentityUser user, string token)
    {
        var role = await userManager.GetRolesAsync(user);
        return new UsuarioRespostaLogin
        {
            Email = user.Email,
            UserName = user.UserName,
            Role = role.FirstOrDefault(),
            Token = token
        };
    }

    private async Task<string> GerarJwt(string usuarioNome)
    {
        var user = await userManager.FindByNameAsync(usuarioNome);
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

        // Adicionar roles como claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtSettings.Emissor,
            Audience = _jwtSettings.Audiencia,
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return encodedToken;
    }
}
