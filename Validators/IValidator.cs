using System.Text.RegularExpressions;

namespace ProductAPI.Validators
{
    public interface IValidator
    {
        public List<ValidatorErrorResponse>? ValidateParams();

    }
}
