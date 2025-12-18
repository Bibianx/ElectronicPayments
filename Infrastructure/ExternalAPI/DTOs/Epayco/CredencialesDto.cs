namespace Infraestructure.ExternalAPI.DTOs.Epayco
{
    public class EpaycoCredentials
    {
        public string PCustIdCliente { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string PKey { get; set; }
    }

    public class TokenResponse
    {
        public string token { get; set; }
    }

}