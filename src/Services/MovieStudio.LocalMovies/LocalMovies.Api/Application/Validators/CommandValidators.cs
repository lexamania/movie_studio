using FluentValidation;
using LocalMovies.Api.Application.Commands;

namespace LocalMovies.Api.Application.Validators;

public class AddDirectoryCommandValidator : AbstractValidator<AddDirectoryCommand>
{
    public AddDirectoryCommandValidator()
    {
        RuleFor(x => x.DirectoryPath)
            .NotEmpty()
            .WithMessage("Directory path is required")
            .Must(DirectoryExists)
            .WithMessage("Directory path does not exist");

        RuleFor(x => x.Caption)
            .NotEmpty()
            .WithMessage("Caption is required")
            .MaximumLength(255)
            .WithMessage("Caption cannot exceed 255 characters");
    }

    private static bool DirectoryExists(string path)
    {
        try
        {
            return Directory.Exists(path);
        }
        catch
        {
            return false;
        }
    }
}

public class RemoveDirectoryCommandValidator : AbstractValidator<RemoveDirectoryCommand>
{
    public RemoveDirectoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Directory ID is required");
    }
}
