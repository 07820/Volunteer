using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Model
{
    // 缴费管理表
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        public int ElderlyID { get; set; }

        public string PaymentName { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
