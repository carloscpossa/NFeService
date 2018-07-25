using System;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NFeService
{
    [ClassInterface(ClassInterfaceType.None),
     Guid("FEEAAB09-7EA8-4794-A978-D1CD72BA68B2"),
     ComVisible(true)]
    public class RequisicaoSOAP : IRequisicaoSOAP
    {
        public string Requisicao(string xml, string SOAPAction, string requestUri, string serialCertificado)
        {
            return RequisicaoLTS(xml, SOAPAction, requestUri, serialCertificado).Result;
        }

        private async Task<string> RequisicaoLTS(string xml, string SOAPAction, string requestUri, string serialCertificado)
        {
            
            try
            {
                var certificado = CertificadoDigital.ObtemCertificadoDigital(serialCertificado);

                WebRequestHandler handler = new WebRequestHandler();
                handler.ClientCertificates.Add(certificado);

                var content = new StringContent(xml, Encoding.UTF8, "text/xml");

                using (HttpClient client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Add("SOAPAction", SOAPAction);

                    using (var resposta = await client.PostAsync(requestUri, content))
                    {
                        using (var conteudoResposta = resposta.Content)
                        {
                            var resp = await conteudoResposta.ReadAsStringAsync();
                            return resp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return ex.Message + " - " + ex.InnerException.Message;
                }
                else
                {
                    return ex.Message;
                }
            }
        }

    }
}
