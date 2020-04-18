
using CodenationDesafio1.consumer;
using CodenationDesafio1.model;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CodenationDesafio1.logic
{
    public class CesarCipher : IDisposable
    {
        public void Trigger(ref AnswerConsumer consumer)
        {
            try
            {
                int MIN = 97, // A
                    MAX = 122, // Z
                    localDisplacement = consumer.Answer.Displacement % (MAX - MIN); // Não importa a quantidade de 
                                                                                    // deslocamento, ele deve permanecer no ciclo
                StringBuilder sb = new StringBuilder();
                char[] text = consumer.Answer.EncryptMessage.ToLower().ToCharArray();

                for (int i = 0; i < consumer.Answer.EncryptMessage.Length; i++)
                {
                    int currentElem = (int)text[i];
                    if (currentElem >= MIN && currentElem <= MAX)
                    {
                        // É um caractere de A a Z
                        int value = currentElem - localDisplacement;

                        if (value < MIN)
                        {
                            value = MAX - (MIN - value) + 1;
                        }

                        sb.Append((char)value);
                    }
                    else
                    {
                        // Não é caractere de A a Z
                        sb.Append(text[i]);
                    }
                }

                consumer.Answer.DecryptMessage = sb.ToString().ToLower();
                Console.WriteLine(consumer.Answer.DecryptMessage);
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
            }
        }

        public void HashSHA1(ref AnswerConsumer consumer)
        {
            try
            {
                if (string.IsNullOrEmpty(consumer.Answer.DecryptMessage))
                    throw new Exception("String inválida");

                SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

                byte[] buffer = Encoding.Default.GetBytes(consumer.Answer.DecryptMessage);
                consumer.Answer.Hash = BitConverter.ToString(sha1.ComputeHash(buffer)).Replace("-", "");
            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                consumer.Answer.Hash = null;
            }
        }

        public void Dispose()
        {

        }
    }
}
