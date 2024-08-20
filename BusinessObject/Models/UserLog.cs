namespace HospitalManagementSystem.BusinessObject.Models;

public partial class UserLog
{
    public int UserLogId { get; set; }
    public int UserId { get; set; }
    public string Action { get; set; } = null!;
    public DateTime? ActionDate { get; set; }
    public string? TableName { get; set; }
    public int? RecordId { get; set; }

    public virtual User User { get; set; } = null!;
}