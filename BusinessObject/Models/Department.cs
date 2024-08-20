namespace HospitalManagementSystem.BusinessObject.Models;

public partial class Department
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
    public string? Location { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    //Method
    public bool HasDoctors() => Doctors.Any();
    public bool HasRooms() => Rooms.Any();
}