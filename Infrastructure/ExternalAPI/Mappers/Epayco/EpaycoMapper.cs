using Domain.Entities.Epayco;
using Infraestructure.ExternalAPI.DTOs.Epayco;

namespace Infraestructure.ExternalAPI.Mappers.Epayco
{
    public class AutoMapperEpayco : Profile
    {
        public AutoMapperEpayco()
        {
            CreateMap<TransactionResponse, EPAYCO>()
                .ForMember(dest => dest.valor, opt => opt.MapFrom(src => src.data.valor))
                .ForMember(dest => dest.transactionID, opt => opt.MapFrom(src => src.data.transactionID))
                .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.data.descripcion))
                .ForMember(dest => dest.ref_payco, opt => opt.MapFrom(src => src.data.ref_payco))
                .ForMember(dest => dest.factura, opt => opt.MapFrom(src => src.data.factura))
                .ForMember(dest => dest.estado, opt => opt.MapFrom(src => "P"));
        }
    }
}
