
namespace chess4connect.MiddleWares;
public class WebSocketMiddleWare : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //Obtiene el jwt de la ruta
        string jwt = context.Request.Query["jwt"].ToString();

        if (string.IsNullOrEmpty(jwt))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }


        //Lo introduce en el header para que el Websocket pueda leerlo 
        context.Request.Headers["Authorization"] = $"Bearer {jwt}";


        return Task.CompletedTask;
    }
}

