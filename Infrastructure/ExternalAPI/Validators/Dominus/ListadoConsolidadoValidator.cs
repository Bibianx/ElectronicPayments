using Infraestructure.ExternalAPI.DTOs.Dominus;

namespace Infraestructure.Validators.Dominus
{
    public class ListadoConsolidadoValidator : AbstractValidator<RequestListadoConsolidados>
    {
        public ListadoConsolidadoValidator()
        {
            RuleFor(x => x.branch_id)
                .NotNull()
                .WithMessage("La propiedad branch_id no puede ser nula");
            
            RuleFor(x => x.start_date)
                .NotNull()
                .WithMessage("La propiedad start_date no puede ser nula")
                .Must((request, start_date) =>
                {
                    return string.Compare(start_date, request.final_date) <= 0;
                })
                .WithMessage("La propiedad start_date no puede ser mayor a final_date");
            
            RuleFor(x => x.final_date)
                .NotNull()
                .WithMessage("La propiedad final_date no puede ser nula")
                .Must((request, final_date) =>
                {
                    return string.Compare(final_date, request.start_date) >= 0;
                })
                .WithMessage("La propiedad final_date no puede ser menor a start_date");
            
        }

    }

}