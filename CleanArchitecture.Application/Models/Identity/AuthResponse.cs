namespace CleanArchitecture.Application.Models.Identity
{
    public class AuthResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;


        // --- refresh token ---
        public string RefreshToken { get; set; } = string.Empty;
        public bool Success { get; set; } = false;
        public List<string>? Errors { get; set; } = new List<string>();
    }
}
