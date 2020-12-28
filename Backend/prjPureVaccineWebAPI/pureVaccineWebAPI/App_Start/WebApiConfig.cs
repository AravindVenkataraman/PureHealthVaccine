using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using pureVaccineWebAPI.Filters;

namespace pureVaccineWebAPI
{
    public static class WebApiConfig
    {
        static string origins = "*";  // For multiple domains http://domain1.example, http://domain2.example
        static string headers = "Access-Control-Allow-Headers', 'Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers,X-Access-Token,XKey,Authorization";
        static string methods = "GET, POST, PUT";
        public static void Register(HttpConfiguration config)
        {
            //Enable or Disable User/Id Password
            //config.Filters.Add(new UserAuthenticationAttribute());

            //Security headers
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true; // **** Always accept
            };

            // Enable global CORS
            config.EnableCors(new EnableCorsAttribute(origins, headers, methods));

            // Add handler to deal with preflight requests, this is the important part
            config.MessageHandlers.Add(new MessageHandlers.CorsMessageHandler());

            config.MapHttpAttributeRoutes();

            //Register your api here 
            config.Routes.MapHttpRoute(
              name: "saveVaccinationApplication", routeTemplate: "api/{controller}/{saveVaccinationApplication}");

            config.Routes.MapHttpRoute(
              name: "getVaccinationLocationForCity", routeTemplate: "api/{controller}/{getVaccinationLocationForCity}/{cityId}");

            config.Routes.MapHttpRoute(
              name: "getVaccinationAppointment", routeTemplate: "api/{controller}/{getVaccinationAppointment}");

            config.Routes.MapHttpRoute(
             name: "getEmiratesList", routeTemplate: "api/{controller}/{getEmiratesList}");

            config.Routes.MapHttpRoute(
             name: "validateUserLogin", routeTemplate: "api/{controller}/{validateUserLogin}");

            config.Routes.MapHttpRoute(
             name: "sendOtpSMS", routeTemplate: "api/{controller}/{sendOtpSMS}");

            config.Routes.MapHttpRoute(
             name: "validateOTP", routeTemplate: "api/{controller}/{validateOTP}");

            config.Routes.MapHttpRoute(
                        name: "getVaccinationApplicationDetails", routeTemplate: "api/{controller}/{getVaccinationApplicationDetails}");

            config.Routes.MapHttpRoute(
                        name: "saveNursingObservationForPatient", routeTemplate: "api/{controller}/{saveNursingObservationForPatient}");

            config.Routes.MapHttpRoute(
                        name: "getAllLocationsForVaccination", routeTemplate: "api/{controller}/{getAllLocationsForVaccination}");

            config.Routes.MapHttpRoute(
                                    name: "updateDEOResponseForApplication", routeTemplate: "api/{controller}/{updateDEOResponseForApplication}");
            config.Routes.MapHttpRoute(
                       name: "saveAdverseReactionForPatient", routeTemplate: "api/{controller}/{saveAdverseReactionForPatient}");
            config.Routes.MapHttpRoute(
                      name: "getAdverseReactionForPatient", routeTemplate: "api/{controller}/{getAdverseReactionForPatient}");

            config.Routes.MapHttpRoute(
                     name: "getPatientNursingObservation", routeTemplate: "api/{controller}/{getPatientNursingObservation}");
            config.Routes.MapHttpRoute(
                     name: "getPatientDetailsByEID", routeTemplate: "api/{controller}/{getPatientDetailsByEID}");

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
