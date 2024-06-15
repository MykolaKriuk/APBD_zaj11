using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_zaj11.Models;

[Table("Users")]
public class User
{
    [Key]
    [Column("Id")]
    public int UserId { get; set; }
    
    [Column("Login")]
    public string UserLogin { get; set; }
    
    [Column("Email")]
    public string UserEmail { get; set; }
    
    [Column("HashedPassword")]
    public string Password { get; set; }
    
    [Column("Salt")]
    public string Salt { get; set; }
    
    [Column("RefreshToken")]
    public string RefreshToken { get; set; }
    
    [Column("RefreshTokenExp")]
    public DateTime? RefreshTokenExp { get; set; }
}