namespace CampusGuideWSG.DTO
{
    public class AuthenticationResponse
    {
        public string JwtToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public ModeratorDto Value { get; set; } = null!;
    }
}
