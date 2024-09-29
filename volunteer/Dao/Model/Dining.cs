using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dao.Model
{
    [Table("Dining")]
    public class Dining
    {
        [Key]
        public int DiningID { get; set; }

        public string DiningDate { get; set; }

        public string MealType { get; set; }

        public string MealName { get; set; }
    }
}
