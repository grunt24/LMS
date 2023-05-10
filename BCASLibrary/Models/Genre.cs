using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BCASLibrary.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Genre Name")]
        public string GenreName { get; set; }
    }
}
