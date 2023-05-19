using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarSecurity.Entites;

namespace StarSecurity.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ClientController : Controller
    {
        private readonly StarSecurityContext _context;

        public ClientController(StarSecurityContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListClient(string name)
        {
            var res = await _context.Clients.Where(m => string.IsNullOrEmpty(name) || m.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            return View(res);
        }

        public async Task<IActionResult> DetailClient(long id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }
            var res = await _context.Clients
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        public async Task<IActionResult> AddClient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(Client client)
        {
            try
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return Redirect(nameof(ListClient));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(client);
        }

        public async Task<IActionResult> EditClient(long id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> EditClient(long id, Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
                return Redirect(nameof(ListClient));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return View(client);
        }

        public async Task<IActionResult> DeleteClient(long id)
        {
            if (_context.Clients == null)
            {
                return Problem("Client is null");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return Redirect(nameof(ListClient));
        }
    }
}
