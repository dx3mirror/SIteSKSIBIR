using WebApplication1.Models;

namespace WebApplication1.Factory
{
    public interface IOtpuskViewModelFactory
    {
        Task<OtpuskViewModel> CreateOtpuskViewModelAsync(int sotrudnikId);
    }
}
