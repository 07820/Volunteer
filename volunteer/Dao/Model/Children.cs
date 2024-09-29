using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Model
{
    // 子女管理表
    [Table("Children")]
    public class Children
    {
        [Key]
        public int ChildID { get; set; }

        public string ChildName { get; set; }

        public string ContactNumber { get; set; }
    }
}
