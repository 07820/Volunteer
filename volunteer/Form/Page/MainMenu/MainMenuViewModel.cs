using Dao.Model;
using Dapper;
using Form.Control;
using Form.Model;
using Stylet;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Controls;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Windows;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Data;
using Form.Page.Post;
using Form.Config;
using Form.Page.NewPost;
using WindowsFormsApp1;



namespace Form.Page.MainMenu
{
    public class MainMenuViewModel : Screen
    {
        public MainMenuView SView { get; set; }
        public BindableCollection<OrmForumItem> Posts { get; set; }
        public int PageType { get; set; } = 1;
        public string KeyWord { get; set; }


        public void Load()
        {
            SView = this.View as MainMenuView;
            Posts = new BindableCollection<OrmForumItem>();
            LoadData();
        }
        public void ShowPost(object sender, MouseButtonEventArgs e)
        {
            var ctl = sender as FrameworkElement;
            var bound = ctl.DataContext as OrmForumItem;
            PostViewModel postViewModel = new PostViewModel(bound);
            Boot.Manager.ShowDialog(postViewModel);
            LoadData();
        }
        public void Like(object sender, MouseButtonEventArgs e)
        {
            var ctl = sender as FrameworkElement;
            var bound = ctl.DataContext as OrmForumItem;
            using (var db = DBHelper.GetConn())
            {
                
                IDbTransaction tran = db.BeginTransaction();
                try
                {
                    var sql = $@"SELECT COUNT(UserID) FROM UserLikes WHERE UserID = {App.UserId} AND PostID = {bound.PostId} ";
                    var cnt = db.QuerySingle<int>(sql,transaction: tran);
                    //var cnt = db.QueryFirst<int>(sql, tran);
                    //var ss = db.RecordCount<int>(sql,tran);
                    if (cnt == 0)
                    {
                        db.Execute($"INSERT INTO UserLikes (UserID, PostID) VALUES ({App.UserId}, {bound.PostId})", transaction: tran);
                        db.Execute($"UPDATE T_Post SET Likes = ISNULL(Likes, 0) + 1 WHERE postID = {bound.PostId}", transaction: tran);
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
                catch(Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void Close()
        {
            this.RequestClose();
        }
        public void Search(string key)
        {
            KeyWord = key;
            PageType = 3;
            LoadData();
        }
        public void SendPost(object sender, RoutedEventArgs e)
        {
            NewPostViewModel newPostViewModel = new NewPostViewModel();
            Boot.Manager.ShowDialog(newPostViewModel);
            LoadData();
        }
        public void LoadData()
        {
            if (PageType == 1)
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
	                    LEFT JOIN UserLikes ul ON ul.PostID = p.postID AND ul.UserID = {App.UserId} )
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
                      LEFT JOIN UserLikes ul ON ul.PostID = p.postID AND ul.UserID = {App.UserId})
                    ORDER BY
	                    postID DESC ";
                    var res = db.Query<OrmForumItem>(sql).ToList();
                    Posts.Clear();
                    res.ForEach(x => Posts.Add(x));
                }
            }
            else if (PageType == 2)
            {
                using (var db = DBHelper.GetConn())
                {
                    var sql = $@"
                        SELECT
                        top 6
	                        p.postID,
	                        p.nickname,
	                        p.title,
	                        p.content,
	                        COALESCE ( p.Likes, 0 ) AS Hot,
	                        COALESCE ( p.Likes, 0 ) AS Likes,
	                        u.avatar,
	                        COALESCE ( c.comment_count, 0 ) AS commentcount ,
                            CreateTime,
                            VideoPath
                        FROM
	                        T_Post p
	                        LEFT JOIN student_information u ON p.UserId = u.stuID
	                        LEFT JOIN ( SELECT postID, COUNT ( * ) AS comment_count FROM Comments GROUP BY postID ) c ON p.postID = c.postID 
                        ORDER BY
	                        Likes DESC; ";
                    var res = db.Query<OrmForumItem>(sql).ToList();
                    Posts.Clear();
                    int i = 1;
                    res.ForEach(x => { x.Rank = (i++) + ""; Posts.Add(x); });
                }
            }
            else if (PageType == 3)
            {
                using (var db = DBHelper.GetConn())
                {
                    var sql = $@"
                        SELECT
	                        p.postID,
	                        p.nickname,
	                        p.title,
	                        p.content,
	                        COALESCE ( p.Likes, 0 ) AS Likes,
	                        u.avatar,
	                        COALESCE ( c.comment_count, 0 ) AS commentcount,
                            CreateTime,
                            VideoPath
                        FROM
	                        T_Post p
	                        LEFT JOIN student_information u ON p.UserId = u.stuID
	                        LEFT JOIN ( SELECT postID, COUNT ( * ) AS comment_count FROM Comments GROUP BY postID ) c ON p.postID = c.postID 
                        WHERE
	                        title like '%{KeyWord}%'
	                        or content Like '%{KeyWord}%'
                        ORDER BY
	                        postID DESC ";
                    var res = db.Query<OrmForumItem>(sql).ToList();
                    Posts.Clear();
                    res.ForEach(x => Posts.Add(x));
                }
            }
        }
        public void SelectPage(object sender, MouseButtonEventArgs e)
        {
            var tbx = sender as TextBlock;
            PageType = Convert.ToInt32(tbx.Tag);
            LoadData();
        }
    }
}