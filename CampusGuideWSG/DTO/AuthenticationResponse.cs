namespace CampusGuideWSG.DTO
{
    public class AuthenticationResponse
    {
        public ModeratorDto Value { get; set; } = null!;
        public string JwtToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}
