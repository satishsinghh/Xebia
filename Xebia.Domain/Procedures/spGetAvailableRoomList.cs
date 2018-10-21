using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xebia.DatabaseCore.Common;
using Xebia.Model;

namespace Xebia.DatabaseCore.Procedures
{
    public class spGetAvailableRoomList
         : ProcedureExecuteReader<MeetingDetails>
    {
        public const string PROCEDURE_NAME = "spGetAvailableRoomList";

        public spGetAvailableRoomList(IXebiaDatabase database)
            : base(database)
        {
        }

        public int BookedBy { get; set; }
        public int AssetId { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }

        public override MeetingDetails HandleDataReader(IDataReader reader, IEnumerable<QueryParameter> parameters)
        {
            var GetEmployeeExpense = reader.GetNextObject<MeetingDetails>();
            return GetEmployeeExpense;
        }

        public override string GetName()
        {
            return PROCEDURE_NAME;
        }
    }
}
