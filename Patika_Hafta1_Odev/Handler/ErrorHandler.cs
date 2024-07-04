using System.Net;
using System.Text.Json;

namespace Patika_Hafta1_Odev.Handler
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _handler;

        public ErrorHandler(RequestDelegate handler)
        {
            _handler = handler;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _handler(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new { message = error?.Message };

                switch (error)
                {
                    //400 hata kodu işlenmesi
                    case ApplicationException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    //404 hata kodu işlenmesi
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    //500 hata kodu işlenmesi
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
