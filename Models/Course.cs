using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Album.Models
{
    [Table("Courses")]
    public class Course
    {
        [Key]
        public int course_Id { get; set; }

        [Display(Name = "Title")]
        public string course_Name { get; set; }

        [Display(Name = "Describe")]
        public string course_Descrition { get; set; }
        //public List<RegisterEventCourse> RegisterEventCourse { get; set; }
        public List<RegisterCourse> RegisterCourse { get; set; }

        public List<Article> Articles { get; set; }




    }
}
