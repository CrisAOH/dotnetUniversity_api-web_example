using FluentValidation;

namespace Application.Cursos.CursoCreate
{
    public class CursoCreateValidator : AbstractValidator<CursoCreateRequest>
    {
        public CursoCreateValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("El título no debe ser vacío.");
            RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La descripción no debe ser vacía.");
        }
    }
}