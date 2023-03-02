using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaFamilyLibrary.Infrastructure.Security
{
    public class AESEncryptionOption
    {
        public string? SecretParameterEncryptKey { get; set; }

        public string? SecretParameterIV { get; set; }
    }
}
