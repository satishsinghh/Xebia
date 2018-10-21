using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.Model
{
    public class Meeting
    {
        public int MeetingId { get; set; }
        public int AssetId { get; set; }
        public int MeetingRoomId { get; set; }
        public string Agenda { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BookedBy { get; set; }
    }
}
