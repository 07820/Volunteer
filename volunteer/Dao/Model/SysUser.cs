using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Model
{
    public class SysUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserNo { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
    }
}
