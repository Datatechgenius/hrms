using HRMS.Domain.Entities.Asset;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Queries.Asset
{
    public class GetAllAssetByOrgIdQuery : IRequest<List<AssetModel>>
    {
        public Guid OrgId { get; set; }
        public GetAllAssetByOrgIdQuery(Guid id)
        {
            OrgId = id;
        }
    }
}
