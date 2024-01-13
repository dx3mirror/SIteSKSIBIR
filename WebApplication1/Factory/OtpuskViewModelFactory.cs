using WebApplication1.Models;
using WebApplication1.UnitOfWork;
using WebApplication2;

namespace WebApplication1.Factory
{
    public class OtpuskViewModelFactory : IOtpuskViewModelFactory
    {
        private readonly IViewOtpuskUnitOfWork _viewOtpuskUnitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OtpuskViewModelFactory(IViewOtpuskUnitOfWork viewOtpuskUnitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _viewOtpuskUnitOfWork = viewOtpuskUnitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OtpuskViewModel> CreateOtpuskViewModelAsync(int sotrudnikId)
        {
            var otpuskList = await _viewOtpuskUnitOfWork.OtpuskRepository.GetAllAsync();

            var lastName = _httpContextAccessor.HttpContext.Session.GetString("LastName") ?? "";
            var firstName = _httpContextAccessor.HttpContext.Session.GetString("FirstName") ?? "";
            var patranomic = _httpContextAccessor.HttpContext.Session.GetString("Patranomic") ?? "";

            return new OtpuskViewModel
            {
                SotrudnikId = sotrudnikId,
                LastName = lastName,
                FirstName = firstName,
                Patranomic = patranomic,
                OtpuskList = (List<Otpusk>)otpuskList
            };
        }
    }
}
