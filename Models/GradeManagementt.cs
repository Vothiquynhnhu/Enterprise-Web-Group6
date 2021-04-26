using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Album.Models
{
    public class GradeManagementt
    {
        [Key]
        public int Id { get; set; }
        public int grade { get; set; }
        public List<RegisterGrade> RegisterGrade { get; set; }

    }
}
