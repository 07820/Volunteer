﻿using GMap.NET.WindowsForms.ToolTips;
using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map
{
    public class GMapImageMarker : GMapMarker
    {
        private Image image;

        public GMapImageMarker(PointLatLng p, Image image, string tipText = "")
            : base(p)
        {
            this.image = image;
            Size = new System.Drawing.Size(image.Width, image.Height);
            Offset = new System.Drawing.Point(-Size.Width / 2, -Size.Height / 2);
            this.ToolTipText = tipText;
            this.ToolTip = new GMapBaloonToolTip(this);
        }

        public override void OnRender(Graphics g)
        {
            if (image == null) return;

            Rectangle rect = new Rectangle(LocalPosition.X, LocalPosition.Y - Size.Height / 2, Size.Width, Size.Height);
            g.DrawImage(image, rect);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
