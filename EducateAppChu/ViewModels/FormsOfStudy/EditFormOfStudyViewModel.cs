using System.ComponentModel.DataAnnotations;

namespace EducateAppChu.ViewModels.FormsOfStudy
{
    public class EditFormOfStudyViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите название формы обучения")]
        [Display(Name = "Форма обучения")]
        public string FormOfEdu { get; set; }

        public string IdUser { get; set; }
        public string InterCertType { get; internal set; }
    }
}
