using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace upacontrol.Agenda.Models
{

    [Table("Agendamento")]
    public class AgendamentoModel
    {
        [Key]
        public int id { get; set; }
        public int id_medico { get; set; }
        public int id_usuario { get; set; }
        public int id_especialidade { get; set; }
        public string data_nascimento { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public decimal valor { get; set; }
        public int status { get; set; }
        public string sticker { get; set; }
        public string observacao { get; set; }
    }

    public class AgendamentoNovoModel
    {
        public int id_medico { get; set; }
        public int id_usuario { get; set; }
        public int id_especialidade { get; set; }
        public string data_nascimento { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public decimal valor { get; set; }
        public int status { get; set; }
        public string sticker { get; set; }
        public string observacao { get; set; }
    }

    [Table("vwLocAgendamento")]
    public class vwLocAgendamento
    {
        public int id_usuario { get; set; }
        public int id_medico { get; set; }
        public string nome_medico { get; set; }
        public int id_especialidade { get; set; }
        public string nome_especialidade { get; set; }
        public string horario_inicio { get; set; }
        public string horario_fim { get; set; }
    }
}
