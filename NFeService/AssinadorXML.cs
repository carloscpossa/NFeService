using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace NFeService
{

    [ClassInterface(ClassInterfaceType.None),
     Guid("3A8B6B96-4F23-4EB5-B4B1-243BFBC41A39"),
     ComVisible(true)]
    public class AssinadorXML : IAssinadorXML
    {
        private string vResultadoString = "";
        private String vXMLStringAssinado;
        private int vResultado = 0;

        public AssinadorXML()
        {
        }

        public string AssinaXML(string aXML, string pUri, string URI, string serialCertificado)
        {
            // open the XML file 
            //StreamReader SR = File.OpenText(pArqXMLAssinar);
            String vXMLString = aXML;
            //SR.Close();

            // return parameters
            this.vResultado = 0;
            this.vResultadoString = "Assinatura realizada com sucesso";
            this.vXMLStringAssinado = String.Empty;

            try
            {
                // checking if there is a certified used on xml sign
                string _xnome = "";
                bool vRetorna;

                X509Certificate2 _X509Cert = CertificadoDigital.ObtemCertificadoDigital(serialCertificado);

                if (_X509Cert == null)
                {
                    vRetorna = false;
                }
                else
                {                    
                    string x;
                    x = _X509Cert.GetKeyAlgorithm().ToString();

                    // Create a new XML document.
                    XmlDocument doc = new XmlDocument();

                    // Format the document to ignore white spaces.
                    doc.PreserveWhitespace = false;

                    // Load the passed XML file using it’s name.
                    try
                    {
                        doc.LoadXml(vXMLString);

                        // cheching the elemento will be sign 
                        int qtdeRefUri = doc.GetElementsByTagName(pUri).Count;

                        if (qtdeRefUri == 0)
                        {
                            this.vResultado = 4;
                            this.vResultadoString = "A tag de assinatura " + pUri.Trim() + " não existe";
                        }
                        else
                        {
                            if (qtdeRefUri > 1)
                            {
                                this.vResultado = 5;
                                this.vResultadoString = "A tag de assinatura " + pUri.Trim() + " não é unica";
                            }
                            else
                            {
                                try
                                {
                                    // Create a SignedXml object.
                                    SignedXml signedXml = new SignedXml(doc);

                                    // Add the key to the SignedXml document
                                    signedXml.SigningKey = _X509Cert.PrivateKey;

                                    // Create a reference to be signed
                                    Reference reference = new Reference();
                                    reference.Uri = URI;

                                    /*if (pUri.Trim() != "")
                                    {
                                        XmlAttributeCollection _Uri = doc.GetElementsByTagName(pUri).Item(0).Attributes;

                                        foreach (XmlAttribute _atributo in _Uri)
                                        {
                                            if (_atributo.Name == "Id")
                                                reference.Uri = "#" + _atributo.InnerText;
                                        }
                                    }
                                    else
                                    {
                                        reference.Uri = "";
                                    }*/

                                    // Add an enveloped transformation to the reference.
                                    XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                                    reference.AddTransform(env);

                                    XmlDsigC14NTransform c14 = new XmlDsigC14NTransform();
                                    reference.AddTransform(c14);

                                    // Add the reference to the SignedXml object.
                                    signedXml.AddReference(reference);

                                    // Create a new KeyInfo object
                                    KeyInfo keyInfo = new KeyInfo();

                                    // Load the certificate into a KeyInfoX509Data object
                                    // and add it to the KeyInfo object.
                                    keyInfo.AddClause(new KeyInfoX509Data(_X509Cert));

                                    // Add the KeyInfo object to the SignedXml object.
                                    signedXml.KeyInfo = keyInfo;
                                    signedXml.ComputeSignature();

                                    // Get the XML representation of the signature and save
                                    // it to an XmlElement object.
                                    XmlElement xmlDigitalSignature = signedXml.GetXml();

                                    // save element on XML 
                                    doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

                                    XmlDocument XMLDoc = new XmlDocument();
                                    XMLDoc.PreserveWhitespace = false;
                                    XMLDoc = doc;

                                    // XML document already signed 
                                    vXMLStringAssinado = XMLDoc.OuterXml;
                                }
                                catch (Exception caught)
                                {
                                    this.vResultado = 6;
                                    this.vResultadoString = "Erro ao assinar o documento - " + caught.Message;
                                }
                            }
                        }
                    }
                    catch (Exception caught)
                    {
                        this.vResultado = 3;
                        this.vResultadoString = "XML mal formado - " + caught.Message;
                    }
                }
            }
            catch (Exception caught)
            {
                this.vResultado = 1;
                this.vResultadoString = "Problema ao acessar o certificado digital" + caught.Message;
            }

            return vXMLStringAssinado;
        }
    }
}
