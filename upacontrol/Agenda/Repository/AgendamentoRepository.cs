using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using upacontrol.Agenda.Models;
using upacontrol.Data;

namespace upacontrol.Agenda.Repository
{
    public class AgendamentoRepository : IAgendamentoRepository
    {

        
        private APIContext _context;

        //Carrega a connection string que aponta para o banco de dados
        public AgendamentoRepository(APIContext context)
        {
            _context = context;
        }

        //POST
        public async Task<AgendamentoModel> CadastrarAgendamento(AgendamentoNovoModel agendamento)
        {
            try
            {
                AgendamentoModel xAgendamentoNovo = new AgendamentoModel();

                xAgendamentoNovo.id_usuario = agendamento.id_usuario;
                xAgendamentoNovo.id_medico = agendamento.id_medico;
                xAgendamentoNovo.id_especialidade = agendamento.id_especialidade;
                xAgendamentoNovo.observacao = agendamento.observacao;
                xAgendamentoNovo.start = agendamento.start;
                xAgendamentoNovo.end = agendamento.end;
                xAgendamentoNovo.sticker = agendamento.sticker;                              

                _context.Add(xAgendamentoNovo);

                await _context.SaveChangesAsync();

                return xAgendamentoNovo;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        //PUT
        public async Task<AgendamentoModel> AtualizarAgendamento(AgendamentoModel agendamento, int id_agendamento)
        {
            if (id_agendamento <= 0)
            {
                return null;
            }

            try
            {
                _context.Update(agendamento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Agenda.Any(e => e.id == agendamento.id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return agendamento;
        }

        //GET GESTOR
        public async Task<List<vwLocAgendamento>> BuscarTodosAgendamentos()
        {
            var agendamentos = await _context.vwLocAgendamentos.ToListAsync();

            return agendamentos;
        }

        //GET PACIENTE
        public async Task<List<vwLocAgendamento>> BuscarAgendamentosPaciente(int id_paciente)
        {
            var agendamentos = await _context.vwLocAgendamentos.Where(a => a.id_usuario == id_paciente).ToListAsync();

            return agendamentos;
        }

        //GET MEDICO
        public async Task<List<vwLocAgendamento>> BuscarAgendamentosMedico(int id_medico)
        {
            var agendamentos = await _context.vwLocAgendamentos.Where(m => m.id_medico == id_medico).ToListAsync();

            return agendamentos;
        }

    }
}
