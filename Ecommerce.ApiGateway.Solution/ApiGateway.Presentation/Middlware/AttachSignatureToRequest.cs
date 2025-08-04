namespace ApiGateway.Presentation.Middlware
{
    public class AttachSignatureToRequest(RequestDelegate request)
    {
     public  async Task InvokeAsync(HttpContext context)
        {
            context.Request.Headers["Api-Gateway"] = "Signed"; 
            await request(context);
        }
    }
}
