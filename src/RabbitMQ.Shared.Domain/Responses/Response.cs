﻿namespace RabbitMQ.Shared.Responses
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static Response<T> Ok(T data) => new() { Success = true, Data = data };
        public static Response<T> Fail(string errorMessage) => new() { Success = false, ErrorMessage = errorMessage };
    }
}