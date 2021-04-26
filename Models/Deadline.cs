using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Album.Models
{
    [Table("Deadlines")]

    public class Deadline
    {
        public int dl_Id { get; set; }

        
        [Display(Name = "Title")]
        public string Title { get; set; }


        [Display(Name = "Duration")]
        [DataType(DataType.Date)]
        public DateTime dl_TimeDeadline { get; set; }


        [Display(Name = "Content")]
        public string dl_Description { set; get; }


        [Display(Name = "Submitted by")]
        public string dl_CreateBy { set; get; }


        [Display(Name = "Status")]
        public string dl_Status { set; get; }


        [Display(Name = "Edited by")]
        public string dl_ModifiedBy { set; get; }


        [Display(Name = "Innitiated date")]
        [DataType(DataType.Date)]
        public DateTime dl_CreateDate { set; get; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }


        public List<UserFile> UserFile { get; set; }

        public List<Comment> Comment { get; set; }

        



    }
}
