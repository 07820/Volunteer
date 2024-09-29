using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Map
{
    public partial class ShowForm : Form
    {
        private GMapOverlay RouteOverlay = new GMapOverlay("RouteOverlay");
        public ShowForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            mapControl.CacheLocation = Environment.CurrentDirectory + "\\GMapCache\\"; //缓存位置
            mapControl.MinZoom = 2;  //最小比例
            mapControl.MaxZoom = 24; //最大比例
            mapControl.Zoom = 12;     //当前比例
            mapControl.ShowCenter = false; //不显示中心十字点
            mapControl.DragButton = System.Windows.Forms.MouseButtons.Left; //左键拖拽地图
            mapControl.MapProvider = GMapProviders.GoogleMap;
            mapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            mapControl.Overlays.Add(RouteOverlay);


            this.Load += FormAddStu_Load;
            this.Resize += FormAddStu_Resize;

        }


        private void FormAddStu_Load(object sender, EventArgs e)
        {
            // 窗体加载时调整label1的位置
            AdjustLabel1Position();



        }

        private void FormAddStu_Resize(object sender, EventArgs e)
        {
            // 窗体加载时调整label1的位置
            AdjustLabel1Position();



        }





        private void AdjustLabel1Position()
        {
            mapControl.Location = new Point(10, 10);

            mapControl.Width = this.ClientSize.Width - 20;

            mapControl.Height = this.ClientSize.Height - 20;

        }













        public void SelectPoint(double lat,double lng)
        {
            var p = new PointLatLng(lat,lng);
            mapControl.Position = p;
            var RouteStartMarker = new GMapImageMarker(p, Properties.Resources.MapMarker_Bubble_Chartreuse);
            this.RouteOverlay.Markers.Add(RouteStartMarker);
        }

        private void mapControl_Load(object sender, EventArgs e)
        {

        }
    }
}
