using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agri_Energy_Connect.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("FarmerId")]
        public int FarmerId { get; set; }
        
        [Required]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Product Description")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Production Date")]
        [DataType(DataType.Date)]
        public DateOnly ProductionDate { get; set; }
        [Required]
        public string? ImageUrl { get; set; }

        [Required]
        public byte []? Image { get; set; }
        [Required]
        [DisplayName("Product Type")]
        public string Type { get; set; }
    }
}