using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.DatabaseCore.Encryption
{
    public class EncryptionOutput
    {
        public string CertStore { get; set; }
        public string CertName { get; set; }
        public string Data { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }
    }
}
