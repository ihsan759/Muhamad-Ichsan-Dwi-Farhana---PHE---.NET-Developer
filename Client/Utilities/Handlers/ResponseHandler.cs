namespace Client.Utilities.Handlers
{
    public class ResponseHandler<TEntity>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public TEntity? Data { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
    }
}
