// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EchoBot.Microstrategy;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;


namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        //JSON devuelto por la API de Microstrategy
        private static string data;

        // Indica si es la primera interaccion con el usuario
        private static bool initFlag = false;
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var userSelection = $"{ turnContext.Activity.Text }";
            var replyText = $"Su selección fue: " + userSelection + "\r\n";
            try
            {
                
                await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                bool parseFlag = int.TryParse(userSelection, out int selection);
                /*
                 * Falta validar que la opcion seleccionada por el usuario exista realmente.
                 */
                if ( parseFlag == true)
                {
                    
                    if (initFlag == true)
                    {
                        BotMenu.GetCubeSelection(selection);
                        ApiHelper.CreateInstance();
                        data = ApiHelper.GetInstance();
                        replyText = BotMenu.ShowInstanceOutput(data);
                        initFlag = false;
                    }
                    else
                    {
                        if (selection == 0)
                        {
                            initFlag = true;
                            replyText = BotMenu.ShowWelcomeMenu();
                        } else
                        {
                            replyText = BotMenu.ShowInstanceData(selection, data);
                        }
                        
                    }
                   
                } else
                {
                    replyText = "No entendí su respuesta. Intente nuevamente.\r\n";
                }
                
            }
            catch(Exception ex)
            {
                replyText = "Ha ocurrido un error. Intente nuevamente.";
            }
            // Enviar al usuario el mensaje contenido en la variable replyText
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = BotMenu.ShowWelcomeMenu();

            // Conexion con la API de Microstrategy
            ApiHelper.InitializeClient();

            //initFlag Indica que es la primera interaccion con el usuario
            initFlag = true;
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
