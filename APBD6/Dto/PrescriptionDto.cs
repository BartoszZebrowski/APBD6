﻿namespace APBD6.Dto
{
    public class PrescriptionDto
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public DoctorDto Doctor { get; set; }
        public List<MedicamentDto> Medicaments { get; set; }
    }
}
