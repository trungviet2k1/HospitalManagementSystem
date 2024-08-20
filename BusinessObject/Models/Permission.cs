﻿namespace HospitalManagementSystem.BusinessObject.Models;

public partial class Permission
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = null!;
    public string? Description { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    // Methods
    public string GetPermissionDescription() => Description ?? "No description available.";
}