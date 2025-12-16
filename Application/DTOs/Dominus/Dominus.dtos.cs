namespace Aplication.DTOs.Dominus
{

    //Generacion de token de autenticacion
    public class RequestToken
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
        public string scope { get; set; }
    }

    public class ResponseToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    //1. Endpoint listado de consolidados
    public class RequestListadoConsolidados
    {
        public int branch_id { get; set; }
        public DateOnly? start_date { get; set; } // Formato: YYYY-MM-DD
        public DateOnly? final_date { get; set; } // Formato: YYYY-MM-DD
    }
    
    public class ResponseListadoConsolidados
    {
        public MensajeRetorno messages { get; set; } = new();
        public ConsolidadoData data { get; set; } = new();
    }

    public class MensajeRetorno
    {
        public int error { get; set; }
        public string msg { get; set; }
        public bool debug { get; set; }
        public string detail { get; set; }
    }

    public class ConsolidadoData
    {
        public List<ItemConsolidado> consolidated_list { get; set; } = [];
    }

    public class ItemConsolidado
    {
        public int id { get; set; }
        public string name { get; set; }
    }

     //2. Endpoint consulta de ventas por consolidado
    public class RequestConsultaVentasConsolidado
    {
        public string consolidated_id { get; set; }
    }

    public class ConsultaVentasConsolidadoResponse
    {
        public MensajeRetorno messages { get; set; } = new();
        public List<DataSales> data { get; set; } = [];
    }

    public class DataSales
    {
        public DateOnly date { get; set; } //verificar formato de fecha
        public List<Sales> sales { get; set; }
    }

    public class Sales
    {
        public int id { get; set; }
        public int branch_id { get; set; }
        public int document { get; set; }
        public string prefix { get; set; }
        public DateTime date_sale { get; set; } //verificar formato de fecha
        public int is_bill { get; set; }
        public int is_enable { get; set; }
        public string wildcard { get; set; }
        public string validator { get; set; }
        public string odemeter { get; set; }
        public string plate { get; set; }
        public decimal total { get; set; }
        public int shift_id { get; set; }
        public int consolidated_id { get; set; }
        public string bill_prefix { get; set; }
        public string bill_number { get; set; }
        public int default_payment_code { get; set; }
        public string default_payment_name { get; set; }
        public decimal default_payment_value { get; set; }
        public string customer_document { get; set; }
        public string customer_first_name { get; set; }
        public string customer_last_name { get; set; }
        public string customer_address { get; set; }
        public string customer_phone { get; set; }
        public string employeee_document { get; set; }
        public string employeee_first_name { get; set; }
        public string employeee_last_name { get; set; }
        public string terminal_code { get; set; }
        public string terminal_name { get; set; }
        public string firmware_version { get; set; }
        public string hose { get; set; }
        public string face { get; set; }
        public string dependence { get; set; }
        public int start_number { get; set; }
        public int final_number { get; set; }
        public string resolution { get; set; }
        public DateOnly resolution_date { get; set; } //Formato YYYY-MM-DD
        public DateOnly begining_date { get; set; } //Formato YYYY-MM-DD
        public DateOnly expires_date { get; set; } //Formato YYYY-MM-DD
        public int type { get; set; }
        public List<Products> products { get; set; }
        public List<Payments> payment { get; set; }
        public List<Closes> closes { get; set; }
    }

    public class Products
    {
        public int id { get; set; }
        public int sale_id { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string presentation { get; set; }
        public int quantify { get; set; }
        public decimal price { get; set; }
        public decimal subtotal { get; set; }
        public decimal total { get; set; }
        public decimal iva { get; set; }
        public decimal impoconsumo { get; set; }
        public List<Taxes> taxes { get; set; }
    }

    public class Taxes
    {
        public int id { get; set; }
        public int productref_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int tax_value { get; set; }
        public int tax_id { get; set; }
        public string label { get; set; }
        public int tax_type { get; set; }
        public int mode { get; set; }
        public string integration_code { get; set; }
        public string value { get; set; }
    }

    public class Payments
    {
        public int id { get; set; }
        public int sale_id { get; set; }
        public decimal value { get; set; }
        public int code { get; set; }
        public string name { get; set; }
    }

    public class Closes
    {
        public int id { get; set; }
        public DateTime date_close_ini { get; set; } //verificar formato de fecha
        public DateTime date_close_end { get; set; } //verificar formato de fecha
        public string pcc { get; set; }
        public string panel { get; set; }
        public string seller { get; set; }
        public string document { get; set; }
        public int volume_total { get; set; }
        public decimal money_total { get; set; }
        public int tmp_close_id { get; set; }
        public List<ClosesHoses> closehose { get; set; }
    }

    public class ClosesHoses
    {
        public int id { get; set; }
        public int closesummary_id { get; set; }
        public int productref_id { get; set; }
        public string productref { get; set; }
        public int volume_ini { get; set; }
        public int volume_end { get; set; }
        public int volume_total { get; set; }
        public decimal money_ini { get; set; }
        public decimal money_end { get; set; }
        public decimal money_total { get; set; }
        public int ppu { get; set; }
        public int face { get; set; }
        public int hose { get; set; }
    }

    public class ClavesDominus
    {
        public string ClientId {get; set;}
        public string ClientSecrect {get; set;}
        public string GrantType {get; set;}
        public string Scope {get; set;}
    }
}