using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xebia.Model;
using Xebia.Service.Interface;

namespace Xebia.Service.Host.Controllers
{
    [Produces("application/json")]
    public class AssetController : BaseController
    {

        private readonly IAssetService _assetService;
        
        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }
        /// <summary>
        /// Add or update Asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public int AddOrUpdateAsset([FromBody] Asset asset)
        {
           return _assetService.AddOrUpdateAsset(asset);
        }

        /// <summary>
        /// Get Asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public Asset GetAsset(int AssetId)
        {
            return _assetService.GetAsset(AssetId);
        }

        /// <summary>
        /// Delete Asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public void DeleteAsset(int AssetId)
        {
             _assetService.DeleteAsset(AssetId);
        }

    }
}