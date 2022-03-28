namespace GameServer.Services.AppSettings
{
    public class JWT
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }
}
