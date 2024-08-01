using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Patika_Hafta1_Odev.Services;

namespace Patika_Hafta1_Odev.Handler
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        //Fake login işlemi için yazılan Attribute
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userService=context.HttpContext.RequestServices.GetService<FakeUserService>();
            var username = context.HttpContext.Request.Headers["Username"].ToString();
            var password = context.HttpContext.Request.Headers["Password"].ToString();

            if(!userService.Authenticate(username, password))
            {
                context.Result=new UnauthorizedResult();
            }
        }
    }
}
