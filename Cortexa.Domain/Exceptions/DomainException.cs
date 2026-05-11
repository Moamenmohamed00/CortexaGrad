using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        public int StatusCode { get; }
        public string Title { get; }

        protected DomainException(string message, int statusCode, string title)
            : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}
