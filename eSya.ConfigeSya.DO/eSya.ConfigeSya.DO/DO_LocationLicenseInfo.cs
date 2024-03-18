using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigeSya.DO
{
    public class DO_LocationLicenseInfo
    {
        public int BusinessKey { get; set; }
        public byte[] EBusinessKey { get; set; } = null!;
        public string ESyaLicenseType { get; set; } = null!;
        public int EUserLicenses { get; set; }
        public byte[] EActiveUsers { get; set; } = null!;
        public int ENoOfBeds { get; set; }
        public bool Lstatus { get; set; }
        public string? LocationDescription { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string FormID { get; set; }
        public string TerminalId { get; set; }



    }
}
