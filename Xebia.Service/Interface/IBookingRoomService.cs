using System;
using System.Collections.Generic;
using System.Text;
using Xebia.Model;

namespace Xebia.Service.Interface
{
    public interface IBookingRoomService
    {
        int AddOrUpdateAsset(MeetingRoom meetingRoom);
        MeetingRoom GetBookingRoom(int MeetingRoomId);
        void DeleteBookingRoom(int MeetingRoomId);
    }
}
