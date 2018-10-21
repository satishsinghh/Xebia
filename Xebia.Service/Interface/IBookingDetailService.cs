
using System;
using System.Collections.Generic;
using System.Text;
using Xebia.Model;

namespace Xebia.Service.Interface
{
    public interface IBookingDetailServiceType<T>
    {
        MeetingDetails GetEmployeeExpense(int BookedBy);
        MeetingDetails GetAvailableRoomList(Meeting meeting);
        MeetingDetails GetMeetingRoom(int BookedBy);
    }
    public interface IBookingDetailService
    {
        MeetingDetails GetEmployeeExpense(int BookedBy);
        MeetingDetails GetAvailableRoomList(Meeting meeting);
        MeetingDetails GetMeetingRoom(int BookedBy);
    }
}