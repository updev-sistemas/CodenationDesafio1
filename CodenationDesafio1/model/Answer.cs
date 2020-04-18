using Newtonsoft.Json;

namespace CodenationDesafio1.model
{
    public class Answer
    {
        public virtual int Displacement { get; set; }
        public virtual string Token { get; set; }
        public virtual string EncryptMessage { get; set; }
        public virtual string DecryptMessage { get; set; }
        public virtual string Hash { get; set; }

        public override string ToString()
        {
            var obj = new
            {
                numero_casas = Displacement,
                token = Token,
                cifrado = EncryptMessage,
                decifrado = DecryptMessage,
                resumo_criptografico = Hash
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
