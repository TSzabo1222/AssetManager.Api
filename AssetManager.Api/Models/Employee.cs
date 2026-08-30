namespace AssetManager.Api.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }

        // Navigation property: one employee can have many assets
        public ICollection<Asset> AssignedAssets { get; set; } = new List<Asset>();
    }
}