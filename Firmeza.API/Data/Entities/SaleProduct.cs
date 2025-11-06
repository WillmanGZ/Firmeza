using System.ComponentModel.DataAnnotations;

namespace Firmeza.API.Data.Entities
{
    public class SaleProduct
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid SaleId { get; set; }
        public Sale Sale { get; set; }

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
