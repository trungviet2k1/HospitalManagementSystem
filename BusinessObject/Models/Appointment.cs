namespace BusinessObject.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
    public int? RoomId { get; set; }
    public string? Status { get; set; }

    public virtual Patient Patient { get; set; } = null!;
    public virtual Doctor Doctor { get; set; } = null!;
    public virtual Room? Room { get; set; }

    //Method
    public bool IsAppointmentToday() => AppointmentDate.Date == DateTime.Today;
    public string GetStatusDescription()
    {
        return Status switch
        {
            "Pending" => "The appointment is scheduled and pending.",
            "Completed" => "The appointment has been completed.",
            "Canceled" => "The appointment has been canceled.",
            _ => "Unknown status."
        };
    }
}