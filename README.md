# NFeService
Assembly COM (interoperável) que realiza assinatura digital de XML de notas fiscais eletrônicas e requisições SOAP.

# Objetivos
- Fornecer uma ideia para quem precisa realizar a assinatura digital em documentos XML utilizando .NET (C#).
- Fornecer um modo de se realizar requisições HTTPS utilizando o protocolo TLS 1.2 e certificados digitais. É útil para quem trabalha com linguagens de programação que não tem suporte a TLS 1.2.

# Observações
- Caso haja a necessidade de se utilizar esta implementação em um projeto que está em produção, será necessário criar um projeto de instalação (Windows Installer) para que o assembly possa ser registrado no Windows.
- Pode ser útil, nesse projeto de instalação, adicionar a pasta do GAC (Add Special Folder, Global Assembly Cache Folder) para que o sistema desenvolvido em outra liguagem consiga interoperar sem problemas de registro de classes (Erros "class id not registered").


