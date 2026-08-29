namespace AssetManager.Api.Models
{
    // Egy eszköz aktuális állapota
    public enum AssetStatus
    {
        InStock,    // raktáron, kiadható
        Assigned,   // ki van adva valakinek
        Retired,    // selejtezve
        Repair      // javítás alatt
    }

    // Felhasználói szerepkörök (későbbi auth-hoz)
    public enum UserRole
    {
        Admin,
        Manager,
        Employee
    }
}
