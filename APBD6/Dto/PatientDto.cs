using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APBD6.Dto
{
    public class PatientDto
    {
        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LasttName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
