using Dapper;
using Form.Config;
using Form.Model;
using Form.Page.MainMenu;
using HandyControl.Controls;
using Stylet;
using Stylet.Xaml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WindowsFormsApp1;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Form.Page.Post
{
    public class PostViewModel : Screen
    {
        public OrmForumItem Post {  get; set; }
        public PostView SView { get; set; }
        public string Msg { get; set; }
        public BindableCollection<OrmCommentItem> CommentItems { get; set; }
        public PostViewModel(OrmForumItem bound)
        {
            this.Post = bound;
        }
        public void Close()
        {
            this.RequestClose();
        }
        public void Like(object sender, MouseButtonEventArgs e)
        {
            using (var db = DBHelper.GetConn())
            {

                IDbTransaction tran = db.BeginTransaction();
                try
                {
                    var sql = $@"SELECT COUNT(UserID) FROM UserLikes WHERE UserID = {App.UserId} AND PostID = {Post.PostId} ";
                    var cnt = db.QuerySingle<int>(sql, transaction: tran);
                    //var cnt = db.QueryFirst<int>(sql, tran);
                    //var ss = db.RecordCount<int>(sql,tran);
                    if (cnt == 0)
                    {
                        db.Execute($"INSERT INTO UserLikes (UserID, PostID) VALUES ({App.UserId}, {Post.PostId})", transaction: tran);
                        db.Execute($"UPDATE T_Post SET Likes = ISNULL(Likes, 0) + 1 WHERE postID = {Post.PostId}", transaction: tran);
                    }
                    else
                    {
                        MessageBox1 form = new MessageBox1("You have already liked this post!");
                        form.ShowDialog();
                        //MessageBox.Show("You have already liked this post!");
                    }
                    tran.Commit();
                    LoadData();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void Load()
        {
            SView = this.View as PostView;
            CommentItems = new BindableCollection<OrmCommentItem>();
            LoadCommentData();
            LoadData();
            LoadPic();
            LoadVdo();

        }
        public async void LoadData()
        {
            using (var db = DBHelper.GetConn())
            {
                var sql = $@"
                    (SELECT
	                    p.postID,
	                    p.nickname,
	                    p.title,
	                    CASE WHEN ul.UserID IS NULL THEN 0 ELSE 1 END AS IsLike,
	                    p.content,
	                    COALESCE ( p.Likes, 0 ) AS Likes,
	                    COALESCE ( c.comment_count, 0 ) AS commentcount,
	                    s.avatar,
	                    CreateTime,
	                    VideoPath 
                    FROM
	                    T_Post p
	                    JOIN student_information s ON p.UserId = s.stuID
	                    LEFT JOIN ( SELECT postID, COUNT ( * ) AS comment_count FROM Comments GROUP BY postID ) c ON p.postID = c.postID
	                    LEFT JOIN UserLikes ul ON ul.PostID = p.postID AND ul.UserID = {App.UserId} where p.postID = {Post.PostId} )
                    UNION
                    (SELECT
	                    p.postID,
	                    p.nickname,
	                    p.title,
	                    CASE WHEN ul.UserID IS NULL THEN 0 ELSE 1 END AS IsLike,
	                    p.content,
	                    COALESCE ( p.Likes, 0 ) AS Likes,
	                    COALESCE ( c.comment_count, 0 ) AS commentcount,
	                    o.avatar,
	                    CreateTime,
	                    VideoPath 
                    FROM
	                    T_Post p
	                    JOIN organizer_audited o ON p.UserId = o.orgID
	                    LEFT JOIN ( SELECT postID, COUNT ( * ) AS comment_count FROM Comments GROUP BY postID ) c ON p.postID = c.postID
                      LEFT JOIN UserLikes ul ON ul.PostID = p.postID AND ul.UserID = {App.UserId} where p.postID = {Post.PostId})
                    ORDER BY
	                    postID DESC ";
                var res = db.Query<OrmForumItem>(sql).ToList();
                if(res.Count > 0)
                {
                    Post.IsLike = res[0].IsLike;
                    // Post.Likes = res[0].Likes;
                    Post.Likes = res[0]._likes;
                }
            }
        }
        public async void LoadVdo()
        {
            SView.MV.Source = new Uri(new System.Uri($@"{Boot.vdoHttp}/{Post.VideoPath}").AbsoluteUri, UriKind.Absolute);
            SView.MV.LoadedBehavior = MediaState.Manual;
            SView.MV.UnloadedBehavior = MediaState.Manual;
            SView.MV.Play();
        }
        public async void LoadPic()
        {
            using (var db = DBHelper.GetConn())
            {
                var sql = $"select picture from T_Post where postID = '{Post.PostId}'";
                var pic = await db.QueryAsync<byte[]>(sql);
                Post.Picture = pic.ToList()[0];
            }
        }
        public void Comment()
        {
            using (var db = DBHelper.GetConn())
            {
                var sql = $"INSERT INTO dbo.Comments (PostID, Content, CreatedTime, AuthorName, UserID) VALUES ({Post.PostId}, '{Msg}', '{DateTime.Now}', '', '{App.UserId}')";
                db.Execute(sql);
                MessageBox1 form = new MessageBox1("Comment successfully!");
                form.ShowDialog();
                //MessageBox.Show("Comment successfully!");
                LoadCommentData();
                Post.CommentCount = (Convert.ToInt32(Post.CommentCountReal) + 1) + "";
            }
            
        }
        public void LoadCommentData()
        {
            using (var db = DBHelper.GetConn())
            {
                var sql = $@"
    (
        SELECT
            c.CommentID,
            c.PostID,
            c.Content,
            c.CreatedTime,
           s.nickName AS AuthorName,
            s.Avatar
        FROM
            Comments c
            JOIN student_information s ON c.UserId = s.stuID
        WHERE
            c.PostID = {Post.PostId}
    )
    UNION 
    (
        SELECT
            c.CommentID,
            c.PostID,
            c.Content,
            c.CreatedTime,
          o.userName AS AuthorName,
            o.Avatar
        FROM
            Comments c
            JOIN organizer_audited o ON c.UserId = o.orgID
        WHERE
            c.PostID = {Post.PostId}
    )
    ORDER BY
        CreatedTime DESC";
                var res = db.Query<OrmCommentItem>(sql).ToList();
                CommentItems.Clear();
                res.ForEach(x => CommentItems.Add(x));
                Msg = "";
            }
        }
    }
}
