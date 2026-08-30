namespace AssetManager.Api.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;       // e.g. "Dell Latitude 5420"
        public string Type { get; set; } = string.Empty;       // e.g. "Laptop", "Monitor", "Phone"
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public AssetStatus Status { get; set; } = AssetStatus.InStock;

        // Nullable: may not be assigned to anyone yet
        public int? AssignedToEmployeeId { get; set; }
        public Employee? AssignedToEmployee { get; set; }
    }
}