using HRMS.Domain.Entities.Asset;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Commands.Asset
{
    public class UpdateAssetCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string AssetCode { get; set; } 
        public Guid? AssetTypeId { get; set; }
        public string AssetName { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiry { get; set; }
        public AssetStatusEnum Status { get; set; }
        public string Condition { get; set; }
        public Guid? AssignedTo { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public Guid? LocationId { get; set; }
        public string Remarks { get; set; }
    }
}
