using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.Model
{
	public class MeetingRoom
	{
		public int? MeetingRoomId { get; set; }
		public string MeetingRoomName { get; set; }
		public int TotalSeat { get; set; }
		public int BookingFee { get; set; }
		public List<Asset> Assets { get; set; }
	}
}
