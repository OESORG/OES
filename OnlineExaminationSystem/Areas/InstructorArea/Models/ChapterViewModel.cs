﻿using OES.Model.Examination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.InstructorArea.Models
{
    public class ChapterViewModel
    {
        public List<Registration> Regisatrations { get; set; }

        public Registration SelectedRegisteration { get; set; }
    }
}