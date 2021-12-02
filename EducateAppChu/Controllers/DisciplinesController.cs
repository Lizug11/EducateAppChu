using ClosedXML.Excel;
using EducateAppChu.Models;
using EducateAppChu.Models.Data;
using EducateAppChu.ViewModels.Disciplines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EducateAppChu.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class DisciplinesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public DisciplinesController(
            AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Disciplines

        public async Task<IActionResult> Index(string indexProfModule, string profModule, string index, string name,
            string shortName,
            int page = 1,
            DisciplineSortState sortOrder = DisciplineSortState.IndexProfModuleAsc)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            int pageSize = 15;

            //фильтрация
            IQueryable<Discipline> disciplines = _context.Disciplines;
                //.Include(s => s.FormOfStudy)                    // связываем специальности с формами обучения
                //.Where(w => w.FormOfStudy.IdUser == user.Id);    // в формах обучения есть поле с внешним ключом пользователя


            if (!String.IsNullOrEmpty(indexProfModule))
            {
                disciplines = disciplines.Where(p => p.IndexProfModule.Contains(indexProfModule));
            }
            if (!String.IsNullOrEmpty(profModule))
            {
                disciplines = disciplines.Where(p => p.ProfModule.Contains(profModule));
            }
            if (!String.IsNullOrEmpty(index))
            {
                disciplines = disciplines.Where(p => p.Index.Contains(index));
            }
            if (!String.IsNullOrEmpty(name))
            {
                disciplines = disciplines.Where(p => p.Name.Contains(name));
            }
            if (!String.IsNullOrEmpty(shortName))
            {
                disciplines = disciplines.Where(p => p.ShortName.Contains(shortName));
            }


            // сортировка
            switch (sortOrder)
            {
                case DisciplineSortState.IndexProfModuleDesc:
                    disciplines = disciplines.OrderByDescending(s => s.IndexProfModule);
                    break;

                case DisciplineSortState.ProfModuleAsc:
                    disciplines = disciplines.OrderBy(s => s.ProfModule);
                    break;
                case DisciplineSortState.ProfModuleDesc:
                    disciplines = disciplines.OrderByDescending(s => s.ProfModule);
                    break;

                case DisciplineSortState.IndexAsc:
                    disciplines = disciplines.OrderBy(s => s.Name);
                    break;
                case DisciplineSortState.IndexDesc:
                    disciplines = disciplines.OrderByDescending(s => s.Name);
                    break;

                case DisciplineSortState.NameAsc:
                    disciplines = disciplines.OrderBy(s => s.Name);
                    break;
                case DisciplineSortState.NameDesc:
                    disciplines = disciplines.OrderByDescending(s => s.Name);
                    break;

                case DisciplineSortState.ShortNameAsc:
                    disciplines = disciplines.OrderBy(s => s.ShortName);
                    break;
                case DisciplineSortState.ShortNameDesc:
                    disciplines = disciplines.OrderByDescending(s => s.ShortName);
                    break;

                default:
                    disciplines = disciplines.OrderBy(s => s.IndexProfModule);
                    break;
            }

            // пагинация
            var count = await disciplines.CountAsync();
            var items = await disciplines.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexDisciplineViewModel viewModel = new()
            {
                PageViewModel = new(count, page, pageSize),
                SortDisciplineViewModel = new(sortOrder),
                FilterDisciplineViewModel = new(indexProfModule, profModule, index, name, shortName),
                Disciplines = items
            };
            return View(viewModel);
        }

        //public async Task<IActionResult> Index()
        //{
        //    // находим информацию о пользователе, который вошел в систему по его имени
        //    IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        //    var appCtx = _context.Disciplines
        //        .Include(d => d.User)
        //        .Where(w => w.IdUser == user.Id)
        //        .OrderBy(o => o.Name);
        //    return View(await appCtx.ToListAsync());
        //}


        // GET: Disciplines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disciplines/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDisciplineViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Disciplines
                .Where(f => f.IdUser == user.Id &&
                    f.Name == model.Name).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенный вид дисциплины уже существует");
            }

            if (ModelState.IsValid)
            {
                Discipline disciplines = new()
                {
                    IndexProfModule = model.IndexProfModule,
                    ProfModule = model.ProfModule,
                    Index = model.Index,
                    Name = model.Name,
                    ShortName = model.ShortName,
                    IdUser = user.Id
                };

                _context.Add(disciplines);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Disciplines/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplines = await _context.Disciplines.FindAsync(id);
            if (disciplines == null)
            {
                return NotFound();
            }

            EditDisciplineViewModel model = new()
            {
                Id = disciplines.Id,
                IndexProfModule = disciplines.IndexProfModule,
                ProfModule = disciplines.ProfModule,
                Index = disciplines.Index,
                Name = disciplines.Name,
                ShortName = disciplines.ShortName,
                IdUser = disciplines.IdUser
            };


            return View(model);
        }

        // POST: Disciplines/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditDisciplineViewModel model)
        {
            Discipline disciplines = await _context.Disciplines.FindAsync(id);

            if (id != disciplines.Id)
            {
                return NotFound();
            }



            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Disciplines
                .Where(f => f.IdUser == user.Id &&
                    f.Name == model.Name).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введенная дисциплина уже существует");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    disciplines.IndexProfModule = model.IndexProfModule;
                    disciplines.ProfModule = model.ProfModule;
                    disciplines.Index = model.Index;
                    disciplines.Name = model.Name;
                    disciplines.ShortName = model.ShortName;
                    _context.Update(disciplines);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplinesExists(disciplines.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: Disciplines/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplines = await _context.Disciplines
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disciplines == null)
            {
                return NotFound();
            }

            return View(disciplines);
        }

        // POST: Disciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var disciplines = await _context.Disciplines.FindAsync(id);
            _context.Disciplines.Remove(disciplines);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplines = await _context.Disciplines
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disciplines == null)
            {
                return NotFound();
            }

            return PartialView(disciplines);
        }

        //// GET: Disciplines/Details/5
        //public async Task<IActionResult> Details(short? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var disciplines = await _context.Disciplines
        //        .Include(d => d.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (disciplines == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(disciplines);
        //}

        private bool DisciplinesExists(short id)
        {
            return _context.Disciplines.Any(e => e.Id == id);
        }

        public async Task<FileResult> DownloadPattern()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // выбираем из базы данных все специальности текущего пользователя
            var appCtx = _context.Specialties
                .Include(s => s.FormOfStudy)    // устанавливая связь с формами обучения
                .Include(f => f.Groups)         // и с группами
                .Where(w => w.FormOfStudy.IdUser == user.Id)
                .OrderBy(f => f.FormOfStudy.FormOfEdu)    // сортируем записи по названию формы обучения
                .ThenBy(f => f.Code);                     // а затем по коду обучения

            int i = 1;      // счетчик

            IXLRange rngBorder;     // объект для работы с диапазонами в Excel (выделение групп ячеек)

            // создание книги Excel
            using (XLWorkbook workbook = new(XLEventTracking.Disabled))
            {
                // для каждой специальности 
                foreach (Specialty specialty in appCtx)
                {
                    // добавить лист в книгу Excel
                    // с названием 3 символа формы обучения и кода специальности
                    IXLWorksheet worksheet = workbook.Worksheets
                        .Add($"{specialty.FormOfStudy.FormOfEdu.Substring(0, 3)} {specialty.Code}");

                    // в первой строке текущего листа указываем: 
                    // в ячейку A1 значение "Форма обучения"
                    worksheet.Cell("A" + i).Value = "Форма обучения";
                    // в ячейку B1 значение - название формы обучения текущей специальности
                    worksheet.Cell("B" + i).Value = specialty.FormOfStudy.FormOfEdu;
                    // увеличение счетчика на единицу
                    i++;

                    // во второй строке
                    worksheet.Cell("A" + i).Value = "Код специальности";
                    worksheet.Cell("B" + i).Value = $"'{specialty.Code}";

                    worksheet.Cell("C" + i).Value = "Название";
                    worksheet.Cell("D" + i).Value = specialty.Name;

                    // делаем отступ на одну строку и пишем в четвертой строке
                    i += 2;
                    // заголовки у столбцов
                    worksheet.Cell("A" + i).Value = "Индекс профессионального модуля";
                    worksheet.Cell("B" + i).Value = "Название профессионального модуля";
                    worksheet.Cell("C" + i).Value = "Индекс";
                    worksheet.Cell("D" + i).Value = "Название";
                    worksheet.Cell("E" + i).Value = "Краткое название";

                    // устанавливаем внешние границы для диапазона A4:F4
                    rngBorder = worksheet.Range("A4:E4");       // создание диапазона (выделения ячеек)
                    rngBorder.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;       // для диапазона задаем внешнюю границу

                    // на листе для столбцов задаем значение ширины по содержимому
                    worksheet.Columns().AdjustToContents();

                    // счетчик "обнуляем"
                    i = 1;
                }

                // создаем стрим
                using (MemoryStream stream = new())
                {
                    // помещаем в стрим созданную книгу
                    workbook.SaveAs(stream);
                    stream.Flush();

                    // возвращаем файл определенного типа
                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"disciplines_{DateTime.UtcNow.ToShortDateString()}.xlsx"     //в названии файла указываем таблицу и текущую дату
                    };
                }
            }
        }
    }
}