namespace Shared.AuthenticationModels
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public string Issure { get; set; }
        public string Audience { get; set; }
        public double DurationInDays { get; set; }
    }
}
