using ProductAPI.Models;

namespace ProductAPI.Database.Entities
{
    public class TransactionEntity
    {
        public string Id { get; set; } = string.Empty;
        public string BeneficiaryName { get; set; } = string.Empty;

        public DateOnly Date { get; set; } // dateonly ili string?

        // direction neki ordered map
        public Directions Direction { get; set; } // d ili c

        public double Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty; // min i max length su 3 po standardu

        // mcc je neki enum
        public int Mcc { get; set; }

        // kind je isto enum
        public TransactionKind Kind { get; set; }

    }
}
