using ProductAPI.Models;
using System.Text.RegularExpressions;

namespace ProductAPI.Validators
{
    public class GetTransactionsValidator
    {
        private string? _transactionKind;

        private DateTime? _startDate;

        private DateTime? _endDate;

        public GetTransactionsValidator(string? transactionKind, DateTime? startDate, DateTime? endDate)
        {
            _transactionKind = transactionKind;
            _startDate = startDate;
            _endDate = endDate;
        }

        public ValidatorErrorResponse? ValidateParams()
        {
            if (_transactionKind != null)
            {
                if (!Enum.IsDefined(typeof(TransactionKind), _transactionKind))
                {
                    var resp = new ValidatorErrorResponse();
                    resp.Tag = "transaction-kind";
                    resp.Error = PascalCaseToKebabCase(ErrorEnum.UnknownEnum.ToString());
                    resp.Message = $"Validation error: {_transactionKind} is not regular transaction kind";

                    return resp;
                }  
            }

            return null;
        }

        private string PascalCaseToKebabCase(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return Regex.Replace(
                value,
                "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])",
                "-$1",
                RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }
    }
}
