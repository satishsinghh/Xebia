using System;
using System.Collections.Generic;
using System.Text;
using Xebia.DatabaseCore.Common;

namespace Xebia.DatabaseCore.Procedures
{
 
    public class spDeleteBookingRoom : ProcedureExecuteQuery
    {
        public const string PROCEDURE_NAME = "spDeleteBookingRoom";

        public spDeleteBookingRoom(IXebiaDatabase database)
            : base(database)
        {
        }

        public int MeetingRoomId { get; set; }

        public override string GetName()
        {
            return PROCEDURE_NAME;
        }

        public static void Execute(IXebiaDatabase database, int MeetingRoomId)
        {
            var proc = new spDeleteBookingRoom(database)
            {
                MeetingRoomId = MeetingRoomId
            };
            proc.Execute();
        }
    }
}
