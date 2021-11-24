using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using upacontrol.Agenda.Models;
using upacontrol.Agenda.Repository;
using static upacontrol.Agenda.Models.AgendamentoModel;

namespace upacontrol.Agenda.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AgendamentoController : Controller
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public AgendamentoController(IAgendamentoRepository agendamento)
        {
            _agendamentoRepository = agendamento;
        }

        // POST api/<EspecialidadeController>
        [HttpPost]
        public async Task<ActionResult<AgendamentoModel>> CadastrarAgendamento(AgendamentoNovoModel agendamento)
        {
            var agedamentdoNovo = await _agendamentoRepository.CadastrarAgendamento(agendamento);

            if (agedamentdoNovo == null)
            {
                return BadRequest($"Nenhum agendamento foi encontrado");
            }

            return agedamentdoNovo;
        }

        // PUT api/<EspecialidadeController>
        [HttpPut("{id_agendamento}")]
        public async Task<ActionResult<AgendamentoModel>> AtualizarEspecialidade([FromBody] AgendamentoModel agendamento, int id_agendamento)
        {
            if (id_agendamento <= 0)
            {
                return BadRequest($"O identificador do agendamento não foi informado");
            }

            var agendamentoAtualizado = await _agendamentoRepository.AtualizarAgendamento(agendamento, id_agendamento);

            if (agendamentoAtualizado == null)
            {
                return BadRequest($"Nenhum agendamento foi encontrado");
            }

            return agendamentoAtualizado;
        }

        // GET: Gestor
        [HttpGet]
        public async Task<ActionResult<List<vwLocAgendamento>>> BuscaTodosAgendamentos()
        {
            var agendamentos = await _agendamentoRepository.BuscarTodosAgendamentos();

            if (agendamentos == null)
            {
                return NotFound();
            }

            return agendamentos;
        }

        // GET: Paciente
        [HttpGet("{id_paciente}")]
        public async Task<ActionResult<List<vwLocAgendamento>>> BuscarAgendamentosPaciente(int id_paciente)
        {
            var agendamentos = await _agendamentoRepository.BuscarAgendamentosPaciente(id_paciente);

            if (agendamentos == null)
            {
                return NotFound();
            }

            return agendamentos;
        }

        // GET: Medico
        [HttpGet("{id_medico}")]
        public async Task<ActionResult<List<vwLocAgendamento>>> BuscarAgendamentosMedico(int id_medico)
        {
            var agendamentos = await _agendamentoRepository.BuscarAgendamentosMedico(id_medico);

            if (agendamentos == null)
            {
                return NotFound();
            }

            return agendamentos;
        }
    }
}
