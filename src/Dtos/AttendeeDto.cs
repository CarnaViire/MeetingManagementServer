namespace MeetingManagementServer.Models
{
    /// <summary>
    /// Partner attending a meeting DTO
    /// </summary>
    public class AttendeeDto
    {
        public long Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
    }
}
