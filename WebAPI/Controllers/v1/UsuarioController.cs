using Aplicacao.Interfaces;
using Dominio.Interfaces;
using Dominio.Interfaces.Filas;
using Entidades.Entidades;
using Entidades.Enums;
using Insfraestrutura.Filas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebAPI.Models;
using WebAPI.Token;

namespace WebAPI.Controllers.v1;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IAplicacaoUsuario _IAplicacaoUsuario;
    private readonly IInsereNaFila _IInsereNaFila;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger _logger;

    public UsuarioController(IAplicacaoUsuario IAplicacaoUsuario, SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager, ILogger logger, IInsereNaFila insereNaFila)
    {
        _IAplicacaoUsuario = IAplicacaoUsuario;
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _IInsereNaFila = insereNaFila;
    }


    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost("/v1/User/Create")]
    public async Task<IActionResult> AdicionaUsuarioIdentity([FromBody] Login login)
    {
        var user = new ApplicationUser
        {
            UserName = login.userName,
            Email = login.email,
            Celular = login.celular,
            Tipo = TipoUsuario.Comum,
            NormalizedUserName = login.userName,
            PasswordHash = login.senha,
            DataDeNascimento = login.nascimento
        };

        _IInsereNaFila.Inserir(user, "InsertApplicationUser");

        return Ok("Usu�rio ser� inserido em breve");

        //if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
        //    return Ok("Falta alguns dados");


        //var resultado = await _userManager.CreateAsync(user, login.senha);

        //if (resultado.Errors.Any())
        //{
        //    return BadRequest(resultado.Errors);
        //}

        //// Gera��o de Confirma��o caso precise
        //var userId = await _userManager.GetUserIdAsync(user);
        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        //// retorno email 
        //code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        //var resultado2 = await _userManager.ConfirmEmailAsync(user, code);

        //if (resultado2.Succeeded)
        //    return Ok("Usu�rio Adicionado com Sucesso");
        //else
        //    return Ok("Erro ao confirmar usu�rios");
    }


    [AllowAnonymous]
    [Produces("application/json")]
    [HttpPost("/v1/User/Token")]
    public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
    {
        if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
            return Unauthorized();

        var resultado = await
            _signInManager.PasswordSignInAsync(login.userName, login.senha, false, lockoutOnFailure: false);

        if (resultado.Succeeded)
        {
            // muda no program.cs tambem
            var key = "Secret_Key-12345678_Secret_Key-12345678";

            var idUsuario = await _IAplicacaoUsuario.RetornaIdUsuario(login.email);

            var token = new TokenJWTBuilder()
                 .AddSecurityKey(JwtSecurityKey.Create(key))
             .AddSubject("Empresa - Canal Dev Net Core")
             .AddIssuer("Teste.Securiry.Bearer")
             .AddAudience("Teste.Securiry.Bearer")
             .AddClaim("idUsuario", idUsuario)
             .AddExpiry(172800000)
             .Builder();

            return Ok(token.value);
        }
        else
        {
            return Unauthorized();
        }

    }

    [Authorize]
    [Produces("application/json")]
    [HttpGet("/v1/User/Info")]
    public async Task<IActionResult> GetInfoAboutUser()
    {
        var email = User.Claims.FirstOrDefault().Subject.Name;
        var idUsuario = await _IAplicacaoUsuario.RetornaIdUsuario(email);

        var nomeDoUsuario = await _IAplicacaoUsuario.RetornaONomeDoUsuarioPorId(idUsuario);

        return Ok(new { name = nomeDoUsuario });
    }
}
