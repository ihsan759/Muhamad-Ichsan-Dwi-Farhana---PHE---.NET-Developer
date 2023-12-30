﻿namespace Register_Supply_Management.Utilities.Handlers
{
    public class ResponseHandlers<TEntity>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public TEntity? Data { get; set; }
    }
}
