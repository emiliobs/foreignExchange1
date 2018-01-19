using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ForeignExchange1.Models;
using Newtonsoft.Json;

namespace ForeignExchange1.ApiService
{
    public class ApiService
    {
        public async Task<Response> GetList<T>(string urlBase, string controller)
        {
            try
            {
                //Aqui consumo los datos
                //aqui cargo la clase:
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                //var controller = "/api/rates";
               var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        return new Response()
                        {
                            IsSuccess = false,
                            Message = result,
                        };


                    }

                //if (!response.IsSuccessStatusCode)
                //{
                //    //IsRunning = false;
                //    //Result = result;
                //}

                //aqui serializo el string que vienes desde el rest:
                //var rates = JsonConvert.DeserializeObject<List<Rate>>(result);
                var list = JsonConvert.DeserializeObject<List<T>>(result);

                //aqui  convierto la list en un observablecollection:
               // Rates = new ObservableCollection<Rate>(rates);

                //IsRunning = false;
                ////Result = result;   
                //IsEnable = true;
                //Result = "Ready to Convert";

               return new Response()
                {
                    IsSuccess = true,
                    Result =list,
                };

            }
            catch (Exception ex)
            {

                //sihay error:
                //IsRunning = false;
                //Result = ex.Message;

             return new Response()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}

