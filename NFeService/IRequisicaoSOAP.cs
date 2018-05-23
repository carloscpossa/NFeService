using System;
using System.Runtime.InteropServices;

namespace NFeService
{
    [InterfaceType(ComInterfaceType.InterfaceIsDual),
     Guid("87D2BDB5-611B-4C69-874F-15C7B7D4B914")]
    public interface IRequisicaoSOAP
    {
        string Requisicao(string xml, string SOAPAction, string requestUri, string serialCertificado);
    }
}
