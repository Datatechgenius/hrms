using HRMS.Domain.Entities.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Infrastructure.Interfaces.Asset
{
    public interface IAssetRepository 
    {
        Task InsertAssetAsync(AssetModel asset);
        Task<AssetModel> GetAssetByIdAsync(Guid assetId);
        Task<List<AssetModel>> GetAllAssetByOrgIdAsync(Guid orgId);
        Task UpdateAssetAsync(AssetModel asset);
        Task<bool> DeleteAssetAsync(Guid assetId);
    }
}
