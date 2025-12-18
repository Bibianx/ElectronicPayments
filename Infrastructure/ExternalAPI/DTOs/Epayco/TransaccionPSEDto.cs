namespace Infraestructure.ExternalAPI.DTOs.Epayco
{
    public class TransactionRequest
    {
        public string bank { get; set; }
        public string value { get; set; }
        public string docType { get; set; }
        public string docNumber { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string cellPhone { get; set; }
        public string address { get; set; }
        public string ip { get; set; }
        public string urlResponse { get; set; }
        public string phone { get; set; }
        public float tax { get; set; }
        public float taxBase { get; set; }
        public string description { get; set; }
        public string invoice { get; set; }
        public string currency { get; set; }
        public string typePerson { get; set; }
        public string urlConfirmation { get; set; }
        public string methodConfimation { get; set; }
        public bool testMode { get; set; }
        public string extra1 { get; set; }
        public string extra2 { get; set; }
        public string extra3 { get; set; }
        public string extra4 { get; set; }
        public string extra5 { get; set; }
    }

     public class TransactionResponse : MessageResponse
    {
        public TransactionData data { get; set; }
    }

    public class TransactionData
    {
        public int ref_payco { get; set; }
        public string factura { get; set; }
        public string descripcion { get; set; }
        public float valor { get; set; }
        public float iva { get; set; }
        public float ico { get; set; }
        public float baseiva { get; set; }
        public string moneda { get; set; }
        public string estado { get; set; }
        public string respuesta { get; set; }
        public int cod_respuesta { get; set; }
        public string cod_error { get; set; }
        public string autorizacion { get; set; }
        public string ciudad { get; set; }
        public string recibo { get; set; }
        public string fecha { get; set; }
        public string urlbanco { get; set; }
        public string transactionID { get; set; }
        public string ticketId { get; set; }
        public TransactionPaymentExtras extras { get; set; }
        public string ciclo { get; set; }
    }

    public class TransactionPaymentExtras
    {
        public string extra1 { get; set; }
        public string extra2 { get; set; }
        public string extra3 { get; set; }
        public string extra4 { get; set; }
        public string extra5 { get; set; }
        public string extra6 { get; set; }
        public string extra7 { get; set; }
        public string extra8 { get; set; }
        public string extra9 { get; set; }
        public string extra10 { get; set; }
    }
    public class MessageResponse
    {
        public bool success { get; set; }
        public string titleResponse { get; set; }
        public string textResponse { get; set; }
        public string lastAction { get; set; }
    }

}