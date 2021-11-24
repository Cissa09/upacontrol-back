using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace upacontrol.Especialidade.Models
{
    [Table("Especialidade")]
    public class EspecialidadeModel
    {
        [Key]
        public int id { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
    }

    public class EspecialidadeModelNovo
    {
        public string nome { get; set; }
        public string descricao { get; set; }
    }
}
