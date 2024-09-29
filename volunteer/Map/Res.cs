using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map
{
    public class Res
    {
        public class Location
        {
            public double lng { get; set; }
            public double lat { get; set; }
        }

        public class Edz
        {
            public string name { get; set; }
        }

        public class AddressComponent
        {
            public string country { get; set; }
            public int country_code { get; set; }
            public string country_code_iso { get; set; }
            public string country_code_iso2 { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public int city_level { get; set; }
            public string district { get; set; }
            public string town { get; set; }
            public string town_code { get; set; }
            public string distance { get; set; }
            public string direction { get; set; }
            public string adcode { get; set; }
            public string street { get; set; }
            public string street_number { get; set; }
        }

        public class Result
        {
            public Location location { get; set; }
            public string formatted_address { get; set; }
            public Edz edz { get; set; }
            public string business { get; set; }
            public AddressComponent addressComponent { get; set; }
            public List<object> pois { get; set; }
            public List<object> roads { get; set; }
            public List<object> poiRegions { get; set; }
            public string sematic_description { get; set; }
            public string formatted_address_poi { get; set; }
            public int cityCode { get; set; }
        }

        public class RootObject
        {
            public int status { get; set; }
            public Result result { get; set; }
        }
    }
}
