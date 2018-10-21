using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xebia.DatabaseCore.Common;
using Xebia.Model;

namespace Xebia.DatabaseCore.Procedures
{
  
    public class spGetBookingRoom
           : ProcedureExecuteReader<MeetingRoom>
    {
        public const string PROCEDURE_NAME = "spGetBookingRoom";

        public spGetBookingRoom(IXebiaDatabase database)
            : base(database)
        {
        }

        public int MeetingRoomId { get; set; }

        public override MeetingRoom HandleDataReader(IDataReader reader, IEnumerable<QueryParameter> parameters)
        {
            var MeetingRoom = reader.GetNextObject<MeetingRoom>();
            return MeetingRoom;
        }

        public override string GetName()
        {
            return PROCEDURE_NAME;
        }
    }
}
