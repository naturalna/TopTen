using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTenApp.Models
{
    public class Groups
    {
        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }

        public Groups()
        {

        }
    }
}
