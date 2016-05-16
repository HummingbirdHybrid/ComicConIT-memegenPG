using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicConIT.Models
{
    public class Medals
    {
        public int Id { get; set; }
        public string MedalName { get; set; }
        public string MedalImage { get; set; }

        public ICollection<User> Users { get; set; }
        public Medals()
        {
            Users = new List<User>();
        }
    }

    public class Comments
    {
        public int Id { get; set; }
        public string Content { get; set; }
 
        public int? UserId { get; set; }
        public User User { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Comments> Comments { get; set; }
        public User()
        {
            Comments = new List<Comments>();
        }
    }
    public class UserDBContext : DbContext
    {
        public DbSet<Comments> Commentes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Medals> Medals { get; set; }
    }
}
