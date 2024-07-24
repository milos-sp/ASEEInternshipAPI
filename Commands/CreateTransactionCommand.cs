using ProductAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Commands
{
    public class CreateTransactionCommand
    {
        public string Id { get; set; } = string.Empty;

        public string? BeneficiaryName { get; set; } = string.Empty;

        [Required]
        public string? Date { get; set; }

        [Required]
        public Directions? Direction { get; set; } // d ili c

        [Required]
        // [RegularExpression(@"\d+", ErrorMessage = "Amount not in right format")]
        public double? Amount { get; set; }

        public string? Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        public string? Currency { get; set; } = string.Empty;

        public int? Mcc { get; set; } = null;

        [Required]
        public TransactionKind? Kind { get; set; }
    }
}
