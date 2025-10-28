using System.ComponentModel.DataAnnotations;

namespace AdminManager.Web.Data.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre solo puede contener 100 caracteres maximo")]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [StringLength(100, ErrorMessage = "El correo solo puede contener 100 caracteres maximo")]
        [EmailAddress(ErrorMessage = "Formato inválido (correo@ejemplo.com)")]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, ErrorMessage = "La contraseña solo puede contener 100 caracteres maximo")]
        public string Password { get; set; } = String.Empty;

        [Required(ErrorMessage = "El telefono es obligatorio")]
        [StringLength(20, ErrorMessage = "El telefono solo puede contener 20 caracteres maximo")]
        public string Phone { get; set; } = String.Empty;

        public User(string name, string email, string password, string phone)
        {
            Name = name;
            Email = email;
            Password = password;
            Phone = phone;
        }

        public User() { }
    }
}
