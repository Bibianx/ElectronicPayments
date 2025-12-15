using System.Security.Cryptography;
using Domain.Entities.ZonaPagos;
using System.Text;

namespace Infraestructure.Mappers
{
    public class AutoMapperZonaPagos : Profile
{
    public AutoMapperZonaPagos()
    {
        CreateMap<IniciarPagoParams, INTENTOSZP>()
            .ForMember(dest => dest.str_descripcion_pago, opt => opt.MapFrom(src => src.InformacionPago.str_descripcion_pago))
            .ForMember(dest => dest.str_apellido_cliente, opt => opt.MapFrom(src => src.InformacionPago.str_apellido_cliente))
            .ForMember(dest => dest.str_telefono_cliente, opt => opt.MapFrom(src => src.InformacionPago.str_telefono_cliente))
            .ForMember(dest => dest.str_usuario, opt => opt.MapFrom(src => HashKeys(src.InformacionSeguridad.str_usuario)))
            .ForMember(dest => dest.str_nombre_cliente, opt => opt.MapFrom(src => src.InformacionPago.str_nombre_cliente))
            .ForMember(dest => dest.int_id_comercio, opt => opt.MapFrom(src => src.InformacionSeguridad.int_id_comercio))
            .ForMember(dest => dest.flt_total_con_iva, opt => opt.MapFrom(src => src.InformacionPago.flt_total_con_iva))
            .ForMember(dest => dest.str_clave, opt => opt.MapFrom(src => HashKeys(src.InformacionSeguridad.str_clave)))
            .ForMember(dest => dest.int_modalidad, opt => opt.MapFrom(src => src.InformacionSeguridad.int_modalidad))
            .ForMember(dest => dest.str_id_cliente, opt => opt.MapFrom(src => src.InformacionPago.str_id_cliente))
            .ForMember(dest => dest.flt_valor_iva, opt => opt.MapFrom(src => src.InformacionPago.flt_valor_iva))
            .ForMember(dest => dest.str_opcional1, opt => opt.MapFrom(src => src.InformacionPago.str_opcional1))
            .ForMember(dest => dest.str_opcional2, opt => opt.MapFrom(src => src.InformacionPago.str_opcional2))
            .ForMember(dest => dest.str_opcional3, opt => opt.MapFrom(src => src.InformacionPago.str_opcional3))
            .ForMember(dest => dest.str_opcional4, opt => opt.MapFrom(src => src.InformacionPago.str_opcional4))
            .ForMember(dest => dest.str_opcional5, opt => opt.MapFrom(src => src.InformacionPago.str_opcional5))
            .ForMember(dest => dest.str_id_pago, opt => opt.MapFrom(src => src.InformacionPago.str_id_pago))
            .ForMember(dest => dest.str_tipo_id, opt => opt.MapFrom(src => src.InformacionPago.str_tipo_id))
            .ForMember(dest => dest.str_email, opt => opt.MapFrom(src => src.InformacionPago.str_email))
            .ForMember(dest => dest.estado_intento, opt => opt.MapFrom(src => "P"));
    }

    private static string HashKeys(string password)
    {
        if (string.IsNullOrEmpty(password))
            return string.Empty;
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hash = SHA256.HashData(passwordBytes);

        string hexHash = Convert.ToHexString(hash);
        return hexHash.Substring(0, Math.Min(30, hexHash.Length));
    }
}

}

