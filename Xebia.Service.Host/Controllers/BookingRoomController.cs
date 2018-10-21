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
    public class BookingRoomController : BaseController
    {
        private readonly IBookingRoomService _bookingRoomService;

        public BookingRoomController(IBookingRoomService bookingRoomService)
        {
            _bookingRoomService = bookingRoomService;
        }
        /// <summary>
        /// Add or update Asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public int AddOrUpdateAsset([FromBody] MeetingRoom meetingRoom)
        {
            return _bookingRoomService.AddOrUpdateAsset(meetingRoom);
        }

        /// <summary>
        /// Get Asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public MeetingRoom GetAsset(int MeetingRoomId)
        {
            return _bookingRoomService.GetBookingRoom(MeetingRoomId);
        }

        /// <summary>
        /// Delete Asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public void DeleteAsset(int MeetingRoomId)
        {
            _bookingRoomService.DeleteBookingRoom(MeetingRoomId);
        }
    }
}
