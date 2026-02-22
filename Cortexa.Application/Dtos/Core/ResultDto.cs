using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Dtos.Core
{
    public class ResultDto<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public static ResultDto<T> SuccessResult(T data, string message = "")
            => new() { Success = true, Data = data, Message = message };

        public static ResultDto<T> Failure(string message)
            => new() { Success = false, Message = message };
    }
}
