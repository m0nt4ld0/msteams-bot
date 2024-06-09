using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EchoBot.Microstrategy
{
    /*
     * Interaccion con el usuario.
     * Muestra la informacion devuelta por la API en forma legible.
     */
    class BotMenu
    {
        /*
         * Mostrar mensaje de bienvenida y listado de cubos que se pueden consultar.
         */
        public static string ShowWelcomeMenu()
        {
            var text = "Bienvenido. ¿En qué puedo ayudarle?\r\n";
            text += "1 - Ver información de <Cubo 1>.\r\n";
            text += "2 - Ver información de <Cubo 2>\r\n";
            return text;
        }

        /*
         * Establecer cubos (predefinidos) a usar en base a la seleccion del usuario.
         */
        public static void GetCubeSelection(int selection)
        {
            switch (selection)
            {
                case 1:
                    ApiHelper.ProjectID = "IDPROYECTOMICRO";
                    ApiHelper.CubeID = "IDCUBOMICRO";
                    break;
                case 2:
                    ApiHelper.ProjectID = "IDPROYECTOMICRO";
                    ApiHelper.CubeID = "IDCUBOMICRO";
                    break;
            }
        }

        /*
         * Muestra menu del cubo (informacion que se puede consultar)
         */
        public static string ShowInstanceOutput(string instanceJSON)
        {
            var response = "";
            var jsonContent = JObject.Parse(instanceJSON);
            response += "El cubo seleccionado es " + jsonContent["name"] + "\r\n";
            response += "Metricas y atributos de " + jsonContent["name"] + ":\r\n";
            for (int i = 0; i < jsonContent["definition"]["grid"]["rows"].Count(); i++)
                response += (i + 1) + " - " + jsonContent["definition"]["grid"]["rows"][i]["name"] + "\r\n";
            response += "0 - Volver al menú anterior.\r\n";
            return response;
        }

        /*
         * Devolver los datos de la opcion seleccionada en el menu
         */
        public static string ShowInstanceData(int n, string data)
        {
            var response="";
            var jsonContent = JObject.Parse(data);

            for (int i = 0; i < jsonContent["definition"]["grid"]["rows"][n - 1]["elements"].Count(); i++)
                response += jsonContent["definition"]["grid"]["rows"][n-1]["elements"][i]["formValues"][0] + "\r\n";
            return response;
        }
        
        public static void ShowExitMessage()
        {
            Console.WriteLine("Hasta luego!");
        }
    }

}
