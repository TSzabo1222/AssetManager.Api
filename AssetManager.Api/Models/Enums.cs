namespace AssetManager.Api.Models
{
    // Current status of an asset
    public enum AssetStatus
    {
        InStock,
        Assigned,
        Retired,
        Repair
    }

    // User roles (for future auth)
    public enum UserRole
    {
        Admin,
        Manager,
        Employee
    }
}