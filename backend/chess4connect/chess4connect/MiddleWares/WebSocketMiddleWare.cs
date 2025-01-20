
namespace chess4connect.MiddleWares;
public class WebSocketMiddleWare : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //Obtiene el jwt de la ruta
        string jwt = context.Request.Query["jwt"].ToString();

        //Lo introduce en el header para que el Websocket pueda leerlo 
        context.Request.Headers.Append("Authorization", jwt);

        return Task.CompletedTask;
    }
}

