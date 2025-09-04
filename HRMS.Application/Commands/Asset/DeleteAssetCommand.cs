using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Asset
{
    public class DeleteAssetCommand : IRequest<bool>
    {
        public Guid AssetId { get; set; }
        public DeleteAssetCommand(Guid assetId)
        {
            AssetId = assetId;
        }
    }
}
