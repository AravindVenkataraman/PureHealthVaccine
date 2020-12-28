using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace pureVaccineWebAPI.MessageHandlers
{
    public class CorsMessageHandler : DelegatingHandler
    {
        static string origins = "*";
        static string headers = "Access-Control-Allow-Headers', 'Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers,X-Access-Token,XKey,Authorization";
        static string methods = "GET, POST, OPTIONS, PUT, DELETE";
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Headers.Contains("Origin") && request.Method.Method.Equals("OPTIONS"))
                {
                    var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
                    // Define and add values to variables: origins, headers, methods (can be global)               
                    response.Headers.Add("Access-Control-Allow-Origin", origins);
                    response.Headers.Add("Access-Control-Allow-Headers", headers);
                    response.Headers.Add("Access-Control-Allow-Methods", methods);
                    response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(response);
                    return tsc.Task;
                }
            }
            catch(Exception ex)
            {

            }
            return base.SendAsync(request, cancellationToken);
        } 
    }

}