using CodenationDesafio1.consumer;
using CodenationDesafio1.json;
using CodenationDesafio1.logic;
using CodenationDesafio1.model;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CodenationDesafio1
{
    public static class Program
    {
        private static void Save(ref AnswerConsumer consumer)
        {
            try
            {
                System.IO.File.WriteAllText(@"C:\output\answer.json", consumer.Answer.ToString());
            }
            catch(Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
        }

        public static void Main(string[] args)
        {
            try
            {
                AnswerConsumer consumer = new AnswerConsumer();

               
                using(var cc = new CesarCipher())
                {
                    cc.Trigger(ref consumer);
                    cc.HashSHA1(ref consumer);
                }

                Save(ref consumer);
            
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
