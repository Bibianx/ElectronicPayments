namespace Infraestructure.ExternalAPI.DTOs.Epayco
{
    public class TransactionConfirmRequest
    {
        public int transactionID { get; set; }
    }

    public class TransactionConfirmResponse : MessageResponse
    {
        public TransactionConfirmData data { get; set; }
    }

    public class TransactionConfirmData : TransactionData
    {
        public string franquicia { get; set; }
        public string ip { get; set; }
        public int enpruebas { get; set; }
        public string tipo_doc { get; set; }
        public string documento { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string email { get; set; }
        public string direccion { get; set; }
        public string ind_pais { get; set; }
        public string country_card { get; set; }
        public CCNetwork cc_network_response { get; set; }
        public new string transactionID { get; set; }
        public new long? ticketId { get; set; }
    }

    public class CCNetwork
    {
        public string code { get; set; }
        public string message { get; set; }
    }    
}