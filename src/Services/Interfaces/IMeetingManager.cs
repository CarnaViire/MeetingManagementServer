using MeetingManagementServer.Dtos;
using MeetingManagementServer.Models;
namespace MeetingManagementServer.Services.Interfaces
{
    public interface IMeetingManager
    {
        Meeting[] BuildMeetings();

        Meeting BuildMeeting(Country country);
    }
}
