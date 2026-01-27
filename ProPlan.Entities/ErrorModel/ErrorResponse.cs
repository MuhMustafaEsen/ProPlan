using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProPlan.Entities.ErrorModel
{
        public class ErrorResponse
        {
            public int StatusCode { get; set; }
            public string Message { get; set; } = string.Empty;
            public Dictionary<string, string[]>? Errors { get; set; }
            public string? Details { get; set; }

            public static ErrorResponse Create(int statusCode, string message, Dictionary<string, string[]>? errors = null, string? details = null)
            {
                return new ErrorResponse
                {
                    StatusCode = statusCode,
                    Message = message,
                    Errors = errors,
                    Details = details
                };
            }
        }
    }

