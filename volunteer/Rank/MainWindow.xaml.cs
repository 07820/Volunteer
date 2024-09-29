using Dapper;
using Rank.Config;
using Rank.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Config;

namespace Rank
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var conn = ConfigInfo.GetDbConnectionString();
            DBHelper.Init(SimpleCRUD.Dialect.SQLServer, conn);
            using (var db = DBHelper.GetConn())
            {
                var item = db.Query<RankModel>("select nickName as UserName,credit as Score,sign as Sign,avatar from student_information ORDER BY credit desc").ToList();
                int i = 1;
                item.ForEach(x => { x.RankNum = i++; });
                if(item.Count > 0)
                {
                    GrdRank2.DataContext = item[0];
                }
                if (item.Count > 1)
                {
                    GrdRank1.DataContext = item[1];
                }
                if (item.Count > 2)
                {
                    GrdRank3.DataContext = item[2];
                }
                List<RankModel> rankModels = new List<RankModel>();
                item.ForEach((e) =>
                {
                    if(e.RankNum != 1 && e.RankNum != 2 && e.RankNum != 3)
                    {
                        rankModels.Add(e);
                    }
                });
                ItemRank.ItemsSource = rankModels;
            }
        }
    }
}
