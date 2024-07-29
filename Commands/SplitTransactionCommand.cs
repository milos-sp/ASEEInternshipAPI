using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Commands
{
    public class SplitTransactionCommand
    {

        [Required]
        [DataType(DataType.Text)]
        public string Catcode { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be number")]
        public double Amount { get; set; }

    }
}
