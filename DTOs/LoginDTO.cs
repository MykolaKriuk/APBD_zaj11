using System.ComponentModel.DataAnnotations;

namespace APBD_zaj11.DTOs;

public class LoginDTO
{
    [MaxLength(100)]
    public string Login { get; set; }
    
    public string Password { get; set; }
}