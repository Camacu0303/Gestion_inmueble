using Microsoft.AspNetCore.Mvc;
using API_WEB_CLIENT.Models;
using System.Threading.Tasks;
using API_WEB_CLIENT.Services;

public class UsuarioController : Controller
{
    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _usuarioService.GetUsuariosAsync();
        return View(usuarios);  
    }


    public IActionResult Create()
    {
        return View("Create");

    }


    [HttpPost]
    public async Task<IActionResult> Create(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            await _usuarioService.AddUsuarioAsync(usuario);
            return RedirectToAction("Usuario/Index");
        }
        else
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
        }
        return View(usuario);
    }



    public async Task<IActionResult> Edit(int id)
    {
        var usuario = await _usuarioService.GetUsuarioAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return View("Edit", usuario); 
    }


    [HttpPost]
    public async Task<IActionResult> Edit(int id, Usuario usuario)
    {
        if (id != usuario.id_Usuario)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            await _usuarioService.UpdateUsuarioAsync(id, usuario);
            return RedirectToAction(nameof(Index));
        }
        return View("Usuario/Edit", usuario); 
    }

    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _usuarioService.GetUsuarioAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return View("Delete", usuario);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _usuarioService.DeleteUsuarioAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

