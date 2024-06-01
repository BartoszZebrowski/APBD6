using APBD6.Dto;
using APBD6.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD6.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly PrescriptionService _patientService;

        public PrescriptionController(PrescriptionService patientService)
        {
            _patientService = patientService;
        }

        public async Task<IActionResult> AddPrescription(AddPrescriptionDto addPrescriptionDto)
        {
            await _patientService.AddPrescription(addPrescriptionDto);
            return Ok();
        }
    }
}
