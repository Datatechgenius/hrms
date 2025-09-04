using HRMS.Domain.Entities.Asset;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Asset
{
    public class GetAssetByIdQuery : IRequest<AssetModel>
    {
        public Guid AssetId { get; set; }
        public GetAssetByIdQuery(Guid assetId)
        {
            AssetId = assetId;
        }
    }
}
