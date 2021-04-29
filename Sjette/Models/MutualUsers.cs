using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Sjette.Models
{
    public class MutualUsers
    {   
        [Key]
        public long row { get; set; }

        public int pk_UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Admin { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Hash { get; set; }
        public DateTime CreationDate { get; set; }
        public int GroupID { get; set; }
    }
}
