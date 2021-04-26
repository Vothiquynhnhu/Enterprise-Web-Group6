using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Album.Models
{
    [Table("Articles")]
    public class Article
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Date Submitted")]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }

        [Display(Name = "Content")]
        public string Content { set; get; }
       // public List<RegisterEventCourse> RegisterEventCourse { get; set; }
        public List<Deadline> Deadline { get; set; }
        public int courseId { get; set; }
        public Course course { get; set; }
        public List<RegisterGrade> RegisterGrade { get; set; }

    }
}
