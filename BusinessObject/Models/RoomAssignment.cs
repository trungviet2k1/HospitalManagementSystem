namespace BusinessObject.Models;

public partial class RoomAssignment
{
    public int RoomAssignmentId { get; set; }

    public int RoomId { get; set; }

    public int PatientId { get; set; }

    public DateTime AssignmentDate { get; set; }

    public DateTime DischargeDate { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}