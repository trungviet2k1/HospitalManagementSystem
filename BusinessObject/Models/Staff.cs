namespace BusinessObject.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int UserId { get; set; }

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}