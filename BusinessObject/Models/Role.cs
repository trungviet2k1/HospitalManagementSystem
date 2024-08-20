namespace BusinessObject.Models;

public partial class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public string? Description { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    // Methods
    public string GetRoleDescription() => Description ?? "No description available.";
}