using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Model
{
    // 老人管理表
    [Table("Elderly")]
    public class Elderly
    {
        [Key]
        public int ElderlyID { get; set; }

        public string ElderlyName { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime AdmissionDate { get; set; }

        public int BedID { get; set; }

        public int ChildID { get; set; }

        public int CaregiverID { get; set; }

        public DateTime ExpiryDate { get; set; }

        public string IDNumber { get; set; }

        public string Remark { get; set; }
        public int Discharge { get; set; }
        public string Img { get; set; }
    }

}
