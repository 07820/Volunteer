using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Form.Model
{
    [AddINotifyPropertyChangedInterface]
    public class OrmForumItem
    {
        public string Rank { get; set; }
        public string PostId { get; set; }
        public string NickName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string VideoPath { get; set; }
        public byte[] Picture { get; set; }
        // private string _likes;
        public string _likes;
        public string Likes { get { return $"{_likes} Likes"; } set { _likes = value; } }
        private string _hot;
        public string Hot { get { return $"{_hot} Heat"; } set { _hot = value; } }
        private string _commentCount;

        public string CommentCount { get { return $"{_commentCount} Comments"; } set { _commentCount = value; } }
        public string CommentCountReal { get { return _commentCount; } set { _commentCount = value; } }
        public byte[] Avatar { get; set; }
        public int IsLike { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
