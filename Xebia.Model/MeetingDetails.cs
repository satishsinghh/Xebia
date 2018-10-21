using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.Model
{
   public class MeetingDetails
    {
        public int BookedBy { get; set; }
        public int BookingAmount { get; set; }
        public string MeetingRoomName { get; set; }
        public int TotalSeat { get; set; }
        public string Agenda { get; set; }
        public string Description { get; set; }
       
    }
}
