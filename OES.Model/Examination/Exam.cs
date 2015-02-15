using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class Exam : BaseEntity
    {
        public Exam()
        {
            ExamId = GenerateKey();
        }

        public string ExamId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<ExamDifficulty> ExamDifficulties { get; set; }

        [ForeignKey("CourseStudentId")]
        public Registration Course { get; set; }
        public string CourseStudentId { get; set; }

    }
}
