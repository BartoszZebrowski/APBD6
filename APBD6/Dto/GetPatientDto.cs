namespace APBD6.Dto
{
    public class GetPatientDto : PatientDto
    {
        public List<PrescriptionDto> Prescription { get; set; }

    }
}
