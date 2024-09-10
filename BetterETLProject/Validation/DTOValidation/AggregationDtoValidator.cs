using BetterETLProject.DTO;
using BetterETLProject.Validation.SourcesValidation;
using FluentValidation;

namespace BetterETLProject.Validation.DTOValidation;

public class AggregationDtoValidator : AbstractValidator<AggregationDto>
{
    public AggregationDtoValidator()
    {
        RuleFor(dto => dto.TableName).NotNull().NotEmpty();
        RuleFor(dto => dto.GroupedByColumnNames).NotNull().NotEmpty();
        RuleForEach(dto => dto.GroupedByColumnNames).NotNull().NotEmpty();
        RuleFor(dto => dto.AggregatedColumnName).NotNull().NotEmpty();
        RuleFor(dto => dto.AggregateType).NotNull().NotEmpty();
        RuleFor(dto => dto.Address).NotNull().SetValidator(new ConnectionSettingValidator());
        RuleFor(dto => dto.Limit).NotNull().NotEmpty()
            .Must(limit => limit.Equals("All", StringComparison.OrdinalIgnoreCase) || int.TryParse(limit, out _));
    }
}