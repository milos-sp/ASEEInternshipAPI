namespace ProductAPI.Commands
{
    public class CreateCategoryCommand
    {
        public string? Code { get; set; } = String.Empty;

        public string? Name { get; set; } = String.Empty;

        public string? ParentCode { get; set; } = null;
    }
}
