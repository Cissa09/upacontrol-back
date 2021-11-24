using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using upacontrol.Medico.Models;
using upacontrol.Medico.Repository;

namespace upacontrol.Medico.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MedicoController : Controller
    {

        private readonly IMedicoRepository _medicoRepository;

        public MedicoController(IMedicoRepository medico)
        {
            _medicoRepository = medico;
        }

        // POST api/<EspecialidadeController>
        [HttpPost]
        public async Task<ActionResult<MedicoModel>> CadastrarEspecialidade([FromBody]MedicoModelNovo medico)
        {
            var especialidadeNova = await _medicoRepository.CadastrarMedico(medico);

            if (especialidadeNova == null)
            {
                return BadRequest($"Nenhum médico foi encontrado");
            }

            return especialidadeNova;
        }

        // PUT api/<EspecialidadeController>
        [HttpPut("{id_especialidade}")]
        public async Task<ActionResult<MedicoModel>> AtualizarEspecialidade([FromBody]MedicoModel medico, int id_medico)
        {
            if (id_medico <= 0)
            {
                return BadRequest($"O identificador do médico não foi informado");
            }

            var EspecialidadeAtualizada = await _medicoRepository.AtualizarMedico(medico, id_medico);

            if (EspecialidadeAtualizada == null)
            {
                return BadRequest($"Nenhum médico foi encontrado");
            }

            return EspecialidadeAtualizada;
        }

        // GET: api/<EspecialidadeController>
        [HttpGet]
        public async Task<ActionResult<List<MedicoModel>>> BuscarEspecialidade()
        {
            var especialidades = await _medicoRepository.BuscarMedicos();

            if (especialidades == null)
            {
                return NotFound();
            }

            return especialidades;
        }
    }
}
