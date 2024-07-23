namespace ProductAPI.Models
{
    public class Transaction
    {
        public string Id { get; set; } = string.Empty;

        public string BeneficiaryName { get; set; } = string.Empty;

        public DateOnly Date { get; set; }

        public Directions Direction { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public int Mcc { get; set; }
        public TransactionKind Kind { get; set; }
    }
}
