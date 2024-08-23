namespace BusinessObject.Models;

public partial class RoomAssignment
{
    public int RoomAssignmentId { get; set; }

    public int RoomId { get; set; }

    public int PatientId { get; set; }

    public DateOnly AssignmentDate { get; set; }

    public DateOnly? DischargeDate { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}