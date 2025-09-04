using HRMS.Application.Commands.Asset;
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
    public class DeleteAssetCommandHandler : IRequestHandler<DeleteAssetCommand, bool>
    {
        private readonly IAssetRepository _assetRepository;
        public DeleteAssetCommandHandler(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }
        public async Task<bool> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
        {
            return await _assetRepository.DeleteAssetAsync(request.AssetId);
        }
    }
}
