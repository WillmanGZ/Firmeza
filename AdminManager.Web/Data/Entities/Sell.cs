using System.ComponentModel.DataAnnotations;

namespace AdminManager.Web.Data.Entities
{
    public class Sell
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "La fecha de venta es obligatoria")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El Id del cliente es obligatorio")]
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public ICollection<SellProduct> SellProducts { get; set; } = new List<SellProduct>();
    }
}
