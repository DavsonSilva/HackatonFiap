namespace Hackaton.Domain.Requests.Agenda
{
    public class AgendaRequest
    {
        private DateTime _dataHora;

        public DateTime DataHora
        {
            get => _dataHora;
            set => _dataHora = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        public bool Disponivel { get; set; } = true;
    }
}
