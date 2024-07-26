namespace ProductAPI.Commands
{
    public class CreateCategoryCommand
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? ParentCode { get; set; } = null;
    }
}
