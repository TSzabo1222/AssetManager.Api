namespace AssetManager.Api.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;      // e.g. "created", "deleted", "assigned", "returned"
        public string EntityType { get; set; } = string.Empty;  // e.g. "Asset", "Employee", "Inventory item"
        public string EntityName { get; set; } = string.Empty;  // e.g. "Dell Latitude 5420"
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}