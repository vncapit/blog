
namespace BlogApi.Services.Interfaces
{
    public interface ICurrentUserService
    {
        public int? GetUserId();
        public string? GetUserName();
        public bool IsAuthenticated();
    }
}