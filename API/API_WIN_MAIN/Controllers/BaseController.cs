using API_WIN_MAIN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_WIN_MAIN.Controllers
{
    public class BaseController<TEntity> : ControllerBase where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseController(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetById(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return NotFound();
            return entity;
        }

        [HttpPost]
        public async Task<ActionResult<TEntity>> Create(TEntity entity)
        {
                        _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(entity) }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TEntity entity)
        {
            if (id != (int)GetEntityId(entity))
            {
                return BadRequest();
            }

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _dbSet.FindAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return NotFound();

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private object GetEntityId(TEntity entity)
        {
            var idProperty = entity.GetType().GetProperties()
                .FirstOrDefault(p => p.Name.ToLower().Contains("id"));
            return idProperty?.GetValue(entity);
        }
    }
}
