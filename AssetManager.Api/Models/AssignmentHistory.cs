namespace AssetManager.Api.Models
{
    // Napló: ki, mikor kapott meg egy eszközt, és mikor adta vissza.
    // Ez teszi lehetővé, hogy egy eszköz teljes "életútját" vissza tudd nézni.
    public class AssignmentHistory
    {
        public int Id { get; set; }

        public int AssetId { get; set; }
        public Asset Asset { get; set; } = null!;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }  // null = még nála van
    }
}
