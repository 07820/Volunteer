using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Model
{
    // 护工管理表
    [Table("Caregiver")]
    public class Caregiver
    {
        [Key]
        public int CaregiverID { get; set; }

        public string CaregiverName { get; set; }

        public string ContactNumber { get; set; }
    }
}
