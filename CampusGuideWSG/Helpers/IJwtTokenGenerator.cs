using CampusGuideWSG.DTO;

namespace CampusGuideWSG.Helpers
{
    public interface IJwtTokenGenerator
    {
        public (string Token, DateTime ExpiresAt) CreateToken(ModeratorDto user, IEnumerable<string> roles);
    }
}
