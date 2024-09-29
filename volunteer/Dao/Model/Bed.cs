using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Model
{
    // 床位管理表
    [Table("Bed")]
    public class Bed
    {
        [Key]
        public int BedID { get; set; }

        public string BedName { get; set; }

        public int IsVacant { get; set; }
    }
}
