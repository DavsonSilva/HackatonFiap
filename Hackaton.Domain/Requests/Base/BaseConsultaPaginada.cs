namespace Hackaton.Domain.Requests.Base
{
    public class BaseConsultaPaginada
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
        public string Search { get; set; } = string.Empty;
    }
}
