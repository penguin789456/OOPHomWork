using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HomeWork112.Models
{
    public class customCount
    {
        [DisplayName("姓名")]
        public string cName { get; set; }
        [DisplayName("統計")]
        public int oAmount { get; set; }

    }
}