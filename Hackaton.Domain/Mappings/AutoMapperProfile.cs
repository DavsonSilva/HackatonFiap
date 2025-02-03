using AutoMapper;
using Hackaton.Domain.Entities.AgendaEntity;
using Hackaton.Domain.Entities.ConsultaEntity;
using Hackaton.Domain.Entities.MedicoEntity;
using Hackaton.Domain.Entities.NotificacaoEntity;
using Hackaton.Domain.Entities.PacienteEntity;
using Hackaton.Domain.Requests.Agenda;
using Hackaton.Domain.Requests.Consulta;
using Hackaton.Domain.Requests.Medico;
using Hackaton.Domain.Requests.Notificacao;
using Hackaton.Domain.Requests.Paciente;
using Hackaton.Domain.Responses;

namespace Hackaton.Domain.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Medico, MedicoResponse>();
            CreateMap<CreateMedicoRequest, Medico>();
            CreateMap<UpdateMedicoRequest, Medico>();

            CreateMap<Paciente, PacienteResponse>();
            CreateMap<CreatePacienteRequest, Paciente>();
            CreateMap<UpdatePacienteRequest, Paciente>();

            CreateMap<Consulta, ConsultaResponse>();
            CreateMap<CreateConsultaRequest, Consulta>();
            CreateMap<UpdateConsultaRequest, Consulta>();

            CreateMap<Agenda, AgendaResponse>();
            CreateMap<CreateAgendaRequest, Agenda>();
            CreateMap<UpdateAgendaRequest, Agenda>();

            CreateMap<Notificacao, NotificacaoResponse>();
            CreateMap<CreateNotificacaoRequest, Notificacao>();

            CreateMap<Consulta, ConsultaDetalhadaResponse>()
                .ForMember(dest => dest.NomeMedico, opt => opt.MapFrom(src => src.Medico.Nome))
                .ForMember(dest => dest.NomePaciente, opt => opt.MapFrom(src => src.Paciente.Nome));

            CreateMap<CreateMedicoRequest, Medico>()
                .ForMember(dest => dest.HorariosDisponiveis, opt => opt.Ignore()); 

            CreateMap<AgendaRequest, Agenda>();

            CreateMap<Medico, MedicoResponse>()
                .ForMember(dest => dest.HorariosDisponiveis, opt => opt.MapFrom(src => src.HorariosDisponiveis));

        }
    }
}
