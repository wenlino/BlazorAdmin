using AdminSenyun.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AdminSenyun.Server.Controllers;


public class AccountController(IUser userService) : Controller
{

    [IgnoreAntiforgeryToken]
    [HttpPost]
    public async Task<IActionResult> Logins([FromBody] LoginInfo login)
    {
        string? username = login.UserName;
        string? password = login.Password;
        bool rememberMe = login.RememberMe;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return Ok(new { code = 50000, message = "账号密码不能为空" });
        }
        var user = userService.GetUserByUserName(username);


        //验证用户是否存在
        if (user is null || string.IsNullOrWhiteSpace(user.PassSalt))
        {
            return Ok(new { code = 20000, message = "用户或者密码不正确" });
        }

        //验证密码是否正确
        var isAuth = user.Password == ComputeHash(password, user.PassSalt);

        if (!isAuth)
        {
            return Ok(new { code = 20000, message = "用户或者密码不正确" });
        }

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(ClaimTypes.Name, username));
        var properties = new AuthenticationProperties();

        //记住我，有效期一个月
        if (rememberMe)
        {
            properties.IsPersistent = true;
            properties.ExpiresUtc = DateTimeOffset.Now.AddMonths(1);
        }
        else
        {
            properties.IsPersistent = false;
            properties.ExpiresUtc = DateTimeOffset.Now.AddDays(1);
        }

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), properties);

        return Ok(new { code = 200, message = "登录成功" });
    }

    private static string ComputeHash(string data, string salt)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(data + salt);
        return Convert.ToBase64String(SHA256.HashData(bytes));
    }


    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/Account/Login");
    }
}
