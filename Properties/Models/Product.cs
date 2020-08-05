using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{

    //[Table("Categoria")]
    public class Product 
    {
        [Key]
        //[Column("Cat_ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60,ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3,ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        //[DataType("nvarchar")]
        public string Title { get; set; }

        [MaxLength(1024,ErrorMessage = "Este campo deve conter no máximo 1024 caracteres")]
        public string Description { get; set; }
    
        [Required(ErrorMessage=" Este campo é Obrigratorio")]
        [Range(1,int.MaxValue,ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatorio")]
        [Range(1,int.MaxValue, ErrorMessage = "Categoria invalida")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }

}