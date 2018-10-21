using System.Collections.Generic;
using System.Text;
using Xebia.DatabaseCore;
using Xebia.DatabaseCore.Procedures;
using Xebia.Model;
using Xebia.Service.Interface;

namespace Xebia.Service
{
    public class BookingDetailService : IBookingDetailService
    {
        private readonly IXebiaDatabase database;
        public BookingDetailService(IXebiaDatabase database)
        {
            this.database = database;
        }

        /// <summary>
        /// Get BookingRoom
        /// </summary>
        /// <param name="AssetId"></param>
        /// <returns></returns>
        public MeetingDetails GetEmployeeExpense(int BookedBy)
        {
            var proc = new spGetEmployeeExpense(this.database)
            {
                BookedBy = BookedBy
            };
            return proc.Execute();
        }
        /// <summary>
        /// Get Get Available Room List
        /// </summary>
        /// <param name="AssetId"></param>
        /// <returns></returns>
        public MeetingDetails GetAvailableRoomList(Meeting meeting)
        {
            var proc = new spGetAvailableRoomList(this.database)
            {
                BookedBy = meeting.BookedBy,
                AssetId=meeting.AssetId,
                fromDate=meeting.StartDate,
                toDate=meeting.EndDate
            };
            return proc.Execute();
        }
        /// <summary>
        /// get Get Meeting Room
        /// </summary>
        /// <param name="meeting"></param>
        /// <returns></returns>
        public MeetingDetails GetMeetingRoom(int BookedBy)
        {
            var proc = new spGetMeetingRoom(this.database)
            {
                BookedBy = BookedBy
            };
            return proc.Execute();
        }

    }
}
