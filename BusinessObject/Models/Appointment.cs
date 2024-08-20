namespace BusinessObject.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? Reason { get; set; }

    public virtual User Doctor { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    //Method
    public bool IsAppointmentToday() => AppointmentDate.Date == DateTime.Today;
}