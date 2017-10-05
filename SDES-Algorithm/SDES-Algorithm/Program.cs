using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDES_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            SDESMethods Encryption = new SDESMethods();
            var n = Encryption.Ciphertext("11100010");
            var d = Encryption.Desencrypted(n);
        }
    }
}
