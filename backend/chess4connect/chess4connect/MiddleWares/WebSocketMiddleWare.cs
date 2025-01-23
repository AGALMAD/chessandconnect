

namespace chess4connect.MiddleWares;

public class WebSocketMiddleWare : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //Obtiene el jwt de la ruta
        string jwt = context.Request.Query["jwt"].ToString();

        Console.WriteLine(jwt);

        if (string.IsNullOrEmpty(jwt))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }


        //Lo introduce en el header para que el Websocket pueda leerlo 
        context.Request.Headers["Authorization"] = $"Bearer {jwt}";

        await next(context);


    }
}

