using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using System.Net.Http;
using Newtonsoft.Json;
using static Map.Res;

namespace Map
{
    public partial class SelectForm : Form
    {
        private GMapOverlay RouteOverlay = new GMapOverlay("RouteOverlay");
        private PointLatLng RouteStartPoint = new PointLatLng(51.303065, 0.73175);
        private GMapImageMarker RouteStartMarker;
        public Action<double,double,string> MapAction;
        public string apiKey = "7PXhgAurAlbXtQVT5GP8Pzn5JwshqPpX";
        public bool CanDraw = false;
        public SelectForm(double lat = 51.303065, double lng = 0.73175)
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
            // 中心坐标
            mapControl.Position = new PointLatLng(lat, lng);
            mapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;
            mapControl.Overlays.Add(RouteOverlay);
            mapControl.MouseDown += new MouseEventHandler(mapControl_MouseDown);
            mapControl.MouseUp += new MouseEventHandler(mapControl_MouseUp);
            mapControl.MouseMove += new MouseEventHandler(mapControl_MouseMove);
            mapControl.MouseDoubleClick += new MouseEventHandler(mapControl_MouseDoubleClick);
            SelectPoint(mapControl.Position);
        





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

            panel1.Left = this.ClientSize.Width / 2 - panel1.Width / 2;
            //panel1.Width = this.ClientSize.Width - 20;
            panel1.Top = this.ClientSize.Height - panel1.Height - 10;


        mapControl.Width = this.ClientSize.Width - 20;

        mapControl.Height = panel1.Top - 20;

    }











    async void GetAddressInfo(PointLatLng p)
        {
            this.TxtPlace.Text = "Processing...";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.map.baidu.com/reverse_geocoding/v3/?ak={apiKey}&output=json&coordtype=wgs84ll&location={p.Lat},{p.Lng}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var str = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<RootObject>(str);
            if(obj.status == 0)
            {
                this.TxtPlace.Text = obj.result.formatted_address;
            }
            else
            {
                this.TxtPlace.Text = "Request Failed!";
            }
        }
        void mapControl_MouseDown(object sender, MouseEventArgs e)
        {
            CanDraw = true;
        }
        void SelectPoint(PointLatLng point)
        {
            RouteStartPoint = point;
            this.TxtLatitude.Text = point.Lat.ToString();
            this.TxtLongitude.Text = point.Lng.ToString();
            if (this.RouteOverlay.Markers.Contains(RouteStartMarker))
            {
                this.RouteOverlay.Markers.Remove(RouteStartMarker);
            }
            this.TxtPlace.Text = "获取中...";
            RouteStartMarker = new GMapImageMarker(RouteStartPoint, Properties.Resources.MapMarker_Bubble_Chartreuse);
            this.RouteOverlay.Markers.Add(RouteStartMarker);
            GetAddressInfo(point);
        }
        void mapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && CanDraw)
            {
                PointLatLng point = mapControl.FromLocalToLatLng(e.X, e.Y);
                SelectPoint(point);
            }
        }
        void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            CanDraw = false;
        }

        void mapControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PointLatLng point = mapControl.FromLocalToLatLng(e.X, e.Y);
        }

        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            MapAction(RouteStartPoint.Lat, RouteStartPoint.Lng, this.TxtPlace.Text);
            this.Close();
        }

        private void mapControl_Load(object sender, EventArgs e)
        {

        }

        private void SelectForm_Load(object sender, EventArgs e)
        {

        }
    }
}
