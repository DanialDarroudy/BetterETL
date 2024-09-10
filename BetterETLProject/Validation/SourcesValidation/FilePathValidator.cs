using BetterETLProject.Sources;
using FluentValidation;

namespace BetterETLProject.Validation.SourcesValidation;

public class FilePathValidator : AbstractValidator<FilePath>
{
    public FilePathValidator()
    {
        RuleFor(path => path.TableName).NotNull().NotEmpty();
        RuleFor(path => path.Type).NotNull().NotEmpty().MinimumLength(3)
            .Equal("CSV", StringComparer.OrdinalIgnoreCase);
    }
}