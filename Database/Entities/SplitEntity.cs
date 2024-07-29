namespace ProductAPI.Database.Entities
{
    public class SplitEntity
    {
        public int Id { get; set; }

        public string Catcode { get; set; }

        public double Amount { get; set; }

        public string TransactionId { get; set; }

        public TransactionEntity? Transaction { get; set; }
    }
}
