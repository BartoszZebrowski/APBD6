using APBD6.Dto;
using APBD6.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD6.Services
{
    public class PrescriptionService
    {
        private readonly DatabaseContext _databaseContext;

        public PrescriptionService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        internal async Task AddPrescription(AddPrescriptionDto addPrescription)
        {
            var patient = await GetPatient(addPrescription.Patient.IdPatient);

            if (patient is null)
            {
                patient = await CreatePatient(addPrescription.Patient);
            }

            await EnsureMedicamentsExist(addPrescription.Medicaments);
            await EnsureNoumberOfMedicaments(addPrescription.Medicaments);
            await EnsureDueDateIsAfterDate(addPrescription.DueDate, addPrescription.Date);


            var doctor = await GetDoctor(addPrescription.IdDoctor);
            var prescription = await AddPrescription(addPrescription, patient, doctor);
            await AddPrescriptionMedicaments(addPrescription.Medicaments, prescription);
        }

        private async Task AddPrescriptionMedicaments(IList<MedicamentDto> medicaments, Prescription prescription)
        {
            var prescriptionMedicaments = new List<PrescriptionMedicament>();
            foreach (var meciament in medicaments)
            {
                var prescriptionMedicament = new PrescriptionMedicament()
                {
                    IdMedicament = meciament.IdMedicament,
                    IdPrescription = prescription.IdPrescription,
                    Dose = meciament.Dose,
                    Details = meciament.Details,
                };

                prescriptionMedicaments.Add(prescriptionMedicament);
            }

            await _databaseContext.PrescriptionMedicaments.AddRangeAsync(prescriptionMedicaments);
        }

        private async Task<Prescription> AddPrescription(AddPrescriptionDto addPrescription, Patient patient, Doctor doctor)
        {
            var prescription = new Prescription()
            {
                Doctor = doctor,
                Date = addPrescription.Date,
                DueDate = addPrescription.DueDate,
                Patient = patient,
            };

            return (await _databaseContext.Prescriptions.AddAsync(prescription)).Entity;
        }

        private async Task<Doctor> GetDoctor(int idDoctor)
        {
            var doctor = await _databaseContext.Doctors.FirstOrDefaultAsync(d => d.IdDoctor == idDoctor);

            if (doctor is null)
                throw new ArgumentException("This doctor don't exist");

            return doctor;
        }


        private async Task EnsureDueDateIsAfterDate(DateTime dueDate, DateTime date)
        {
            if (dueDate < date)
                throw new ArgumentException("Due Date is after or equal of date");
        }

        private async Task EnsureNoumberOfMedicaments(List<MedicamentDto> medicaments)
        {
            if (medicaments.Count > 10)
                throw new ArgumentException("More that 10 midcaments!");
        }

        private async Task EnsureMedicamentsExist(List<MedicamentDto> medicaments)
        {
            var existingMedicaments = await _databaseContext.Medicaments
                .Where(m => medicaments.Select(m => m.IdMedicament).Contains(m.IdMedicament))
                .Select(m => m.IdMedicament)
                .ToListAsync();

            if (existingMedicaments.Count != medicaments.Count)
            {
                var missingIds = medicaments.Select(m => m.IdMedicament).Except(existingMedicaments).ToList();
                throw new InvalidOperationException($"The following medicament IDs do not exist: {string.Join(", ", missingIds)}");
            }
        }

        internal async Task<Patient> CreatePatient(PatientDto patient)
        {
            var newPatient = new Patient()
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LasttName,
                BirthDate = patient.BirthDate,
            };

            return (await _databaseContext.Patients.AddAsync(newPatient)).Entity;
        }

        private async Task<Patient?> GetPatient(int idPatient)
        {
            return await _databaseContext.Patients
                .FirstOrDefaultAsync(p => p.IdPatient == idPatient);
        }
    }
}
