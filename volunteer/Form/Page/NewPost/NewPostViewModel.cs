using Dapper;
using Form.Model;
using Form.Page.Post;
using Microsoft.Win32;
using Stylet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Form.Util;
using HandyControl.Controls;
using Form.Page.Progress;
//using MessageBox = HandyControl.Controls.MessageBox;
using MessageBox = System.Windows.Forms.MessageBox;
using Form.Config;
using Model;
using WindowsFormsApp1;

namespace Form.Page.NewPost
{
    public class NewPostViewModel : Screen
    {
        public NewPostView SView { get; set; }
        public void Load()
        {
            this.SView = this.View as NewPostView;
        }
        public void Close()
        {
            this.RequestClose();
        }
        public void PublicPost(object sender, RoutedEventArgs e)
        {

            using (var db = DBHelper.GetConn())
            {
                IDbTransaction tran = db.BeginTransaction();
                try
                {


                    byte[] videoBytes = null;
                    if (SView.PicPost.HasValue)
                    {
                        var picPath = SView.PicPost.Uri.LocalPath;
                        videoBytes = File.ReadAllBytes(picPath);
                    }
                    string sql1 = "SELECT MAX(postID) FROM T_Post";
                    var postId = db.QuerySingle<int>("SELECT ISNULL(MAX(postID), 0) AS MaxPostID FROM T_Post", transaction: tran) + 1;
                    db.Execute($@"INSERT INTO T_Post (postID, UserId, title, content, picture, Likes, createtime,nickname,VideoPath)
                    VALUES (@Par0, @Par1,@Par2,@Par3,@Par4,@Par5,@Par6,@Par7,@Par8)", 
                    new { 
                        Par0 = postId, 
                        Par1 = App.UserId, 
                        Par2 = SView.TxtTitle.Text, 
                        Par3 = SView.TxtContent.Text, 
                        Par4 = videoBytes ,Par5 = 0,
                        Par6=DateTime.Now, 
                        Par7 = App.userName,
                        Par8 = SView.TxtVdoPath.Text
                    }, tran);
                    tran.Commit();

                    //PostMessage postmessage = new PostMessage();
                    //postmessage.Show();
                    MessageBox1 form = new MessageBox1("Upload successfully!");
                    form.ShowDialog();
                    //MessageBox.Show("Upload successfully!");
                    this.RequestClose();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void SelectVdo(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video files (*.mp4)|*.mp4";

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                this.SView.TxtVdoPath.Text = Path.GetFileName(selectedFilePath);
                double fs = 0;
                double ds = 0;
                //上传视频
                if (File.Exists(selectedFilePath))
                {
                    ProgressView progressView = new ProgressView("Upload file");
                    progressView.Topmost = true;
                    progressView.Show();
                    UploadService.Upload(selectedFilePath, Boot.vdoHttp,
                        (p, max) =>
                        {
                            bool downloadNext = true;
                            fs = Convert.ToDouble(max);
                            ds = Convert.ToDouble(p);
                            System.Windows.Application.Current.Dispatcher.Invoke(() => {
                                progressView.prg.Value = ds / fs * 100;
                                progressView.txtCurrentProgress.Text = $"{Math.Round(ds / 1000000, 2)}M/{Math.Round(fs / 1000000, 2)}M";
                                // 如果窗口已经关闭，则停止下载
                                downloadNext = !progressView.IsClose;
                            });
                            return downloadNext;
                        },
                        (par1, par2) =>
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(() => {
                                progressView.Close();
                                MessageBox.Show(par2);
                            });
                        });
                }

            }
        }
    }
}
