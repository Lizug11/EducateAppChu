using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducateAppChu.ViewModels.Disciplines
{
    public class FilterDisciplineViewModel
    {
        public string SelectedIndexProfModule { get; private set; }    // введенный Индекс профессионального модуля
        public string SelectedProfModule { get; private set; }    // введенный Название профессионального модуля
        public string SelectedIndex { get; private set; }    // введенное Индекс
        public string SelectedName { get; private set; }    // введенное Название
        public string SelectedShortName { get; private set; }    // введенное Краткое название


        //public SelectList FormOfStudies { get; private set; } // список форм обучения
        //public short? FormOfEdu { get; private set; }   // выбранная форма обучения



        public FilterDisciplineViewModel(string indexProfModule, string profModule, string index, string name,
            string shortName)
        {
            SelectedIndexProfModule = indexProfModule;
            SelectedProfModule = profModule;
            SelectedIndex = index;
            SelectedName = name;
            SelectedShortName = shortName;

            //// устанавливаем начальный элемент, который позволит выбрать всех
            //formOfStudies.Insert(0, new FormOfStudy { FormOfEdu = "", Id = 0 });

            //FormOfStudies = new SelectList(formOfStudies, "Id", "FormOfEdu", formOfEdu);
            //FormOfEdu = formOfEdu;
        }
    }
}
