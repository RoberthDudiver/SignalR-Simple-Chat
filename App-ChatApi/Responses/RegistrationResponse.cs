using App_ChatApi.Data.Dto;

namespace App_ChatApi.Responses
{
    public class RegistrationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ApplicationUserDto User { get; set; }
        public List<string> Errors { get; set; }
    }
}
