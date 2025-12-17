using Infraestructure.ExternalAPI.DTOs.Dominus;

namespace Validators.Dominus
{
public class TokenValidator : AbstractValidator<TokenParams>
{
    public TokenValidator(IConfiguration configuration)
    {
        string client_secret = configuration["Dominus:ClientSecrect"];
        string grant_type = configuration["Dominus:GrantType"];
        string client_id = configuration["Dominus:ClientId"];
        string scope = configuration["Dominus:Scope"];

            RuleFor(x => x.client_id)
                .NotEmpty()
                .WithMessage("client_id es obligatorio")
                .Must(x => x == client_id)
                .WithMessage("client_id inválido");

            RuleFor(x => x.client_secret)
                .NotEmpty()
                .WithMessage("client_secret es obligatorio")
                .Must(x => x == client_secret)
                .WithMessage("client_secret inválido");

            RuleFor(x => x.grant_type)
                .NotEmpty()
                .WithMessage("grant_type es obligatorio")
                .Must(x => x == grant_type)
                .WithMessage("grant_type inválido");

            RuleFor(x => x.scope)
                .NotEmpty()
                .WithMessage("scope es obligatorio")
                .Must(x => x == scope)
                .WithMessage("scope inválido");
        }
    }

}
