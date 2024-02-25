namespace PrescriberDocAPI.Shared.Domain
{
    public class UserConfig
    {
        public string IssuerSigningKey { get; set; } = string.Empty;
        public string ConnectionStrinfg { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;

    }
}
