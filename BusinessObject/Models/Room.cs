namespace BusinessObject.Models;

public partial class Room
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = null!;
    public int? DepartmentId { get; set; }
    public int? Capacity { get; set; }
    
    public virtual Department? Department { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    // Methods
    public bool IsAvailable() => !Appointments.Any(a => a.AppointmentDate.Date == DateTime.Today);
}