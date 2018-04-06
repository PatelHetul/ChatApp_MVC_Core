using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Chat_App_MVC_Core.Models
{
    public class GroupDetails
    {
        [Key]
        public int Grp_id { get; set; }
        [Display(Name = "Group Name")]
        [Required(ErrorMessage = "Please Enter Group Name")]
        [StringLength(40, MinimumLength = 2)]
        public string Grp_Name { get; set; }

        [Display(Name = "Group Member List")]
        public string Grp_Member { get; set; }
        [Display(Name = "Group Admin")]
        public string Grp_Admin { get; set; }
    }
}
