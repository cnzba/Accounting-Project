using System.Threading.Tasks;

namespace WebApp.Services
{
    public interface IViewRenderService
    {
        string RenderToStringAsync(string viewName, object model);
    }
}