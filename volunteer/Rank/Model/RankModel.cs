using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Rank.Model
{
    public class RankModel
    {
        public int RankNum { get; set; }
        public string UserName { get; set; }
        public string Score { get; set; }
        public string Sign { get; set; }
        public byte[] Avatar { get; set; }
    }
}
