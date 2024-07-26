namespace ProductAPI.Database.Entities
{
    public class CategoryEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string? ParentCode { get; set; } = null;

        public List<TransactionEntity> Transactions { get; set; } = new List<TransactionEntity> { }; // 1 - N: jedna kategorija moze imati vise transakcija
    }
}
