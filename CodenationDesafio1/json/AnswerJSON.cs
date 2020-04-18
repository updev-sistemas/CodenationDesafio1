namespace CodenationDesafio1.json
{
    using System;
    using Newtonsoft.Json;

    public class AnswerJSON
    {
        [JsonProperty("numero_casas")]
        public virtual int Displacement { get; set; }

        [JsonProperty("token")]
        public virtual string Token { get; set; }

        [JsonProperty("cifrado")]
        public virtual string EncryptMessage { get; set; }

        [JsonProperty("decifrado")]
        public virtual string DecryptMessage { get; set; }

        [JsonProperty("resumo_criptografico")]
        public virtual string Hash { get; set; }
    }
}
