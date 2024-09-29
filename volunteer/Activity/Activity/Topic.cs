﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Activity
{
    public class Topic
    {

        public string Nickname { get; set; }
        // public string AvatarPath { get; set; }
        public string Title { get; set; }

        public string StuName { get; set; }

        public int StuID { get; set; }
        public byte[] ImageData { get; set; } // 用于存储图片的二进制数据

        public string ActivityName { get; set; }

        public string ActivityDate { get; set; }

        public string ActivityPopulation { get; set; }

        public int ActivityID { get; set; }

        public string Status { get; set; }

        public string ActivityType { get; set; }

        public string ActivityServePeople { get; set; }

        public string ActivityPlace { get; set; }

        public int NumOfInv { get; set; }
    }
}