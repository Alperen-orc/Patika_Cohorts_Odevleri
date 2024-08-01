using Patika_Hafta1_Odev.Handler;

namespace Patika_Hafta1_Odev.Extension
{
    public static class MiddleWareExtensions
    {
        //Middleware bileşenlerin extension sınıfı
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandler>();
        }
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
