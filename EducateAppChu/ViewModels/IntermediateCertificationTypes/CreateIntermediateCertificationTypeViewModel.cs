using System.ComponentModel.DataAnnotations;

namespace EducateAppChu.ViewModels.IntermediateCertificationTypes
{
    public class CreateIntermediateCertificationTypeViewModel
    {
        [Required(ErrorMessage = "Введите название вида промежуточной аттестации")]
        [Display(Name = "Вид промежуточной аттестации")]
        public string InterCertType { get; set; }
    }
}
