namespace WebApplication1.Strategy
{
    public interface ISignInStrategy
    {
        Task SignInAsync(HttpContext httpContext, string username);
    }
}
