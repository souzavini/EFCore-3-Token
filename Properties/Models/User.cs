using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{

    //[Table("Categoria")]
    public class User 
    {
        [Key]
        //[Column("Cat_ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(20,ErrorMessage = "Este campo deve conter entre 3 e 20 caracteres")]
        [MinLength(3,ErrorMessage="Este campo deve conter entre 3 e 20 caracteres")]
        //[DataType("nvarchar")]
        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }

}