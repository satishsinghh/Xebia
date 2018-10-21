using System;
using System.Collections.Generic;
using System.Text;
using Xebia.DatabaseCore.Common;

namespace Xebia.DatabaseCore.Procedures
{
    public class spSetBookingRoom
       : ProcedureExecuteQuery
    {
        public const string PROCEDURE_NAME = "spSetBookingRoom";

        public spSetBookingRoom(IXebiaDatabase database)
            : base(database)
        {
        }
        public int MeetingRoomId { get; set; }
        public string MeetingRoomName { get; set; }
        public int TotalSeat { get; set; }
        public int BookingFee { get; set; }

        [ParameterDirection(System.Data.ParameterDirection.Output)]
        public int NewMeetingRoomId { get; set; }

        public override string GetName()
        {
            return PROCEDURE_NAME;
        }
    }
}
