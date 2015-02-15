using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class ExamDifficulty : BaseEntity
    {
        public ExamDifficulty()
        {
            ExamDifficultyId = GenerateKey();
        }

        public string ExamDifficultyId { get; set; }
        public Difficulty Difficulty { get; set; }
        public decimal Percentage { get; set; }

        public Exam Exam { get; set; }
    }
    public enum Difficulty
    {
        High,
        Meduim,
        Low
    }
}
