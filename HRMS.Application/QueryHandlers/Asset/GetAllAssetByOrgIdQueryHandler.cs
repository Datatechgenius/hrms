using HRMS.Application.Queries.Asset;
using HRMS.Domain.Entities.Asset;
using HRMS.Infrastructure.Interfaces.Asset;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.QueryHandlers.Asset
{
    public class GetAllAssetByOrgIdQueryHandler : IRequestHandler<GetAllAssetByOrgIdQuery, List<AssetModel>>
    {
        private readonly IAssetRepository _assetRepository;
        public GetAllAssetByOrgIdQueryHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<List<AssetModel>> Handle(GetAllAssetByOrgIdQuery request, CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetAllAssetByOrgIdAsync(request.OrgId);
            return assets;
        }

    }
}
