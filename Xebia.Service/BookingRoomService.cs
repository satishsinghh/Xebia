using System;
using System.Collections.Generic;
using System.Text;
using Xebia.DatabaseCore;
using Xebia.DatabaseCore.Procedures;
using Xebia.Model;
using Xebia.Service.Interface;

namespace Xebia.Service
{
   public class BookingRoomService: IBookingRoomService
    {
        private readonly IXebiaDatabase database;
        public BookingRoomService(IXebiaDatabase database)
        {
            this.database = database;
        }
        /// <summary>
        /// Added or update BookingRoom
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
		public int AddOrUpdateAsset(MeetingRoom meetingRoom)
        {
            var proc = new spSetBookingRoom(database)
            {
                MeetingRoomName = meetingRoom.MeetingRoomName,
                TotalSeat = meetingRoom.TotalSeat,
                BookingFee = meetingRoom.BookingFee
            };
             proc.Execute();
            return proc.NewMeetingRoomId;
        }

        /// <summary>
        /// Get BookingRoom
        /// </summary>
        /// <param name="AssetId"></param>
        /// <returns></returns>
        public MeetingRoom GetBookingRoom(int MeetingRoomId)
        {
            var proc = new spGetBookingRoom(this.database)
            {
                MeetingRoomId = MeetingRoomId
            };
            return proc.Execute();
        }

        /// <summary>
        /// Delete asset by BookingRoom
        /// </summary>
        /// <param name="AssetId"></param>
        public void DeleteBookingRoom(int MeetingRoomId)
        {
            var proc = new spDeleteBookingRoom(this.database)
            {
                MeetingRoomId = MeetingRoomId
            };
            proc.Execute();
        }
    }
}
