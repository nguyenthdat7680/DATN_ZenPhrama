using Microsoft.AspNetCore.Mvc;
using NhaThuocOnline.ApiIntergration;
using NhaThuocOnline.ViewModel.Prescription;

namespace NhaThuocOnline.WebApp.Controllers
{
    public class PrescriptionController : ClientBaseController
    {
        private readonly IPrescriptionApiClient _prescriptionApiClient;
        public PrescriptionController(IPrescriptionApiClient prescriptionApiClient)
        {
            _prescriptionApiClient = prescriptionApiClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PrescriptionCreateRequest request)
        {
            var result = await _prescriptionApiClient.Create(request);
            return View(result);
        }
    }
}
