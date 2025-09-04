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
    public class CreateAssetCommandHandler : IRequestHandler<CreateAssetCommand, Guid>
    { 
        private readonly IAssetRepository _assetRepository;
        public CreateAssetCommandHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }
        public async Task<Guid> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = new AssetModel
            {
                Id = Guid.NewGuid(),
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
                CreatedAt = DateTime.UtcNow
            };
            await _assetRepository.InsertAssetAsync(asset);
            return asset.Id;
        }
    }
}
