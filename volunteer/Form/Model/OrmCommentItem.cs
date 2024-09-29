using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Form.Model
{
    public class OrmCommentItem
    {
        public string CommentID { get; set; }
        public string PostID { get; set; }
        public string Content { get; set; }
        public string CreatedTime { get; set; }
        public string AuthorName { get; set; }
        public byte[] Avatar { get; set; }
    }
}
