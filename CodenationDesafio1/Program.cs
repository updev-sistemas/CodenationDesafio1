using CodenationDesafio1.consumer;
using CodenationDesafio1.json;
using CodenationDesafio1.model;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CodenationDesafio1
{
    public static class Program
    {
        private static void Save(Answer answer)
        {
            try
            {
                System.IO.File.WriteAllText(@"C:\output\answer.json", answer.ToString());
            }
            catch(Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
        }


        private static void Trigger(ref Answer answer)
        {
            try
            {
                int MIN = 65, // A
                    MAX = 90, // Z
                    localDisplacement = answer.Displacement % (MAX - MIN); // Não importa a quantidade de 
                                                                           // deslocamento, ele deve permanecer no ciclo
                StringBuilder sb = new StringBuilder();
                char[] text = answer.EncryptMessage.ToUpper().ToCharArray();

                for (int i = 0; i < answer.EncryptMessage.Length; i++) 
                {
                    int currentElem = (int) text[i];
                    if(currentElem >= MIN && currentElem <= MAX)
                    {
                        // É um caractere de A a Z
                        int value = currentElem - localDisplacement;

                        if(value < MIN)
                        {
                            value = MAX - (MIN - value) + 1;
                        }

                        sb.Append((char) value);
                    }
                    else
                    {
                        // Não é caractere de A a Z
                        sb.Append(text[i]);
                    }
                }

                answer.DecryptMessage = sb.ToString();
                answer.Hash = HashSHA1(answer.DecryptMessage);

                Console.WriteLine(answer.DecryptMessage);
            }
            catch(Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
        }

        public static string HashSHA1(string message)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                    throw new Exception("String inválida");

                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

                byte[] buffer = Encoding.Default.GetBytes(message);
                return BitConverter.ToString(sha1.ComputeHash(buffer)).Replace("-", "");
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                return string.Empty;
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                AnswerConsumer consumer = new AnswerConsumer();

                var answer = consumer.Answer;

                Trigger(ref answer);

                Save(answer);

                consumer.Answer = answer;

                consumer.Response();
            }
            catch(Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
        }
    }
}
