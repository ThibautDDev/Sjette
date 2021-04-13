using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Sjette.Models
{
    // Activities model
    public class GroupMembership
    {
        [Key]
        public int GroupMembershipID { get; set; }

        public int UserID { get; set; }
        public int GroupID { get; set; }
    }
}
