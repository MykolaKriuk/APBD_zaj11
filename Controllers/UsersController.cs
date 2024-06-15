using System.IdentityModel.Tokens.Jwt;
using System.Text;
using APBD_zaj11.Contexts;
using APBD_zaj11.DTOs;
using APBD_zaj11.Helpers;
using APBD_zaj11.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APBD_zaj11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IConfiguration configuration, DatabaseContext context) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult RegisterUser(RegisterDTO newUserModel)
    {
        var hashedPassword = SecurityHelper.GetHashedPasswordAndSalt(newUserModel.Password);
        var newUser = new User()
        {
            UserEmail = newUserModel.Email,
            UserLogin = newUserModel.Login,
            Password = hashedPassword.Item1,
            Salt = hashedPassword.Item2,
            RefreshToken = SecurityHelper.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };
        context.Users.Add(newUser);
        context.SaveChanges();
        
        // SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
        //
        // SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //
        // JwtSecurityToken token = new JwtSecurityToken(
        //     issuer: configuration["JWT:Issuer"],
        //     audience: configuration["JWT:Audience"],
        //     expires: DateTime.Now.AddMinutes(10),
        //     signingCredentials: creds
        // );
        

        return Ok($"User {newUserModel.Login} successfully added");
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult LoginUser(LoginDTO loginUser)
    {
        var user = context.Users.FirstOrDefault(u => u.UserLogin == loginUser.Login);
        if (user is null)
        {
            return Unauthorized("Login or password is incorrect");
        }
        
        var passwordHashFromDatabase = user.Password;
        var currHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(loginUser.Password, user.Salt);
        if (passwordHashFromDatabase != currHashedPassword)
        {
            return Unauthorized("Login or password is incorrect");
        }
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
        
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = new JwtSecurityTokenHandler().WriteToken(token);
        user.RefreshTokenExp = DateTime.Now.AddDays(1);

        return Ok(new
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            user.RefreshToken
        });
    }
    
}