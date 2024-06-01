namespace APBD6.Dto
{
    public class AddPrescriptionDto
    {
        public PatientDto Patient { get; set; }
        public List<MedicamentDto> Medicaments { get; set; }
        public int IdDoctor { get; set; }   
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
    }
}
