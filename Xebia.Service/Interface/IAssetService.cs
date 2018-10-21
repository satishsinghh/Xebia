using System;
using System.Collections.Generic;
using System.Text;
using Xebia.Model;

namespace Xebia.Service.Interface
{
    public interface IAssetService
    {
        int AddOrUpdateAsset(Asset asset);
        void DeleteAsset(int AssetId);
        Asset GetAsset(int AssetId);
    }
}
