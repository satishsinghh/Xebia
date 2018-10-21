
using System;
using System.Collections.Generic;
using System.Text;
using Xebia.DatabaseCore.Common;

namespace Xebia.DatabaseCore.Procedures
{
    public class DeleteAsset : ProcedureExecuteQuery
    {
        public const string PROCEDURE_NAME = "spDeleteAsset";

        public DeleteAsset(IXebiaDatabase database)
            : base(database)
        {
        }

        public int AssetId { get; set; }

        public override string GetName()
        {
            return PROCEDURE_NAME;
        }

        public static void Execute(IXebiaDatabase database, int AssetId)
        {
            var proc = new DeleteAsset(database)
            {
                AssetId = AssetId
            };
            proc.Execute();
        }
    }
}