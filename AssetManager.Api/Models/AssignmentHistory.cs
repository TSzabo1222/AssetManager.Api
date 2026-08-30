namespace AssetManager.Api.Models
{
    // Log: who received which asset, and when they returned it.
    // This lets you look back at an asset's full lifecycle.
    public class AssignmentHistory
    {
        public int Id { get; set; }

        public int AssetId { get; set; }
        public Asset Asset { get; set; } = null!;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }  // null = still with the employee
    }
}