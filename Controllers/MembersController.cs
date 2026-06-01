using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // added: enables the [Authorize] attribute
using HeirWebApp.Models;
using HeirWebApp.Data;

public class MembersController : Controller
{
    private readonly ApplicationDbContext _context;

    public MembersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: MEMBERS  (viewing the list is open to everyone)
    public async Task<IActionResult> Index()
    {
        return View(await _context.Member.ToListAsync());
    }

    // GET: MEMBERS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var member = await _context.Member
            .FirstOrDefaultAsync(m => m.id == id);
        if (member == null)
        {
            return NotFound();
        }

        return View(member);
    }

    // GET: MEMBERS/Create  (only logged-in users, ASP.NET Core Part 2 slide 89)
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    // POST: MEMBERS/Create
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("id,name,role,contact,description,parentId")] Member member)
    {
        if (ModelState.IsValid)
        {
            _context.Add(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(member);
    }

    // GET: MEMBERS/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var member = await _context.Member.FindAsync(id);
        if (member == null)
        {
            return NotFound();
        }
        return View(member);
    }

    // POST: MEMBERS/Edit/5
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("id,name,role,contact,description,parentId")] Member member)
    {
        if (id != member.id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(member);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(member.id))
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
        return View(member);
    }

    // GET: MEMBERS/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var member = await _context.Member
            .FirstOrDefaultAsync(m => m.id == id);
        if (member == null)
        {
            return NotFound();
        }

        return View(member);
    }

    // POST: MEMBERS/Delete/5
    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var member = await _context.Member.FindAsync(id);
        if (member != null)
        {
            _context.Member.Remove(member);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MemberExists(int? id)
    {
        return _context.Member.Any(e => e.id == id);
    }

    // GET: MEMBERS/SearchForm  (not async: it has no await, which removed the CS1998 warning)
    public IActionResult SearchForm()
    {
        return View();
    }

    // Members/ShowSearchFormResult  (searches by name OR role)
    public async Task<IActionResult> ShowSearchFormResult(string SearchMember)
    {
        if (_context.Member == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Member' is null.");
        }

        var filteredMembers = await _context.Member
            .Where(j => j.name.Contains(SearchMember) || j.role.Contains(SearchMember))
            .ToListAsync();

        return View("Index", filteredMembers);
    }
}
