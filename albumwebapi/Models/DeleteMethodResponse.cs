namespace albumwebapi.Models
{
    public class DeleteMethodResponse
    {
        public int Code { get; set; }
        public string Description { get; set; }

        public DeleteMethodResponse(int code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
