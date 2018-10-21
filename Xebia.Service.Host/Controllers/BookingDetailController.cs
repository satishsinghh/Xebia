using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xebia.Service.Interface;
using Xebia.Model;

namespace Xebia.Service.Host.Controllers
{
    [Produces("application/json")]
    public class BookingDetailController : BaseController
    {
        private readonly IBookingDetailService _bookingDetailService;

        public BookingDetailController(IBookingDetailService bookingDetailService)
        {
            _bookingDetailService = bookingDetailService;
        }

        /// <summary>
        /// Get GetEmployeeExpense
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public MeetingDetails GetEmployeeExpense(int BookedBy)
        {
            return _bookingDetailService.GetEmployeeExpense(BookedBy);
        }

        /// <summary>
        /// Get GetAvailableRoomList
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public MeetingDetails GetAvailableRoomList(Meeting meeting)
        {
            return _bookingDetailService.GetAvailableRoomList(meeting);
        }

        /// <summary>
        /// Get Meeting Room
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public MeetingDetails GetMeetingRoom(int MeetingId)
        {
           return _bookingDetailService.GetMeetingRoom(MeetingId);
        }
    }
}
