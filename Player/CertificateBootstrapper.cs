using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Player
{
    public static class CertificateBootstrapperRegistration
    {
        public static (string Password, string CertPath) CheckAndCreateCertificate(IConfiguration configuration)
        {
            string certFolder = configuration.GetValue<string>("CertificatePath") ?? "https/certs";
            certFolder = certFolder.Replace('/', Path.DirectorySeparatorChar);

            string fullpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, certFolder);
            string certPath = Path.Combine(fullpath, "certificate.pfx");
            string secretPath = Path.Combine(fullpath, "secret.txt");

            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            string password;

            if (File.Exists(secretPath))
            {
                password = File.ReadAllText(secretPath);
            }
            else
            {
                using (var rng = new RNGCryptoServiceProvider())
                {
                    byte[] randomBytes = new byte[32];
                    rng.GetBytes(randomBytes);
                    password = Convert.ToBase64String(randomBytes);
                    File.WriteAllText(secretPath, password);
                }
            }

            if (!File.Exists(certPath))
            {
                using (RSA rsa = RSA.Create(2048))
                {
                    var certRequest = new CertificateRequest("cn=simplemediaplayer.local", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                    var cert = certRequest.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
                    byte[] certData = cert.Export(X509ContentType.Pfx, password);
                    File.WriteAllBytes(certPath, certData);
                }
            }

            return (password, certPath);
        }

    }
}
