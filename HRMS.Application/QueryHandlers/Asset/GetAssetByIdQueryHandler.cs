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
    public class GetAssetByIdQueryHandler : IRequestHandler<GetAssetByIdQuery, AssetModel>
    {
        private readonly IAssetRepository _assetRepository;
        public GetAssetByIdQueryHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }
        public async Task<AssetModel> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetAssetByIdAsync(request.AssetId);
            return asset;
        }
    }
}
