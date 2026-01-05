namespace Infraestructure.ExternalAPI.DTOs.Dominus
{
    public class ConsultaVentasConsolidadoParams
    {
        public string consolidated_id { get; set; }
    }

    public class ConsultaVentasConsolidadoResponse
    {
        public MensajeRetorno messages { get; set; } = new();
        public DataSales data { get; set; } = new();
    }

    public class DataSales
    {
        public string date { get; set; } 
        public List<Sales> sales { get; set; }
        public List<PaymentEmployee> payments_employee { get; set; }
        public List<Closes> closes { get; set; }
    }

    public class Sales
    {
        public int? id { get; set; }
        public int? branch_id { get; set; }
        public string prefix { get; set; }
        public int? document { get; set; }
        public string date_sale { get; set; }
        public int? is_bill { get; set; }
        public int? is_enabled { get; set; }
        public string wildcard { get; set; }
        public string validator { get; set; }
        public int? odometer { get; set; }
        public string plate { get; set; }
        public decimal? total { get; set; }
        public int? shift_id { get; set; }
        public int? closesummary_id { get; set; }
        public int? consolidated_id { get; set; }
        public string bill_prefix { get; set; }
        public string bill_number { get; set; }
        public string cufe { get; set; }
        public int? default_payment_code { get; set; }
        public string default_payment_name { get; set; }
        public decimal? default_payment_value { get; set; }
        public int? agreement_code { get; set; }
        public string agreement { get; set; }
        public int? person_type { get; set; }
        public int? person_type_id { get; set; }
        public string customer_document { get; set; }
        public string customer_first_name { get; set; }
        public string customer_last_name { get; set; }
        public string customer_address { get; set; }
        public string customer_phone { get; set; }
        public int? person_id { get; set; }
        public string employee_document { get; set; }
        public string employee_first_name { get; set; }
        public string employee_last_name { get; set; }
        public string terminal_code { get; set; }
        public string terminal_name { get; set; }
        public string firmware_version { get; set; }
        public int? hose { get; set; }
        public int? face { get; set; }
        public string dependence { get; set; }
        public int? start_number { get; set; }
        public int? final_number { get; set; }
        public string resolution { get; set; }
        public string resolution_date { get; set; }
        public string begining_date { get; set; }
        public string expires_date { get; set; }
        public int? type { get; set; }
        public long? ebpeople_id { get; set; }
        public string email_delivery { get; set; }
        public string postal_code { get; set; }
        public List<Products> products { get; set; }
        public List<PaymentMethod> payment { get; set; }
    }

    public class Products
    {
        public int? id { get; set; }
        public int? sale_id { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string presentation { get; set; }
        public decimal? quantity { get; set; }
        public decimal? price { get; set; }
        public decimal? base_price { get; set; }
        public decimal? subtotal { get; set; }
        public decimal? total { get; set; }
        public decimal? iva { get; set; }
        public decimal? impoconsumo { get; set; }
        public List<Taxes> taxes { get; set; } = [];
    }

    public class Taxes
    {
        public int? id { get; set; }
        public int? productref_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public decimal? tax_value { get; set; }
        public int? tax_id { get; set; }
        public string label { get; set; }
        public int? tax_type { get; set; }
        public int? mode { get; set; }
        public string integration_code { get; set; }
        public decimal? value { get; set; }
    }

    public class PaymentMethod
    {
        public int? id { get; set; }
        public int? sale_id { get; set; }
        public string approval_code { get; set; }
        public decimal? value { get; set; }
        public int? code { get; set; }
        public string name { get; set; }
    }

    public class PaymentEmployee
    {
        public int? shift_id { get; set; }
        public string value { get; set; }
        public int? code { get; set; }
        public string name { get; set; }
        public string document { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class Closes
    {
        public int? id { get; set; }
        public string date_close_ini { get; set; }
        public string date_close_end { get; set; }
        public string pcc { get; set; }
        public string panel { get; set; }
        public string seller { get; set; }
        public string document { get; set; }
        public decimal? volume_total { get; set; }
        public decimal? money_total { get; set; }
        public int? tmp_close_id { get; set; }
        public List<CloseHose> close_hoses { get; set; }
    }

    public class CloseHose
    {
        public int? id { get; set; }
        public int? closesummary_id { get; set; }
        public int? productref_id { get; set; }
        public string productref { get; set; }
        public int? volume_ini { get; set; }
        public int? volume_end { get; set; }
        public int? volume_total { get; set; }
        public decimal? money_ini { get; set; }
        public decimal? money_end { get; set; }
        public decimal? money_total { get; set; }
        public int? ppu { get; set; }
        public int? face { get; set; }
        public int? hose { get; set; }
    }

    public class ClavesDominus
    {
        public string ClientId {get; set;}
        public string ClientSecrect {get; set;}
        public string GrantType {get; set;}
        public string Scope {get; set;}
    }
}

