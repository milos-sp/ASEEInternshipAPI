using ProductAPI.Models;
using ProductAPI.Services;
using System.Text.RegularExpressions;

namespace ProductAPI.Validators
{
    public class CategorizeTransactionValidator : IValidator
    {
        private string _id;

        private CategoryCode _catcode;

        private ITransactionService _transactionService;

        private ICategoryService _categoryService;

        public CategorizeTransactionValidator(string id, CategoryCode catcode, ITransactionService transactionService, ICategoryService categoryService)
        {
            _id = id;
            _catcode = catcode;
            _transactionService = transactionService;
            _categoryService = categoryService;
        }

        public List<ValidatorErrorResponse>? ValidateParams()
        {
            List<ValidatorErrorResponse> errors = new List<ValidatorErrorResponse>();

            string regex = @"^[1-9][0-9]{7}$";

            if(!Regex.IsMatch(_id, regex))
            {
                var resp = new ValidatorErrorResponse();
                resp.Tag = "id";
                resp.Error = PascalCaseToKebabCase(ErrorEnum.InvalidFormat.ToString());
                resp.Message = "Validation error: TransactionId is not in regular format";

                errors.Add(resp);
            }

            if (String.IsNullOrEmpty(_catcode.Catcode))
            {
                var resp = new ValidatorErrorResponse();
                resp.Tag = "catcode";
                resp.Error = PascalCaseToKebabCase(ErrorEnum.Required.ToString());
                resp.Message = "Validation error: Catcode is required parameter";

                errors.Add(resp);
            }

            return errors.Count() == 0 ? null : errors;
        }

        public async Task<BusinessProblemResponse?> CheckIfExists(string id, string catcode)
        {
            if(await _transactionService.GetTransaction(id) == null)
            {
                var err = new BusinessProblemResponse();
                err.Problem = "transaction-does-not-exist";
                err.Message = "Transaction does not exist";
                err.Details = "Transaction with provided id does not exist";

                return err;
            }

            if (await _categoryService.GetCategoryByCode(catcode) == null)
            {
                var err = new BusinessProblemResponse();
                err.Problem = "provided-category-does-not-exist";
                err.Message = "Provided category does not exist";
                err.Details = "Category with provided category code does not exist";

                return err;
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
