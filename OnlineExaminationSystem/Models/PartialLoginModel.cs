using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Models
{
    public class PartialLoginModel
    {
        public bool IsLogedIn { get; set; }
        public string UserName { get; set; }
    }
}