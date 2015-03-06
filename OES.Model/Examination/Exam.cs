using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Display(Name = "Exam Start Date & Time")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name="Exam End Date & Time")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Number of High Difficulty Questions")]
        public int NumberOfHighQuestion { get; set; }

        [Required]
        [Display(Name="Score of High Question")]
        public int HighQuestionScore { get; set; }

        [Required]
        [Display(Name = "Number of Medium Difficulty Questions")]
        public int NumberOfMediumQuestion { get; set; }
        [Required]
        [Display(Name = "Score of Medium Question")]
        public int MediumQuestionScore { get; set; }

        [Required]
        [Display(Name = "Number of Low Difficulty Questions")]
        public int NumberOfLowQuestion { get; set; }
        [Required]
        [Display(Name = "Score of Low Question")]
        public int LowQuestionScore { get; set; }

        [ForeignKey("RegistrationId")]
        public Registration Registration { get; set; }
        public string RegistrationId { get; set; }

    }
}
