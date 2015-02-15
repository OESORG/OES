using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class Question : BaseEntity
    {
        [Key]
        public string QuestionId { get; set; }
        public Question()
        {
            QuestionId = GenerateKey();
        }

        public string QuestionText { get; set; }

        public string ChapterId { get; set; }

        public QuestionType Type { get; set; }

        public List<Answer> Answers { get; set; }

        [ForeignKey("ChapterId")]
        public Chapter Chapter { get; set; }
    }

    public enum QuestionType
    {
        MCQ,
        TrueFalse,
        Complete
    }
}
