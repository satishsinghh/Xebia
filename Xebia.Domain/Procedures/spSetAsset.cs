using System;
using System.Collections.Generic;
using System.Text;
using Xebia.DatabaseCore.Common;

namespace Xebia.DatabaseCore.Procedures
{
	public class spSetAsset
	   : ProcedureExecuteQuery
	{
		public const string PROCEDURE_NAME = "spSetAsset";

		public spSetAsset(IXebiaDatabase database)
			: base(database)
		{
		}
		public int AssetId { get; set; }
		public string AssetName { get; set; }

		[ParameterDirection(System.Data.ParameterDirection.Output)]
		public int NewAssetId { get; set; }

		public override string GetName()
		{
			return PROCEDURE_NAME;
		}
	}
}
