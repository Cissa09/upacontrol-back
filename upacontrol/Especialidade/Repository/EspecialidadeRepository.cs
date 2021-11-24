using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using upacontrol.Data;
using upacontrol.Especialidade.Models;

namespace upacontrol.Especialidade.Repository
{
    public class EspecialidadeRepository : IEspecialidadeRepository
    {

        private APIContext _context;

        //Carrega a connection string que aponta para o banco de dados
        public EspecialidadeRepository(APIContext context)
        {
            _context = context;
        }

        //POST
        public async Task<EspecialidadeModel> CadastrarEspecialidade(EspecialidadeModelNovo especialidade)
        {
            try
            {
                EspecialidadeModel xEspNovo = new EspecialidadeModel();

                xEspNovo.nome = especialidade.nome;
                xEspNovo.descricao = especialidade.descricao;

                _context.Add(xEspNovo);

                await _context.SaveChangesAsync();

                return xEspNovo;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        //PUT
        public async Task<EspecialidadeModel> AtualizarEspecialidade(EspecialidadeModel especialidade, int id_especialidade)
        {
            if (id_especialidade <= 0)
            {
                return null;
            }

            try
            {
                _context.Update(especialidade);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EspecialidadeModel.Any(e => e.id == especialidade.id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return especialidade;
        }

        //GET
        public async Task<List<EspecialidadeModel>> BuscarEspecialidade()
        {
            var especialidades = await _context.EspecialidadeModel.ToListAsync();

            return especialidades;
        }
    }
}
