using FluentValidation;

namespace Application.Cursos.CursoCreate
{
    public class CursoCreateValidator : AbstractValidator<CursoCreateRequest>
    {
        public CursoCreateValidator()
        {
            RuleFor(x => x.Titulo);
            RuleFor(x => x.Descripcion);
        }
    }
}