
using APBD6.Dto;
using APBD6.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD6.Services
{
    public class PatientService
    {
        private readonly DatabaseContext _databaseContext;

        public PatientService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        internal async Task<GetPatientDto> GetPatientPrescritpions(int idPatient)
        {
            var patient = await GetPatient(idPatient);

            EnsurePatientExist(patient);

            var prescriptions = await GetPrescriptions(patient);

            return new GetPatientDto()
            {
                IdPatient = idPatient,
                FirstName = patient.FirstName,
                LasttName = patient.LastName,
                BirthDate = patient.BirthDate,
                Prescription = prescriptions,
            };
        }

        private async Task<List<PrescriptionDto>> GetPrescriptions(Patient patient)
        {
            var prescriptionsWithFullInfo = new List<PrescriptionDto>();


            foreach (var prescription in patient.Prescriptions)
            {
                var medicamentsWithInfo = prescription.PrescriptionMedicaments.Select(pm =>
                    new MedicamentDto()
                    {
                        IdMedicament = pm.Medicament.IdMedicament,
                        Description = pm.Medicament.Description,
                        Dose = pm.Dose,
                        Details = pm.Details
                    }).ToList();


                var prescriptionWithFullInfo = new PrescriptionDto()
                {
                    DueDate = prescription.DueDate,
                    Date = prescription.Date,
                    Doctor = new DoctorDto()
                    {
                        IdDoctor = prescription.Doctor.IdDoctor,
                        FirstName = prescription.Doctor.FirstName,
                        LastName = prescription.Doctor.LastName,
                        Email = prescription.Doctor.Email,
                    },
                    Medicaments = medicamentsWithInfo
                };
            }

            return prescriptionsWithFullInfo;
        }


        private void EnsurePatientExist(Patient? patient)
        {
            if (patient is null)
                throw new ArgumentException("This patient don't exist");
        }


        private async Task<Patient?> GetPatient(int idPatient)
        {
            return await _databaseContext.Patients.FirstOrDefaultAsync(p => p.IdPatient == idPatient);
        }
    }
}
