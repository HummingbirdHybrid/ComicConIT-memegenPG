using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ComicConIT.Models
{
    public class Comics
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        public string Title { get; set; }


        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}")]
        public DateTime ReleaseDate { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        public string ImagePath { get; set; }

        public string UserName { get; set; }

        public string Rating { get; set; }

        public int Rate { get; set; } 

      //  public virtual ApplicationUser ApplicationUser { get; set; }

    }

  
        
    

    public class ComicsDBContext : DbContext
    {
        public DbSet<Comics> Comics { get; set; }
    }
}
