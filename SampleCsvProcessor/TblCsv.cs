using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCsvProcessor
{
    [Table("tbl_csv")]
    public class TblCsv
    {
        [Key]
        public int GlobalRank { get; set; }
        public int TldRank { get; set; }
        public string Domain { get; set; }
        public string TLD { get; set; }
        public string RefSubNets { get; set; }
        public string RefIPs { get; set; }
        public string IDN_Domain { get; set; }
        public string IDN_TLD { get; set; }
        public int PrevGlobalRank { get; set; }
        public int PrevTldRank { get; set; }
        public string PrevRefSubNets { get; set; }
        public string PrevRefIPs { get; set; }
    }
}
