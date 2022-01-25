using System.ComponentModel.DataAnnotations;

namespace Primeira_api_data_driven_asp.Models
{
    public class User
    {
        [Key]

        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo e obrigatorio.")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres.")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres.")]

        public string Username { get; set; }

        [Required(ErrorMessage = "Este campo e obrigatorio.")]
        [MaxLength(20, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres.")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres.")]

        public string Password { get; set; }

        public string Role { get; set; }
    }
}