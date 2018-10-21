using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xebia.DatabaseCore.Encryption;

namespace Xebia.DatabaseCore.Configuration
{
    public class XebiaConfigProvider : ConfigurationProvider, IConfigurationSource
    {
        private string _path;

        public XebiaConfigProvider(string path)
        {
            _path = path;
        }

        public override void Load()
        {
            if (!File.Exists(_path))
            {
                throw new FileNotFoundException($"Configuration file not found at path {_path}");
            }

            var sourceFileText = File.ReadAllText(_path).Replace("\n", "").Replace("\r", "");

            var sourceData = JsonConvert.DeserializeObject<EncryptionInput>(sourceFileText);

            if (!ValidateSourceData(sourceData))
            {
                return;
            }

            Data = DecryptConfiguration(sourceData);
        }

        private IDictionary<string, string> DecryptConfiguration(EncryptionInput input)
        {
            var encryptUtil = new ConfigurationEncryptionUtility();

            var cert = encryptUtil.GetCertificate(input.CertStore, input.CertName);

            if (cert == null)
            {
                throw new ArgumentException($"Certificate not found");
            }

            var decryptedOutput = encryptUtil.Decrypt(cert, input);

            var jsonConfigurationFileParser = new JsonConfigurationFileParser();

            IDictionary<string, string> deserializedData = null;

            using (MemoryStream stream = new MemoryStream(decryptedOutput, 0, decryptedOutput.Length))
            {
                deserializedData = jsonConfigurationFileParser.Parse(stream);
            }

            return deserializedData;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new XebiaConfigProvider(_path);
        }

        private static bool ValidateSourceData(EncryptionInput input)
        {
            return input != null &&
                !string.IsNullOrEmpty(input.CertName) &&
                !string.IsNullOrEmpty(input.CertStore) &&
                input.Data != null &&
                input.Key != null &&
                input.IV != null;
        }
    }
}
