using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Album.Models
{
    public class RegisterGrade
    {
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        //public int CourseId { get; set; }
        public int EventId { get; set; }
        public Article Article { get; set; }

        public int GradeId { get; set; }
        public GradeManagementt GradeManagementt { get; set; }


    }
}
