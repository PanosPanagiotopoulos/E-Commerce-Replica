using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Application_API.Models
{
    public class ImageFile
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Filename is required.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "The field must be between 4 and 100 characters long.")]
        public string Filename { get; set; }
        [Required(ErrorMessage = "Blob URL is required.")]
        public string BlobURL { get; set; }
        public DateTime DateUploaded { get; set; }

        // The related product to which this image file belongs
        public Product Product { get; set; }
        public ImageFile()
        {
            this.DateUploaded = DateTime.UtcNow;
        }
    }
}
