using System;
using System.Collections.Generic;
using System.Text;

namespace Xebia.Model
{
	public class ConnectionStrings
	{
		public const string ConfigSection = "ConnectionStrings";

		public string JobDistribution { get; set; }
		public string JobDistributionAudit { get; set; }
	}
}
