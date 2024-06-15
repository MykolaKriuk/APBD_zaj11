using System.ComponentModel.DataAnnotations;

namespace APBD_zaj11.DTOs;

public class RegisterDTO
{
    [MaxLength(100)]
    public string Login { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }
    
    public string Password { get; set; }
}