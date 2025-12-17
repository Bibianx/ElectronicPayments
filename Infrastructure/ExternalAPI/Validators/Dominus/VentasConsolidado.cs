using Infraestructure.ExternalAPI.DTOs.Dominus;

namespace Validators.Dominus
{
public class VentasConsolidadoValidator : AbstractValidator<ConsultaVentasConsolidadoParams>
{
    public VentasConsolidadoValidator()
    {
            RuleFor(x => x.consolidated_id)
                .NotEmpty()
                .WithMessage("La propiedad consolidated_id es obligatorio");
        }
    }

}
