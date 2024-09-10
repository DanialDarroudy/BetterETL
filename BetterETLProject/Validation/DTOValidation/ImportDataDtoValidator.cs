using BetterETLProject.DTO;
using BetterETLProject.Validation.SourcesValidation;
using FluentValidation;

namespace BetterETLProject.Validation.DTOValidation;

public class ImportDataDtoValidator : AbstractValidator<ImportDataDto>
{
    public ImportDataDtoValidator()
    {
        RuleFor(dto => dto.FilePath).NotNull().SetValidator(new FilePathValidator());
        RuleFor(dto => dto.Address).NotNull().SetValidator(new ConnectionSettingValidator());
    }
}