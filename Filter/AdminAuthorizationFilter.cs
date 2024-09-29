using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DonationsWeb.Filter
{
    public class AdminAuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Giả sử bạn có hàm kiểm tra nếu admin đã đăng nhập
            if (!IsAdminLoggedIn(context))
            {
                // Nếu admin chưa đăng nhập, chuyển hướng về trang Login
                context.Result = new RedirectToActionResult("Login", "Users", null);
            }

            base.OnActionExecuting(context);
        }

        private static bool IsAdminLoggedIn(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            return httpContext.Session.GetString("UserId") != null && httpContext.Session.GetString("RoleName") == "Admin";
        }
    }
}
