using System.ComponentModel.DataAnnotations;

namespace EducateAppChu.ViewModels.FormsOfStudy
{
    public class CreateFormOfStudyViewModel
    {
        [Required(ErrorMessage = "Введите название вида промежуточной аттестации")]
        [Display(Name = "Вид промежуточной аттестации")]
        public string FormOfEdu { get; set; }
    }
}
