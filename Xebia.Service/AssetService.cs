using System;
using System.Collections.Generic;
using System.Text;
using Xebia.DatabaseCore;
using Xebia.Model;
using Xebia.DatabaseCore.Procedures;
using Xebia.Service.Interface;

namespace Xebia.Service
{
	public class AssetService: IAssetService
    {
		private readonly IXebiaDatabase database;

        public AssetService(IXebiaDatabase database)
		{
			this.database = database;
		}
        /// <summary>
        /// Added or update asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
		public int AddOrUpdateAsset(Asset asset)
		{
			var proc = new spSetAsset(database)
			{
				AssetName = asset.AssetName
			};
			return proc.Execute();
		}
       
        /// <summary>
        /// Get Asset
        /// </summary>
        /// <param name="AssetId"></param>
        /// <returns></returns>
        public Asset GetAsset(int AssetId)
        {
            var proc = new spGetAsset(this.database)
            {
                AssetId = AssetId
            };
            return proc.Execute();
        }
       
        /// <summary>
        /// Delete asset by AssetId
        /// </summary>
        /// <param name="AssetId"></param>
        public void DeleteAsset(int AssetId)
        {
            var proc = new DeleteAsset(this.database)
            {
                AssetId = AssetId
            };
             proc.Execute();
        }

    }
}
