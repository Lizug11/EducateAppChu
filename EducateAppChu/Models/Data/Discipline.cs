using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducateAppChu.Models.Data
{
    public class Discipline
    {
        // Key - поле первичный ключ
        // DatabaseGenerated(DatabaseGeneratedOption.Identity) - поле автоинкреметное
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ИД")]
        public short Id { get; set; }

        [Display(Name = "Индекс профессионального модуля")]
        public string IndexProfModule { get; set; }

        [Display(Name = "Название профессионального модуля")]
        public string ProfModule { get; set; }

        [Display(Name = "Индекс")]
        public string Index { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Краткое название")]
        public string ShortName { get; set; }

        [Required]
        [Display(Name = "ИД пользователя")]
        public string IdUser { get; set; }

        [ForeignKey("IdUser")]
        [Display(Name = "Пользователь")]
        public User User { get; set; }
    }
}
