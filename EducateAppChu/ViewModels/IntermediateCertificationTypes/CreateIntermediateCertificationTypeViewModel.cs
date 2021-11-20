using System.ComponentModel.DataAnnotations;

namespace EducateAppChu.ViewModels.IntermediateCertificationTypes
{
    public class CreateIntermediateCertificationTypeViewModel
    {
        [Required(ErrorMessage = "Введите название формы обучения")]
        [Display(Name = "Форма обучения")]
        public string InterCertType { get; set; }
    }
}
