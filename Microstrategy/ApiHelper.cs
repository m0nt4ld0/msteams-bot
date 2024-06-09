
using RestSharp;
using System.Linq;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using EchoBot.Microstrategy.Class;
using Nancy.Json;
using System;
using System.Security.Cryptography.Xml;

namespace EchoBot.Microstrategy
{
    /*
     * Endpoint
     * Tipo de peticion - Get o Post
     * Array de objeto que va a ser el cuerpo del objeto
     * Lectura de archivo de configuracion con datos de conexion (usuario, contraseña, endpoint) encriptados
     * Un archivo de configuracion para ambiente de test y otro para produccion
     */
    
    public class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }
        private static RestClient Cliente;
        private static string AuthToken;


        public static string ProjectID { get; set; }

        public static string CubeID { get; set; }
        private static string InstanceID;

        /*
         * 1) Login /auth/login
         * 2) Obtener definicion del cubo GET /cubes/{cubeId}
         * 3) Obtener datos de IS
         *  3.1) Crear instancia POST /cubes/{cubeId}/instances
         *  3.2) Obtener datos del cubo GET /cubes/{cubeId}/instances/{instanceId}
         * 4) Cerrar sesion
         * 
         */
        public class RequestObject
        {
            public string username { get; set; }
            public string password { get; set; }
            public int loginMode { get; set; }
            public int maxSearch { get; set; }
            public int workingSet { get; set; }
            public string changePassword { get; set; }
            public string newPassword { get; set; }
            public string metadataLocale { get; set; }
            public string warehouseDataLocale { get; set; }
            public string displayLocale { get; set; }
            public string messagesLocale { get; set; }
            public string numberLocale { get; set; }
            public string timeZone { get; set; }
            public int applicationType { get; set; }


        }

        public static void InitializeClient()
        {
            Cliente = new RestClient("http://MISERVIDORDEMICRO/MicroStrategyLibrary")
            {
                CookieContainer = new CookieContainer()
            };
            var request = new RestRequest("api/auth/login", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            var miobjeto = new RequestObject
            {
                username = "USERNAME",
                password = "PASSWORD",
                loginMode = 1,
                maxSearch = 3,
                workingSet = 10,
                changePassword = "false",
                newPassword = "string",
                metadataLocale = "en_us",
                warehouseDataLocale = "en_us",
                displayLocale = "en_us",
                messagesLocale = "en_us",
                numberLocale = "en_us",
                timeZone = "UTC",
                applicationType = 35,
            };

            request.AddJsonBody(miobjeto);
            
            IRestResponse response = Cliente.Execute(request);
             
            string token = (string)response.Headers
                .Where(x => x.Name == "X-MSTR-AuthToken")
                .Select(x => x.Value)
                .FirstOrDefault();
            AuthToken = token;

        }

        public static void CreateInstance()
        {
            var client = Cliente;
            var request = new RestRequest("/api/cubes/" + CubeID + "/instances", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("accept", "application/json");
            request.AddHeader("fields", "id,name");
            request.AddHeader("X-MSTR-AuthToken", AuthToken);
            request.AddHeader("X-MSTR-ProjectID", ProjectID);
            request.AddHeader("cubeId", CubeID);
            request.AddHeader("limit", "1");
            IRestResponse response = client.Execute(request);
            
            var jsonInstance = JObject.Parse(response.Content);
            InstanceID = (string)jsonInstance["instanceId"];
        }
        public static string GetInstance()
        {
            var client = Cliente;
            var request = new RestRequest("/api/v2/cubes/" + CubeID + "/instances/" + InstanceID, Method.GET) { RequestFormat = DataFormat.Json };
            request.AddHeader("accept", "application/json");
            request.AddHeader("fields", "id,name");
            request.AddHeader("X-MSTR-AuthToken", AuthToken);
            request.AddHeader("X-MSTR-ProjectID", ProjectID);
            request.AddHeader("cubeId", CubeID);
            request.AddHeader("instanceId", InstanceID);
            request.AddHeader("offset", "0");
            request.AddHeader("limit", "1");
            IRestResponse response = client.Execute(request);
            // CARGAR EL OBJETO CUBO
            var jsonContent = JObject.Parse(response.Content);
            //Console.WriteLine(response.Content);
            //LoadCube(jsonContent);
            // !-- FIN CARGA DEL OBJETO CUBO
            return response.Content;
        }

        // Cargo el cubo para poder buscar dentro del objeto
        public static void LoadCube(JObject data)
        {
            string aux = (string)data["definition"]["grid"]["metricsPosition"]["axis"];
            Console.WriteLine("Definition.Grid.MetricPosition.axis " + aux);
            
            Cube ejemplo = data.ToObject<Cube>();
            Console.WriteLine("Nombre: " + ejemplo.Name);
            Console.WriteLine("Id: " + ejemplo.Id);
            Console.WriteLine("InstanceId: " + ejemplo.InstanceId);
            Console.WriteLine("Status: " + ejemplo.Status);
            Console.WriteLine("Definition.Grid.CrossTab: " + ejemplo.Definition.Grid.CrossTab);
            Console.WriteLine("Definition.Grid.MetricPosition.Index: " + ejemplo.Definition.Grid.MetricPosition.Axis);

        }

        public static void TerminateClient()
        {
            var request = new RestRequest("api/auth/logout", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("accept", "application/json");
            request.AddHeader("X-MSTR-AuthToken", AuthToken);
            IRestResponse response = Cliente.Execute(request);
        }

    }
        
}
