using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class ExamVersion : BaseEntity
    {
        public ExamVersion() {
            ExamVersionId = GenerateKey();
        }
        [Key]
        public string ExamVersionId { get; set; }

        public List<Question> Questions { get; set; }

        [Required]
        public string ExamId { get; set; }

        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }
    }
}
