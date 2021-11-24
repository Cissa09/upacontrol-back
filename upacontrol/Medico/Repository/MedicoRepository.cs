using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using upacontrol.Data;
using upacontrol.Medico.Models;

namespace upacontrol.Medico.Repository
{
    public class MedicoRepository : IMedicoRepository
    {

        private APIContext _context;

        //Carrega a connection string que aponta para o banco de dados
        public MedicoRepository(APIContext context)
        {
            _context = context;
        }

        //POST
        public async Task<MedicoModel> CadastrarMedico(MedicoModelNovo medico)
        {
            try
            {
                MedicoModel xMedNovo = new MedicoModel();

                xMedNovo.id_especialidade = medico.id_especialidade;
                xMedNovo.nome = medico.nome;
                xMedNovo.cpfcnpj = medico.cpfcnpj;
                xMedNovo.rg = medico.rg;
                xMedNovo.crm = medico.crm;
                xMedNovo.data_nascimento = medico.data_nascimento;
                xMedNovo.inativo = medico.inativo;

                _context.Add(xMedNovo);

                await _context.SaveChangesAsync();

                return xMedNovo;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        //PUT
        public async Task<MedicoModel> AtualizarMedico(MedicoModel medico, int id_medico)
        {
            if (id_medico <= 0)
            {
                return null;
            }

            try
            {
                _context.Update(medico);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MedicoModel.Any(e => e.id == medico.id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return medico;
        }

        //GET
        public async Task<List<MedicoModel>> BuscarMedicos()
        {
            var medicos = await _context.MedicoModel.ToListAsync();

            return medicos;
        }
    }
}
