using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Coles.Api.Exceptions
{
    public class ApiClientException : Exception
    {
        /// <summary>
        /// Http status code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        public ApiClientException() : base()
        {
        }

        /// <summary>
        /// Construct exception from content and HttpResponse
        /// </summary>
        /// <param name="content"></param>
        /// <param name="responseMessage"></param>
        public ApiClientException(string content, HttpResponseMessage responseMessage)
            : base(BuildMessage(content, responseMessage))
        {
            StatusCode = responseMessage.StatusCode;
        }

        private static string BuildMessage(string content, HttpResponseMessage responseMessage)
        {
            return $"{(int)responseMessage.StatusCode} [{responseMessage.RequestMessage.Method}] {responseMessage.RequestMessage.RequestUri}\t{content ?? responseMessage.ReasonPhrase}";
        }

        public ApiClientException(string message) : base(message)
        {
        }

        public ApiClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
