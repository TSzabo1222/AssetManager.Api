namespace AssetManager.Api.Models
{
    // Ez a "szabad készlet" - kellékanyagok, amikből lehet fogyasztani
    // (pl. egerek, kábelek), nem egyedi sorozatszámos eszközök.
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
