namespace BusinessObject.Models;

public class Department
{
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public string? Location { get; set; }

    public int? DepartmentHeadId { get; set; }

    public virtual User? DepartmentHead { get; set; }

    public string DepartmentHeadName => DepartmentHead?.FullName ?? "N/A";

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}