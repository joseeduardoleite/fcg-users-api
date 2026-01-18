using FiapCloudGames.Users.Application.Dtos;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace FiapCloudGames.Users.Application.Validators;

[ExcludeFromCodeCoverage]
public sealed class UsuarioValidator : AbstractValidator<UsuarioDto>
{
    public UsuarioValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("O e-mail é obrigatório.")
            .EmailAddress()
                .WithMessage("O e-mail informado não é válido.");

        RuleFor(x => x.Senha)
            .NotEmpty()
                .WithMessage("A senha é obrigatória.")
            .MinimumLength(8)
                .WithMessage("A senha deve ter no mínimo 8 caracteres.")
            .Must(ConterLetrasNumerosCaracteresEspeciais)
                .WithMessage("A senha deve conter pelo menos uma letra, um número e um caractere especial.");
    }

    private static bool ConterLetrasNumerosCaracteresEspeciais(string senha)
    {
        if (string.IsNullOrWhiteSpace(senha))
            return false;

        Regex regex = new(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':"",.<>\/?]).+$");
        return regex.IsMatch(senha);
    }
}