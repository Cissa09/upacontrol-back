using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace upacontrol.Login.Models
{
    public class AuthModels
    {
        #region Registro

        [Table("Usuario")]
        public class Usuario
        {
            [Key]
            public int id { get; set; }
            [MaxLength(150, ErrorMessage = "Nome deve conter no máximo 150 caracteres")]
            public string nome { get; set; }
            public string email { get; set; }
            public string sexo { get; set; }
            public string id_aspnetuser { get; set; }
            public int tipo_usuario { get; set; }
        }

        public class UsuarioNovo
        {           
            public int id { get; set; }
            [MaxLength(150, ErrorMessage = "Nome deve conter no máximo 150 caracteres")]
            public string nome { get; set; }
            public string email { get; set; }
            public string sexo { get; set; }
            public string id_aspnetuser { get; set; }
            public string password { get; set; }
            public string confirmpassword { get; set; }         
            public int tipo_usuario { get; set; }
        }

        public class UsuarioToken
        {
            public int id { get; set; }
            [MaxLength(150, ErrorMessage = "Nome deve conter no máximo 150 caracteres")]
            public string nome { get; set; }
            public string sexo { get; set; }
            public string email { get; set; }
            public string id_aspnetuser { get; set; }
            public Token token { get; set; }
        }
        
        public class RegistroNovoUsuario
        {
            [MaxLength(150, ErrorMessage = "Nome deve conter no máximo 150 caracteres")]
            public string nome { get; set; }
            public string email { get; set; }
            public string id_aspnetuser { get; set; }
            public string sexo { get; set; }                              
            public string password { get; set; }
            public string confirmpassword { get; set; }
            public int tipo_usuario { get; set; }
        }

        #endregion

        #region Login

        public class Login
        {
            [MaxLength(150, ErrorMessage = "Nome deve conter no máximo 150 caracteres")]
            public string nome { get; set; }
            public string email { get; set; }
            public string sexo { get; set; }               
            public string password { get; set; }         
            public string confirmpassword { get; set; }              
        }
        
        #endregion

        public class RetornoTokenLogin
        {
            public Token token { get; set; }
        }

        public class Token
        {
            public bool Authenticated { get; set; }
            public string Created { get; set; }
            public string Expiration { get; set; }
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public string Message { get; set; }
        }

        public class AccessCredentials
        {
            public string nome { get; set; }
            public string user_id { get; set; }  
            public string password { get; set; }  
            public string grant_type { get; set; }   
        }

        public class MagicLink
        {
            public string nome { get; set; }
            public string email { get; set; }    
        }

        public class TokenConfigurations
        {
            public string Audience { get; set; }
            public string Issuer { get; set; }
            public int Seconds { get; set; }
            public int FinalExpiration { get; set; }
            public int MagicLink { get; set; }
        }

        public class RefreshTokenData
        {
            public string RefreshToken { get; set; }
            public string UserID { get; set; }
        }

        public class PublicToken
        {
            public string PublicKey { get; set; }
            public string PublicKeyOrcamento { get; set; }
        }
    }
}
