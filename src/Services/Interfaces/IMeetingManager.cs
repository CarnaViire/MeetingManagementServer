using MeetingManagementServer.Dtos;
using MeetingManagementServer.Models;
namespace MeetingManagementServer.Services.Interfaces
{
    /// <summary>
    /// Meeting manager to plan meetings with partners in countries
    /// </summary>
    public interface IMeetingManager
    {
        /// <summary>
        /// Get meetings of 2 day length with maximal attendance for all countries
        /// </summary>
        Meeting[] BuildAllMeetings();

        /// <summary>
        /// Get the meeting of 2 day length with maximal attendance for a specific country
        /// </summary>
        Meeting BuildMeeting(Country country);
    }
}
