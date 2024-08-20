namespace BusinessObject.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public int DepartmentId { get; set; }

    public bool IsAvailable { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<RoomAssignment> RoomAssignments { get; set; } = new List<RoomAssignment>();
}