using OES.Model.Examination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.InstructorArea.Models
{
    public class QuestionViewModel
    {
        public Chapter SelectedChapter { get; set; }
        public List<Registration> Registrations { get; set; }

        public Registration SelectedRegistration {get; set;}
    }
}