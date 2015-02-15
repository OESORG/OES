using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Model.Examination
{
    public class Semester : BaseEntity
    {
        public Semester() {
            SemesterId = GenerateKey();
        }
        [Key]
        public string SemesterId { get; set; }

        [Required]
        [Display(Name="Title")]
        public string SemesterTitle { get; set; }

        public List<Registration> Registrations { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
