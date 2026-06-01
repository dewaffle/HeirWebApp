using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HeirWebApp.Models;
using HeirWebApp.Data;

public class MembersController : Controller
{
    private readonly ApplicationDbContext _context;

    public MembersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Loads the list of members used to fill the "Parent" dropdown.
    // excludeId keeps a member from being offered as its own parent (on Edit).
    private async Task PopulateParentOptions(int? excludeId = null)
    {
        ViewBag.ParentOptions = await _context.Member
            .Where(m => excludeId == null || m.id != excludeId)
            .OrderBy(m => m.name)
            .ToListAsync();
    }

    // Builds an id -> name map of ALL members, so the list can show a parent's
    // name even when the list is a filtered search result.
    private async Task PopulateNameLookup()
    {
        ViewBag.NameLookup = await _context.Member.ToDictionaryAsync(m => m.id, m => m.name);
    }

    // Looks up a parent's name for display (returns null if no parent).
    private async Task<string?> GetParentName(int? parentId)
    {
        if (parentId == null) return null;
        var parent = await _context.Member.FirstOrDefaultAsync(m => m.id == parentId);
        return parent?.name;
    }

    // GET: MEMBERS
    public async Task<IActionResult> Index()
    {
        await PopulateNameLookup();
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

        ViewBag.ParentName = await GetParentName(member.parentId);
        return View(member);
    }

    // GET: MEMBERS/Create
    [Authorize]
    public async Task<IActionResult> Create()
    {
        await PopulateParentOptions();
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
        await PopulateParentOptions();
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
        await PopulateParentOptions(member.id);
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
        await PopulateParentOptions(member.id);
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

        ViewBag.ParentName = await GetParentName(member.parentId);
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

    // GET: MEMBERS/SearchForm
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

        await PopulateNameLookup();

        var filteredMembers = await _context.Member
            .Where(j => j.name.Contains(SearchMember) || j.role.Contains(SearchMember))
            .ToListAsync();

        return View("Index", filteredMembers);
    }
}