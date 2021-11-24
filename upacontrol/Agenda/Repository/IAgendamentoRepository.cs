using System.Collections.Generic;
using System.Threading.Tasks;
using upacontrol.Agenda.Models;

namespace upacontrol.Agenda.Repository
{
    public interface IAgendamentoRepository
    {
        Task<AgendamentoModel> CadastrarAgendamento(AgendamentoNovoModel agendamento);
        Task<AgendamentoModel> AtualizarAgendamento(AgendamentoModel agendamento, int id);
        Task<List<vwLocAgendamento>> BuscarTodosAgendamentos();
        Task<List<vwLocAgendamento>> BuscarAgendamentosPaciente(int id_paciente);
        Task<List<vwLocAgendamento>> BuscarAgendamentosMedico(int id_medico);
    }
}
