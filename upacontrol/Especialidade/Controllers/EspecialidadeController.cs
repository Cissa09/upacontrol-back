using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using upacontrol.Especialidade.Models;
using upacontrol.Especialidade.Repository;

namespace upacontrol.Especialidade.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EspecialidadeController : Controller
    {

        private readonly IEspecialidadeRepository _especialidadeRepository;

        public EspecialidadeController(IEspecialidadeRepository especialidade)
        {
            _especialidadeRepository = especialidade;
        }

        // POST api/<EspecialidadeController>
        [HttpPost]
        public async Task<ActionResult<EspecialidadeModel>> CadastrarEspecialidade([FromBody]EspecialidadeModelNovo especialidade)
        {
            var especialidadeNova = await _especialidadeRepository.CadastrarEspecialidade(especialidade);

            if (especialidadeNova == null)
            {
                return BadRequest($"Nenhuma especialidade encontrada");
            }

            return especialidadeNova;
        }

        // PUT api/<EspecialidadeController>
        [HttpPut("{id_especialidade}")]
        public async Task<ActionResult<EspecialidadeModel>> AtualizarEspecialidade([FromBody]EspecialidadeModel especialidade, int id_especialidade)
        {
            if (id_especialidade <= 0)
            {
                return BadRequest($"O identificador da especialidade não foi informado");
            }

            var EspecialidadeAtualizada = await _especialidadeRepository.AtualizarEspecialidade(especialidade, id_especialidade);

            if (EspecialidadeAtualizada == null)
            {
                return BadRequest($"Nenhuma especialidade foi encontrada");
            }

            return EspecialidadeAtualizada;
        }

        // GET: api/<EspecialidadeController>
        [HttpGet]
        public async Task<ActionResult<List<EspecialidadeModel>>> BuscarEspecialidade()
        {
            var especialidades = await _especialidadeRepository.BuscarEspecialidade();

            if (especialidades == null)
            {
                return NotFound();
            }

            return especialidades;
        }
    }
}
