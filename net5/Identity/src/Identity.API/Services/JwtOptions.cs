namespace Identity.API.Services
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string SigningKey { get; set; }
    }
}