using System;
using System.Collections.Generic;

namespace eSya.ConfigeSya.DL.Entities
{
    public partial class GtEcbsli
    {
        public int BusinessKey { get; set; }
        public byte[] EBusinessKey { get; set; } = null!;
        public string ESyaLicenseType { get; set; } = null!;
        public byte[] EUserLicenses { get; set; } = null!;
        public byte[] EActiveUsers { get; set; } = null!;
        public byte[]? ENoOfBeds { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
