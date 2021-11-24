using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using upacontrol.Especialidade.Models;
using upacontrol.Medico.Models;

namespace upacontrol.Medico.Repository
{
    public interface IMedicoRepository
    {
        Task<MedicoModel> CadastrarMedico(MedicoModelNovo medico);
        Task<MedicoModel> AtualizarMedico(MedicoModel medico, int id);
        Task<List<MedicoModel>> BuscarMedicos();
    }
}
