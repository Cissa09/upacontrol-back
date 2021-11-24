using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace upacontrol.Login.Security
{
    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations(string publicKey)
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(publicKey));
        }
    }
}
