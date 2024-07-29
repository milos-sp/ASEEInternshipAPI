using ProductAPI.Models;

namespace ProductAPI.Database.Entities
{
    public class TransactionEntity
    {
        public string Id { get; set; } = string.Empty; // jeste string
        public string? BeneficiaryName { get; set; }

        public DateTime Date { get; set; } // ipak datetime zbog sortiranja

        public Directions Direction { get; set; } // d ili c

        public double Amount { get; set; }

        public string? Description { get; set; }

        public string Currency { get; set; } = string.Empty; // min i max length su 3 po standardu

        public int? Mcc { get; set; }

        // kind je isto enum
        public TransactionKind Kind { get; set; }

        // strani kljuc kategorije

        public string? Catcode { get; set; }

        public CategoryEntity Category { get; set; } // navigation property

        public List<SplitEntity> Splits { get; set; } = new List<SplitEntity>() { }; // jedna transakcija se deli na vise splitova

    }
}
