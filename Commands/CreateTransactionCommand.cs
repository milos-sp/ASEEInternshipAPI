using ProductAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Commands
{
    public class CreateTransactionCommand
    {
        // public string Id { get; set; } = string.Empty;

        public string BeneficiaryName { get; set; } = string.Empty;

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public Directions Direction { get; set; } // d ili c

        [Required]
        public double Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        [Required]
        public string Currency { get; set; } = string.Empty;

        public int Mcc { get; set; }

        [Required]
        public TransactionKind Kind { get; set; }
    }
}
