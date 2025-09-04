using HRMS.Application.Commands.Asset;
using HRMS.Domain.Entities.Asset;
using HRMS.Infrastructure.Interfaces.Asset;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.CommandHandlers.Asset
{
    public class UpdateAssetCommandHandler : IRequestHandler<UpdateAssetCommand, bool>
    {
        private readonly IAssetRepository _assetRepository;
        public UpdateAssetCommandHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }
        public async Task<bool> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
        {
            var updateAsset = new AssetModel
            {
                Id = request.Id,
                OrganizationId = request.OrganizationId,
                AssetCode = request.AssetCode,
                AssetTypeId = request.AssetTypeId,
                AssetName = request.AssetName,
                Description = request.Description,
                Brand = request.Brand,
                Model = request.Model,
                SerialNumber = request.SerialNumber,
                PurchaseDate = request.PurchaseDate,
                WarrantyExpiry = request.WarrantyExpiry,
                Status = request.Status,
                Condition = request.Condition,
                AssignedTo = request.AssignedTo,
                AssignedDate = request.AssignedDate,
                ReturnedDate = request.ReturnedDate,
                LocationId = request.LocationId,
                Remarks = request.Remarks,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _assetRepository.UpdateAssetAsync(updateAsset);
            return result;
        }
    }
}
