namespace Infraestructure.ExternalAPI.DTOs.Epayco
{
      public class BancoResponseDto : MessageResponse
    {
        public List<BancoDto> data { get; set; }
    }

    public class BancoDto
    {
        public string BankCode { get; set; }
        public string BankName { get; set; }
    }
}