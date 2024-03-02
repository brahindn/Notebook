
namespace Notebook.WebApi.Responses
{
    public record LogsResponseDTO
    {
        public string Id {  get; set; }
        public string UtcTimesTamp { get; set; }
        public string Level { get; set; }
        public string Message {  get; set; }
    }
}
