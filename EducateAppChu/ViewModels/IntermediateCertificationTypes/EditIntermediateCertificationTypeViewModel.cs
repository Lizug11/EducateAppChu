using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EducateAppChu.ViewModels.IntermediateCertificationTypes
{
    public class EditIntermediateCertificationTypeViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название вида промежуточной аттестации")]
        [Display(Name = "Вид промежуточной аттестации")]
        public string InterCertType { get; set; }

        public string IdUser { get; set; }
    }
}
