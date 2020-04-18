using CodenationDesafio1.json;
using CodenationDesafio1.model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CodenationDesafio1.consumer
{
    public class AnswerConsumer
    {
        private string _endpoint_generate;
        private string _endpoint_register;
        private string _token;

        public Answer Answer { get; set; }

        public AnswerConsumer()
        {
            _endpoint_generate = System.Configuration.ConfigurationSettings.AppSettings["Endpoint_Generate"];
            _endpoint_register = System.Configuration.ConfigurationSettings.AppSettings["Endpoint_Submit"];
            _token = System.Configuration.ConfigurationSettings.AppSettings["Token"];
            Request();
        }
               
        public void Request()
        {
            try
            {
                string url = $"{_endpoint_generate}?token={_token}";
                WebRequest httpWebRequest = WebRequest.Create(url);

                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.PreAuthenticate = false;

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = JsonConvert.DeserializeObject<AnswerJSON>(streamReader.ReadToEnd());

                        if (result == null)
                            throw new Exception("Nenhuma resposta obtida");

                        Answer = new Answer
                        {
                            EncryptMessage = result.EncryptMessage,
                            Displacement = result.Displacement,
                            Token = result.Token,
                            DecryptMessage = null,
                            Hash = null
                        };
                        streamReader.Close();
                    }
                }
            }
            catch (WebException a)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                HttpStatusCode? status = (a.Response != null && a.Response is HttpWebResponse) ? ((HttpWebResponse)a.Response).StatusCode : new HttpStatusCode?();
                switch (status)
                {
                    case HttpStatusCode.NotFound:
                        {
                            Console.WriteLine($"Endpoint não encontrado: {_endpoint_generate}?token={_token}");
                        }
                        break;

                    case HttpStatusCode.Unauthorized:
                        {
                            Console.WriteLine($"Endpoint {_endpoint_generate}?token={_token} solicitou autenticação");
                        }
                        break;

                    case HttpStatusCode.InternalServerError:
                        {
                            Console.WriteLine($"Ocorreu um erro no Endpoint {_endpoint_generate}?token={_token}");
                        }
                        break;

                    default:
                        {
                            Console.WriteLine("Ocorreu uma falha geral e que não entendemos qual é ela.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
        }

        public void Response()
        {

            try
            {
                HttpClient client = new HttpClient();

                MultipartFormDataContent form = new MultipartFormDataContent();
                HttpContent content = new StringContent("fileToUpload");
                form.Add(content, "fileToUpload");
                var stream = new FileStream(@"C:\output\answer.json", FileMode.Open);
                content = new StreamContent(stream);
                var fileName =  content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "answer",
                    FileName = Path.GetFileName(@"C:\output\answer.json"),
                };
                form.Add(content);
                HttpResponseMessage response = null;


                string url = $"{_endpoint_register}?token={_token}";
                var endpoint = new Uri(url);
                response = (client.PostAsync(endpoint, form)).Result;
                var text= Convert.ToString(response);

                Console.WriteLine("Atividade Concluida");
            }
            catch (WebException a)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                HttpStatusCode? status = (a.Response != null && a.Response is HttpWebResponse) ? ((HttpWebResponse)a.Response).StatusCode : new HttpStatusCode?();
                switch (status)
                {
                    case HttpStatusCode.NotFound:
                        {
                            Console.WriteLine($"Endpoint não encontrado: {_endpoint_register}?token={_token}");
                        }
                        break;

                    case HttpStatusCode.Unauthorized:
                        {
                            Console.WriteLine($"Endpoint {_endpoint_register}?token={_token} solicitou autenticação");
                        }
                        break;

                    case HttpStatusCode.InternalServerError:
                        {
                            Console.WriteLine($"Ocorreu um erro no Endpoint {_endpoint_register}?token={_token}");
                        }
                        break;

                    default:
                        {
                            Console.WriteLine("Ocorreu uma falha geral e que não entendemos qual é ela.");
                            Console.WriteLine(a.Message);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
        }

    }
}
