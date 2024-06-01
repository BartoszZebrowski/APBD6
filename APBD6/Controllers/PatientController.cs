using APBD6.Dto;
using APBD6.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD6.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        public async Task<ActionResult<GetPatientDto>> GetPatient(int idPatient)
        {
            var patientPrescritpions = await _patientService.GetPatientPrescritpions(idPatient);
            return Ok(patientPrescritpions);
        }
    }
}
