using System.ComponentModel.DataAnnotations;

namespace AdminManager.Web.Data.Entities
{
    public class SellProduct
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid SellId { get; set; }
        public Sell Sell { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, 1000, ErrorMessage = "La cantidad debe ser entre 1 y 1000")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Range(0, 100000000, ErrorMessage = "El precio debe ser un número entre 0 y 100000000")]
        public int UnitPrice { get; set; }
    }
}
