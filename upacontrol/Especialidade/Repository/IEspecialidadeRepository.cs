using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using upacontrol.Especialidade.Models;

namespace upacontrol.Especialidade.Repository
{
    public interface IEspecialidadeRepository
    {
        Task<EspecialidadeModel> CadastrarEspecialidade(EspecialidadeModelNovo especialidade);
        Task<EspecialidadeModel> AtualizarEspecialidade(EspecialidadeModel especialidade, int id);
        Task<List<EspecialidadeModel>> BuscarEspecialidade();
    }
}
