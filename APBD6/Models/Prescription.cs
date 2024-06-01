﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APBD6.Models
{
    public class Prescription
    {
        [Key]
        public int IdPrescription { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public int IdPatient { get; set; }
        
        [ForeignKey(nameof(IdPatient))]
        public Patient Patient { get; set; }

        public int IdDoctor { get; set; }
        
        [ForeignKey(nameof(IdDoctor))]
        public Doctor Doctor { get; set; }

        public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }


    }
}
