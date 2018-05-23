using System;
using System.Security.Cryptography.X509Certificates;

namespace NFeService
{
    public class CertificadoDigital
    {
        public static X509Certificate2 ObtemCertificadoDigital(string serialCertificado)
        {
            X509Certificate2 _X509Cert = new X509Certificate2();
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            X509Certificate2Collection collection2 = (X509Certificate2Collection)collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

            X509Certificate2Collection scollection = collection2;

            if (!string.IsNullOrEmpty(serialCertificado))
            {
                scollection = collection2.Find(X509FindType.FindBySerialNumber, serialCertificado, false);
            }
            else
            {
                scollection = X509Certificate2UI.SelectFromCollection(collection2, "Certificado(s) Digital(is) disponível(is)", "Selecione o certificado digital para uso no aplicativo", X509SelectionFlag.SingleSelection);
            }

            if (scollection.Count > 0)
                return scollection[0];
            else
                return null;
        }
    }
}
