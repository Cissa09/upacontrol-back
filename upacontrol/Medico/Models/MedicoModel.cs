using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace upacontrol.Medico.Models
{
    [Table("Medico")]
    public class MedicoModel
    {
        [Key]
        public int id { get; set; }
        public int id_especialidade { get; set; }
        public string nome { get; set; }
        public string sexo { get; set; }
        public string cpfcnpj { get; set; }
        public string rg { get; set; }
        public string crm { get; set; }
        public string data_nascimento { get; set; }
        public string obs { get; set; }
        public bool inativo { get; set; }
    }

    public class MedicoModelNovo
    {
        [Key]

        public int id_especialidade { get; set; }
        public string nome { get; set; }
        public string sexo { get; set; }
        public string cpfcnpj { get; set; }
        public string rg { get; set; }
        public string crm { get; set; }
        public string data_nascimento { get; set; }
        public string obs { get; set; }
        public bool inativo { get; set; }
    }
}
