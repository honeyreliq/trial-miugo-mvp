using FluentValidation;
using FluentValidation.Validators;

namespace IUGOCare.Application.Common.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"));
        }

        public static IRuleBuilderOptions<T, string> ZipCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator(@"^\d{5}(-\d{4})?(?!-)$"));
        }
    }
}
