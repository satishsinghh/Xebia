using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Xebia.DatabaseCore.Encryption
{
    public interface IConfigurationEncryptionUtility
    {
        X509Certificate2 GetCertificate(string storeName, string certName);
        EncryptionOutput Decrypt(X509Certificate2 cert, EncryptionInput input);
        EncryptionOutput Encrypt(X509Certificate2 cert, EncryptionInput input);
    }

    public class ConfigurationEncryptionUtility
    {
        public X509Certificate2 GetCertificate(string storeName, string certName)
        {
            var store = new X509Store(storeName, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindBySubjectName, certName, false)[0];
            return cert;
        }

        public byte[] Decrypt(X509Certificate2 cert, EncryptionInput input)
        {
            byte[] decryptOutput = null;

            using (var rsa = cert.GetRSAPrivateKey())
            {
                var decryptedKey = rsa.Decrypt(Convert.FromBase64String(input.Key), RSAEncryptionPadding.OaepSHA1);
                var decryptedIV = rsa.Decrypt(Convert.FromBase64String(input.IV), RSAEncryptionPadding.OaepSHA1);

                using (RijndaelManaged rmCrypto = new RijndaelManaged())
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rmCrypto.CreateDecryptor(decryptedKey, decryptedIV), CryptoStreamMode.Write))
                        {
                            var dataBytes = Convert.FromBase64String(input.Data);

                            cryptoStream.Write(dataBytes, 0, dataBytes.Length);

                            cryptoStream.FlushFinalBlock();

                            decryptOutput = memoryStream.ToArray();

                            memoryStream.Close();

                            cryptoStream.Close();
                        }
                    }
                }
            }

            return decryptOutput;
        }
    }
}
