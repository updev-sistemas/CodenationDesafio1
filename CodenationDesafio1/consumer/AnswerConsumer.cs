using CodenationDesafio1.json;
using CodenationDesafio1.model;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
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
                HttpWebRequest request = null;
                Uri uri = new Uri($"{_endpoint_register}?token={_token}");
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                request.UserAgent = "NPPD";
                request.ContentType = "multipart/form-data; boundary=myboundary";

                string json = System.IO.File.ReadAllText(@"C:\output\answer.json");
                byte[] data = Encoding.Unicode.GetBytes(json);
                request.ContentLength = data.Length;


                using (Stream writeStream = request.GetRequestStream())
                {
                    writeStream.Write(data, 0, data.Length);
                }
                string result = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader readStream = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            result = readStream.ReadToEnd();
                        }
                    }
                }
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
