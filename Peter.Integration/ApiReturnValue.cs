using System;
using System.Collections.Generic;
using System.Text;

namespace Peter.Integration
{
    public class ApiReturnValue<T>
    {
        public bool IsSuccess { get; set; }
        public T Object { get; set; } = default;
        public ReturnMessage ReturnMessage { get; set; } = null;
    }

    public class ReturnMessage
    {
        public string Code { get; set; } = "";
        public string Message { get; set; } = "";
    }

    public class BoolReturnValue
    {
        public bool IsSuccess { get; set; }
        public ReturnMessage ReturnMessage { get; set; } = null;
    }

    public class APIResponse
    {
        string Data { get; set; }
        long Time { get; set; }
    }

    public class WebhookApiReturnError
    {
        public WebhookError Errors { get; set; }
    }
    public class WebhookError
    {
        public List<string> Format { get; set; }
    }
}
