﻿using System;
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
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }
        [Key]
        public string ExamVersionId { get; set; }

        public List<QuestionVersion> Questions { get; set; }
        

        [Required]
        [Display(Name = "Exam Start Date & Time")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Exam End Date & Time")]
        public DateTime EndDate { get; set; }

        #region MCQ

        [Required]
        [Display(Name = "High Difficulty Number")]
        public int MCQHigh { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal MCQHighScore { get; set; }
        [Required]
        [Display(Name = "Medium Difficulty Number")]
        public int MCQMedium { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal MCQMediumScore { get; set; }
        [Required]
        [Display(Name = "Low Difficulty Number")]
        public int MCQLow { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal MCQLowScore { get; set; }

        #endregion

        #region Complete

        [Required]
        [Display(Name = "High Difficulty Number")]
        public int CompleteHigh { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal CompleteHighScore { get; set; }
        [Required]
        [Display(Name = "Medium Difficulty Number")]
        public int CompleteMedium { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal CompleteMediumScore { get; set; }
        [Required]
        [Display(Name = "Low Difficulty Number")]
        public int CompleteLow { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal CompleteLowScore { get; set; }

        #endregion


        #region TrueFalse

        [Required]
        [Display(Name = "High Difficulty Number")]
        public int TrueFalseHigh { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal TrueFalseHighScore { get; set; }
        [Required]
        [Display(Name = "Medium Difficulty Number")]
        public int TrueFalseMedium { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal TrueFalseMediumScore { get; set; }
        [Required]
        [Display(Name = "Low Difficulty Number")]
        public int TrueFalseLow { get; set; }
        [Required]
        [Display(Name = "Score")]
        public decimal TrueFalseLowScore { get; set; }

        #endregion


        [ForeignKey("RegistrationId")]
        public Registration Registration { get; set; }
        public string RegistrationId { get; set; }

    }
}
