using System.ComponentModel.DataAnnotations;

namespace LMS_API.Models.Books
{
    public class UpdateBookRequest
    {
        /*[Display(Name = "Book Name")]
        [StringLength(40, MinimumLength = 4, ErrorMessage = "Book Name must be at least 4 characters long")]
        [Required]
        public string BookName { get; set; }*/


        [Display(Name = "Author Name")]
        [Required]
        public string AuthorName { get; set; }


        [Display(Name = "Publication Name")]
        [Required]
        public string PublicationName { get; set; }


        [Display(Name = "Cost Price Of Book")]
        [Required]
        public int Price { get; set; }


        [Display(Name = "Date OF Purchase")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }


        [Display(Name = "Purchased Quantity")]
        [Required]
        public int Quantity { get; set; }


        [Display(Name = "Book Loacation")]
        [Required]
        public string BookLocation { get; set; }


        [Display(Name = "Remaining Quantity")]
        [Required]
        public int RemainingQuantity { get; set; }
    }
}
