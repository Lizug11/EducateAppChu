
using EducateAppChu.Models;

using EducateAppChu.Models.Data;

using EducateAppChu.ViewModels.IntermediateCertificationTypes;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using System.Linq;

using System.Threading.Tasks;

namespace EducateAppChu.Controllers

{

    [Authorize(Roles = "admin, registeredUser")]

    public class IntermediateCertificationTypesController : Controller

    {

        private readonly AppCtx _context;

        private readonly UserManager<User> _userManager;

        public IntermediateCertificationTypesController(AppCtx context,

        UserManager<User> user)

        {

            _context = context;

            _userManager = user;

        }

        // GET: FormsOfStudy

        public async Task<IActionResult> Index()

        {

            // находим информацию о пользователе, который вошел в систему по его имени

            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // через контекст данных получаем доступ к таблице базы данных FormsOfStudy

            var appCtx = _context.IntermediateCertificationTypes

            .Include(f => f.User) // и связываем с таблицей пользователи через класс User

            .Where(f => f.IdUser == user.Id) // устанавливается условие с выбором записей форм обучения текущего пользователя по его Id

            .OrderBy(f => f.InterCertType); // сортируем все записи по имени форм обучения

            // возвращаем в представление полученный список записей

            return View(await appCtx.ToListAsync());

        }

        // GET: FormsOfStudy/Create

        public IActionResult Create()

        {

            return View();

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateIntermediateCertificationTypeViewModel model)

        {

            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.IntermediateCertificationTypes

            .Where(f => f.IdUser == user.Id &&

            f.InterCertType == model.InterCertType).FirstOrDefault() != null)

            {

                ModelState.AddModelError("", "Введенный вид промежуточной аттестации уже существует");

            }

            if (ModelState.IsValid)

            {

                IntermediateCertificationType intermediateCertificationType = new()

                {

                    InterCertType = model.InterCertType,

                    IdUser = user.Id

                };

                _context.Add(intermediateCertificationType);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            return View(model);

        }

        // GET: FormsOfStudy/Edit/5

        public async Task<IActionResult> Edit(short? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var intermediateCertificationType = await _context.IntermediateCertificationTypes.FindAsync(id);

            if (intermediateCertificationType == null)

            {

                return NotFound();

            }

            EditIntermediateCertificationTypeViewModel model = new()

            {

                Id = intermediateCertificationType.Id,

                InterCertType = intermediateCertificationType.InterCertType,

                IdUser = intermediateCertificationType.IdUser

            };

            return View(model);

        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(short id, EditIntermediateCertificationTypeViewModel model)

        {

            IntermediateCertificationType intermediateCertificationType = await _context.IntermediateCertificationTypes.FindAsync(id);

            if (id != intermediateCertificationType.Id)

            {

                return NotFound();

            }

            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.IntermediateCertificationTypes

            .Where(f => f.IdUser == user.Id &&

            f.InterCertType == model.InterCertType).FirstOrDefault() != null)

            {

                ModelState.AddModelError("", "Введенный вид промежуточной аттестации уже существует");

            }

            if (ModelState.IsValid)

            {

                try

                {

                    intermediateCertificationType.InterCertType = model.InterCertType;

                    _context.Update(intermediateCertificationType);

                    await _context.SaveChangesAsync();

                }

                catch (DbUpdateConcurrencyException)

                {

                    if (!IntermediateCertificationTypeExists(intermediateCertificationType.Id))

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

        // GET: FormsOfStudy/Delete/5

        public async Task<IActionResult> Delete(short? id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var intermediateCertificationType = await _context.IntermediateCertificationTypes

            .Include(f => f.User)

            .FirstOrDefaultAsync(m => m.Id == id);

            if (intermediateCertificationType == null)

            {

                return NotFound();

            }

            return View(intermediateCertificationType);

        }

        // POST: FormsOfStudy/Delete/5

        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(short id)

        {

            var intermediateCertificationType = await _context.IntermediateCertificationTypes.FindAsync(id);

            _context.IntermediateCertificationTypes.Remove(intermediateCertificationType);

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

            var intermediateCertificationType = await _context.IntermediateCertificationTypes

            .Include(f => f.User)

            .FirstOrDefaultAsync(m => m.Id == id);

            if (intermediateCertificationType == null)

            {

                return NotFound();

            }

            return PartialView(intermediateCertificationType);
        }

        //// GET: FormsOfStudy/Details/5

        //public async Task<IActionResult> Details(short? id)

        //{

        //    if (id == null)

        //    {

        //        return NotFound();

        //    }

        //    var intermediateCertificationType = await _context.IntermediateCertificationTypes

        //    .Include(f => f.User)

        //    .FirstOrDefaultAsync(m => m.Id == id);

        //    if (intermediateCertificationType == null)

        //    {

        //        return NotFound();

        //    }

        //    return View(intermediateCertificationType);

        //}

        private bool IntermediateCertificationTypeExists(short id)

        {

            return _context.IntermediateCertificationTypes.Any(e => e.Id == id);

        }

    }

}