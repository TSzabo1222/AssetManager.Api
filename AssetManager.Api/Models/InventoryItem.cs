namespace AssetManager.Api.Models
{
    // This is the "free stock" - consumable supplies (e.g. mice, cables),
    // not individually serialized assets.
    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}