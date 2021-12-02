using EducateAppChu.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducateAppChu.ViewModels.Disciplines
{
    public class IndexDisciplineViewModel
    {
        public IEnumerable<Discipline> Disciplines { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterDisciplineViewModel FilterDisciplineViewModel { get; set; }
        public SortDisciplineViewModel SortDisciplineViewModel { get; set; }
    }
}
