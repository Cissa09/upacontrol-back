using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using upacontrol.Data;
using upacontrol.Login.Models;
using upacontrol.Login.Security;
using static upacontrol.Login.Models.AuthModels;

namespace upacontrol.Login.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private APIContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private TokenConfigurations _tokenConfigurations;
        private SigningConfigurations _signingConfigurations;
        private IDistributedCache _cache;

        public AuthRepository(
          APIContext context,
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          TokenConfigurations tokenConfigurations,
          IDistributedCache cache,
          SigningConfigurations signingConfigurations)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenConfigurations = tokenConfigurations;
            _cache = cache;
        }

        //Cria um usuário e loga
        public async Task<Models.AuthModels.UsuarioToken> Registro(Models.AuthModels.Login usuarioCompleto)
        {
            var result = new IdentityResult();

            //Cria novo applicationuser 
            var user = new ApplicationUser
            {
                UserName = usuarioCompleto.email,
                Email = usuarioCompleto.email,
            };

            //Cria novo AspNetUser
            result = await _userManager.CreateAsync(user, usuarioCompleto.password);

            //Se criou com sucesso
            if (result.Succeeded)
            {
                //Cadastra na tabela usuário          
                var usuarioNovo = new Usuario
                {
                    nome = usuarioCompleto.nome,
                    sexo = usuarioCompleto.sexo,
                    id_aspnetuser = user.Id
                };

                //Salva o Usuario
                var userNovoCriado = await CriaUsuario(usuarioNovo);

                var resultadoUsuarioNovo = new UsuarioToken
                {
                    id = userNovoCriado.id,
                    nome = userNovoCriado.nome,
                    email = usuarioCompleto.email,
                    sexo = userNovoCriado.sexo,
                    id_aspnetuser = userNovoCriado.id_aspnetuser,                   
                };

                //retorno para o BFF                  
                return resultadoUsuarioNovo;
            }
            else
            {
                return null;
            }
        }
        
        //Apenas Faz o Login 
        public async Task<bool> Entrar(AccessCredentials credenciais)
        {
            //Cria a variável de retorno estanciada como falso, para que somente se houver fator real seu valor satisfaça a condição 
            bool credenciaisValidas = false;

            //Testa se existe um usuário realmente tentando efetuar o login na plataforma
            if (credenciais != null && !String.IsNullOrWhiteSpace(credenciais.user_id))
            {

                //Verifica a existência do usuário nas tabelas do ASP.NET Core Identity
                var userIdentity = _userManager.FindByNameAsync(credenciais.user_id).Result;

                if(userIdentity != null)
                {
                    var signInResult = _signInManager.CheckPasswordSignInAsync(userIdentity, credenciais.password, false).Result;

                    if (signInResult.Succeeded)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }                            
            }
            return credenciaisValidas;
        }
        
        public async Task<RetornoTokenLogin> GerarToken(AccessCredentials credenciais)
        {
            //Busca usuario para adicinar novas claims            
            var usuario = await BuscaUsuario(credenciais.user_id);

            //Cria as Claims que serão incluidas no PlayLoad do Token
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(credenciais.user_id, "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim("user", credenciais.user_id),             
                        new Claim("nome", usuario.nome),
                        new Claim("sexo", !String.IsNullOrEmpty(usuario.sexo) ? usuario.sexo : "")                                                    
                }
            );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao,
            });

            var token = handler.WriteToken(securityToken);

            var resultado = new AuthModels.Token()
            {
                Authenticated = true,
                Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                AccessToken = token,
                RefreshToken = Guid.NewGuid().ToString().Replace("-", String.Empty),
                Message = "OK",
            };

            // Armazena o refresh token em cache através do Redis 
            var refreshTokenData = new RefreshTokenData();
            refreshTokenData.RefreshToken = resultado.RefreshToken;
            refreshTokenData.UserID = credenciais.user_id;

            // Calcula o tempo máximo de validade do refresh token
            // (o mesmo será invalidado automaticamente pelo Redis)
            TimeSpan finalExpiration =
                TimeSpan.FromSeconds(_tokenConfigurations.FinalExpiration);

            DistributedCacheEntryOptions opcoesCache =
                new DistributedCacheEntryOptions();
            opcoesCache.SetAbsoluteExpiration(finalExpiration);
            _cache.SetString(resultado.RefreshToken,
                JsonConvert.SerializeObject(refreshTokenData),
                opcoesCache);

            var resultadologintoken = new RetornoTokenLogin
            {               
                token = resultado,
            };

            return resultadologintoken;
        }
        
        private async Task<Usuario> CriaUsuario(Usuario user)
        {
            var usuarioNovo = new Usuario();

            usuarioNovo.nome = user.nome;
            usuarioNovo.sexo = user.sexo;         
            usuarioNovo.id_aspnetuser = user.id_aspnetuser;          

            _context.Add(usuarioNovo);

            if (usuarioNovo.id <= 0)
            {
                _context.SaveChanges();
            }

            return usuarioNovo;
        }

        public async Task<Usuario> BuscaUsuario(string user_id)
        {
            //Busca o usuario dentro da tabela ASP.NET User
            var userIdentity = _userManager.FindByNameAsync(user_id).Result;
            //Busca usuario dentro da tabela usuário pelo id_aspnetuser
            var usuario = _context.Usuario.Where(user => user.id_aspnetuser == userIdentity.Id).First();

            //Monta o retorno do usuário que ja foi criado
            var usuarioNovo = new Usuario
            {
                id = usuario.id,
                nome = usuario.nome,
                sexo = usuario.sexo,
                id_aspnetuser = usuario.id_aspnetuser
            };

            if (usuarioNovo != null)
                return usuarioNovo;
            else
                return null;                      
        }
    }
}
