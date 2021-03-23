using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Sjette.Models
{
    public class Groups
    {
        [Key]
        public int pk_GroupID { get; set; }

        public int fk_CreatorID { get; set; }
        public string GroupName { get; set; }
    }
}
