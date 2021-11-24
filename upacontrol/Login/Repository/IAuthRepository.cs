using System.Threading.Tasks;
using static upacontrol.Login.Models.AuthModels;

namespace upacontrol.Login.Repository
{
    public interface IAuthRepository 
    {
        //Cria um usuario novo através do formulário (Não vamos desenvolver na versão da entrega para UNISINOS), criaremos o usuário via POSTMAN 
        Task<Models.AuthModels.UsuarioToken> Registro(Models.AuthModels.Login registro);        
        //Logar
        Task<bool> Entrar(AccessCredentials credenciais);
        //Gerar Json Web Token
        Task<RetornoTokenLogin> GerarToken(AccessCredentials credenciais);
        //Busca Usuario
        Task<Usuario> BuscaUsuario(string user_id);
    }
}
