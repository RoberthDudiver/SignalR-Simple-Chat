using App_ChatApi.Data.Dto;

namespace App_ChatApi.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

    }
}
