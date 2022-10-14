using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recipes.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Domain.Shared
{
    public class Error : ValueObject
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
            StackTrace = Environment.StackTrace;
        }
        protected string StackTrace { get;}
        public string Code { get; }
        public string Message { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Message;
        }

        public static implicit operator string(Error error) => error.Code;
    }
}
