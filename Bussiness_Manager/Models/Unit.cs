using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bussiness_Manager.Models
{
    public class Unit
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName =("nvarchar(200)"))]
        public string? unitName {  get; set; }
    }
}
