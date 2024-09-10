using BetterETLProject.DTO;
using BetterETLProject.Validation.SourcesValidation;
using FluentValidation;

namespace BetterETLProject.Validation.DTOValidation;

public class ConditionDtoValidator : AbstractValidator<ConditionDto>
{
    public ConditionDtoValidator()
    {
        RuleFor(dto => dto.Condition).NotNull().NotEmpty();
        RuleFor(dto => dto.TableName).NotNull().NotEmpty();
        RuleFor(dto => dto.Address).NotNull().SetValidator(new ConnectionSettingValidator());
        RuleFor(dto => dto.Limit).NotNull().NotEmpty()
            .Must(limit => limit.Equals("All" , StringComparison.OrdinalIgnoreCase) || int.TryParse(limit, out _));
    }
}