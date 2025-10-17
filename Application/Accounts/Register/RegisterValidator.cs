using FluentValidation;

namespace Application.Accounts.Register
{
    public class RegisterValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("El email no es correcto.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("El password está vacío.");
            RuleFor(x => x.NombreCompleto).NotEmpty().WithMessage("El nombre es nulo.");
            RuleFor(x => x.Carrera).NotEmpty().WithMessage("La carrera está vacío.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Ingrese un username.");
        }
    }
}