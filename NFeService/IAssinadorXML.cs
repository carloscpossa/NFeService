using System;
using System.Runtime.InteropServices;

namespace NFeService
{
    [InterfaceType(ComInterfaceType.InterfaceIsDual),
     Guid("0095E40B-8274-4C73-AA46-B913A596C108")]
    public interface IAssinadorXML
    {
        string AssinaXML(string aXML, string pUri, string URI, string serialCertificado);
    }
}
