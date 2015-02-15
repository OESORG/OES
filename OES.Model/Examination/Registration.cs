using OES.Model.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class Registration : BaseEntity
    {
        public Registration()
        {
            CourseStudentId = GenerateKey();
        }
        [Key]
        public string CourseStudentId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }


        [ForeignKey("InstructorId")]
        public Instructor Instructor { get; set; }

        [ForeignKey("SemesterId")]
        public Semester Semester { get; set; }


        [Required]
        public string SemesterId { get; set; }

        [Required]
        public string InstructorId { get; set; }

        [Required]
        public string CourseId { get; set; }


        public List<StudentRegistration> Students { get; set; }
    }
}
