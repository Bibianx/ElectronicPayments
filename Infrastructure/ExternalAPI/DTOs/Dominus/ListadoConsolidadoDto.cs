namespace Infraestructure.ExternalAPI.DTOs.Dominus
{
    public class RequestListadoConsolidados
    {
        public int branch_id { get; set; }
        public string start_date { get; set; } 
        public string final_date { get; set; } 
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
}