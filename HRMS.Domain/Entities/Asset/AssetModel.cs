using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Entities.Asset
{
    public class AssetModel
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public Guid? AssetTypeId { get; set; }
        public string AssetName { get; set; } = string.Empty;
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
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public enum AssetStatusEnum
    {
       available,
       notavailable,
       assigned,
       maintenance
    }

}
