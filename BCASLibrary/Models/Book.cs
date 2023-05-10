using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCASLibrary.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Book Title")]
        public string Title { get; set; }
        [Required]
        [DisplayName("Author/s")]
        public string Author { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Isbn { get; set; }
        [Required]
        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        [ValidateNever]

        public Genre Genre { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

    }

}
