using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EfTest.Models;

namespace EfTest.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString)
        {
            
            IQueryable<Student> studs =
                  from s in _context.Student
                  from g in _context.Grade
                  where s.CurrentGradeID == g.GradeID
                  select new Student
                  {
                      Grade = g,
                      ID = s.ID,
                      Name = s.Name,
                      Surname = s.Surname
                  };

            IQueryable<Grade> gr = from l in _context.Grade
                                   where l.GradeName.Contains(searchString)
                                   select l;

            IQueryable<Student> st =
                  from s in _context.Student
                  from g in gr
                  where s.CurrentGradeID == g.GradeID
                  select new Student
                  {
                      Grade = g,
                      ID = s.ID,
                      Name = s.Name,
                      Surname = s.Surname
                  };
           

            //IQueryable<Student> students = _context.Student
            //                .Join(_context.Grade, student => student.CurrentGradeID, grade => grade.GradeID,
            //                (student, grade) => new Student
            //                {
            //                    ID = student.ID,
            //                    Name = student.Name,
            //                    CurrentGradeID = grade.GradeID,
            //                    Grade = new Grade { GradeID = grade.GradeID, GradeName = grade.GradeName }
            //                });
            if (searchString != null)
            {
                ViewData["selList"] = new SelectList(await st.ToListAsync());
                return View(await st.ToListAsync());
            }
            else
            {
                return View(await studs.ToListAsync());
            }

        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public async Task<IActionResult> Create()
        {
            IQueryable<Grade> GradesQuery = from m in _context.Grade
                                            orderby m.GradeID
                                            select m;

            ViewData["selectlist"] = new SelectList(await GradesQuery.Distinct().ToListAsync(), "GradeID", "GradeName");
            
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Surname,CurrentGradeID")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.ID == id);
        }
    }
}
