using System.ComponentModel.DataAnnotations;

namespace F88.Digital.Application.DTOs.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}