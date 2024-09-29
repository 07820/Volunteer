using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Model
{
    // 用药管理表
    [Table("Medication")]
    public class Medication
    {
        [Key]
        public int MedicationID { get; set; }

        public int ElderlyID { get; set; }

        public string MedicationName { get; set; }
    }
}
