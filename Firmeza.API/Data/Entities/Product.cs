using System.ComponentModel.DataAnnotations;

namespace Firmeza.API.Data.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre solo puede contener 100 caracteres maximo")]
        public string Name { get; set; } = String.Empty;

        [StringLength(255, ErrorMessage = "La descripción solo puede contener 255 caracteres maximo")]
        public string Description { get; set; } = String.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0, 100000000, ErrorMessage = "El precio debe ser un número entre 0 y 100000000")]
        public int Price { get; set; }
    }
}
