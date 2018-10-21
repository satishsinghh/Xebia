
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xebia.DatabaseCore.Common;
using Xebia.Model;

namespace Xebia.DatabaseCore.Procedures
{
    public class spGetAsset
        : ProcedureExecuteReader<Asset>
    {
        public const string PROCEDURE_NAME = "spGetAsset";

        public spGetAsset(IXebiaDatabase database)
            : base(database)
        {
        }

        public int AssetId { get; set; }

        public override Asset HandleDataReader(IDataReader reader, IEnumerable<QueryParameter> parameters)
        {
            var job = reader.GetNextObject<Asset>();
            return job;
        }

        public override string GetName()
        {
            return PROCEDURE_NAME;
        }
    }
}
