using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SdoValidering
{
    class Program
    {
        const string _signingCertificateIdentifier = "CN=www.esignatur.dk";

        static void Main(string[] args)
        {
#if DEBUG
            args = new string[] { @"c:\Users\Brian Schau\Desktop\Signeringsbevis.xml" };
#endif

            if (args.Length != 1)
            {
                Console.WriteLine(@"Brug: SdoValidering.exe \sti\til\Signeringsbevis.xml");
                Environment.Exit(1);
            }

            var signedDocument = XDocument.Load(args[0], LoadOptions.PreserveWhitespace);
            var document = new XmlDocument { PreserveWhitespace = true };
            document.LoadXml(signedDocument.ToString(SaveOptions.DisableFormatting));
            var signedXml = new SignedXml(document);
            signedXml.LoadXml(document.DocumentElement);

            var keyInfoData = signedXml.Signature.KeyInfo.OfType<KeyInfoX509Data>();
            var certificate = keyInfoData.First().Certificates[0] as X509Certificate2;

            // Only for the paranoid
            ValidateCertificateChain(signedXml.KeyInfo, certificate);

            var verified = signedXml.CheckSignature(certificate, verifySignatureOnly: false);
            Console.WriteLine("Certificate Status: {0}", verified);
#if DEBUG
            Console.ReadKey();
#endif
        }

        static void ValidateCertificateChain(KeyInfo keyInfo, X509Certificate2 signingCertificate)
        {
            var certificates = keyInfo.Cast<KeyInfoX509Data>().Select(x => (X509Certificate2)x.Certificates[0]).ToList();
            var orderedCertificates = new List<X509Certificate2>();
            var current = signingCertificate;
            while (!current.Subject.Equals(current.Issuer))
            {
                var targets = certificates.Where(x => x.Subject.Contains(current.Issuer)).ToList();
                if (!targets.Any())
                {
                    Console.WriteLine("Current Issuer '{0}' not found", current.Issuer);
                    Environment.Exit(2);
                }

                current = targets.First();
                orderedCertificates.Insert(0, current);
            }

            var chain = new X509Chain();
            chain.ChainPolicy.ExtraStore.AddRange(orderedCertificates.ToArray());
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;

            var status = chain.Build(signingCertificate);
            if (status == false)
            {
                Console.WriteLine("Invalid chain!");
                Environment.Exit(3);
            }
        }
    }
}
