using App_Chat_Common.Data.Dto;

namespace App_Chat_Common.Responses
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
