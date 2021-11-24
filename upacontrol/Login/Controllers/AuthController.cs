using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using upacontrol.Login.Repository;
using static upacontrol.Login.Models.AuthModels;

namespace upacontrol.Login.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        //construtor
        private IAuthRepository _authRepository;

        [EnableCors]
        [HttpPost("registro")]
        public async Task<ActionResult<RetornoTokenLogin>> Registro(RegistroNovoUsuario registroNovoUsuario)
        {
            //Testa se existe um usuário realmente tentando efetuar o login na plataforma
            if (!String.IsNullOrEmpty(registroNovoUsuario.nome) && !String.IsNullOrEmpty(registroNovoUsuario.email) && !String.IsNullOrEmpty(registroNovoUsuario.password))
            {
                if (registroNovoUsuario.password.Length >= 6)
                {
                    //Converte no objeto genérico de registro
                    var novoUsuario = new Models.AuthModels.Login
                    {
                        nome = registroNovoUsuario.nome,
                        email = registroNovoUsuario.email,
                        sexo = registroNovoUsuario.sexo,
                        password = registroNovoUsuario.password,
                        confirmpassword = registroNovoUsuario.confirmpassword,
                    };

                    //Manda a model de um novo usuário para o Repository fazer a mensageria.
                    var novoUsuarioCriado = await _authRepository.Registro(novoUsuario);

                    //Testa se o usuário foi criado nas tabelas do asp.net Identity
                    if (novoUsuarioCriado != null)
                    {
                        if (!String.IsNullOrEmpty(novoUsuarioCriado.id_aspnetuser))
                        {
                            //Loga na plataforma
                            var credenciais = new AccessCredentials()
                            {
                                nome = novoUsuarioCriado.nome,
                                user_id = novoUsuarioCriado.email,
                                password = novoUsuario.password,
                            };

                            //Valida as credenciais
                            bool validacredenciais = await _authRepository.Entrar(credenciais);

                            //Testa o resultado das credenciais       
                            if (validacredenciais)
                            {
                                //Se for validado e autenticado gera o token
                                var token = await _authRepository.GerarToken(credenciais);

                                if (token.token.Authenticated && token.token.AccessToken != null)
                                    return token;
                                else
                                    return Unauthorized($"Usuário não autorizado");
                            }
                            else
                            {
                                return Unauthorized($"Usuário não autorizado");
                            }
                        }
                        else
                        {
                            return UnprocessableEntity($"Problemas ao criar sua conta, entre em contato com nosso suporte.");
                        }
                    }
                    else
                    {
                        return UnprocessableEntity($"Email ja cadastrado.");
                    }
                }
                else
                {
                    return UnprocessableEntity($"A senha deve contar ao menos 6 dígitos.");
                }
            }
            else
            {
                return UnprocessableEntity($"Preencha todos os dados obrigatórios.");
            }
        }

        // POST: api/Login/
        [EnableCors]
        [HttpPost("login")]
        public async Task<ActionResult<RetornoTokenLogin>> Login(AccessCredentials credenciais)
        {
            //Valida as credenciais
            bool validacredenciaislogin = await _authRepository.Entrar(credenciais);

            //Busca usuario completo para login e para validar troca de senha com OAUTH com google ou facebook.
            var usuario = await _authRepository.BuscaUsuario(credenciais.user_id);

            //Testa o resultado das credenciais       
            if (validacredenciaislogin)
            {
                var cerencialogin = new AccessCredentials
                {
                    user_id = credenciais.user_id,
                    grant_type = credenciais.user_id,
                    nome = usuario.nome,
                    password = credenciais.password
                };

                //Se for validado e autenticado gera o token
                var token = await _authRepository.GerarToken(cerencialogin);

                if (token.token.Authenticated && token.token.AccessToken != null)
                    return token;
                else
                    return BadRequest($"Usuário não autorizado");
            }
            else if (!String.IsNullOrEmpty(credenciais.user_id) && !String.IsNullOrEmpty(credenciais.password))
                return Unauthorized($"E-mail ou senha estão incorretos.");
            else
                return BadRequest($"Preencha todos os dados obrigatórios.");
        }
    }
}

