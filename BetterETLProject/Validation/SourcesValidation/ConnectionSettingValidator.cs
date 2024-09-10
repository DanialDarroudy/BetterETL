using BetterETLProject.Sources;
using FluentValidation;

namespace BetterETLProject.Validation.SourcesValidation;

public class ConnectionSettingValidator : AbstractValidator<ConnectionSetting>
{
    public ConnectionSettingValidator()
    {
        RuleFor(address => address.Host).NotNull().NotEmpty();
        RuleFor(address => address.UserName).NotNull().NotEmpty();
        RuleFor(address => address.PassWord).NotNull().NotEmpty();
        RuleFor(address => address.DataBase).NotNull().NotEmpty();
    }
}