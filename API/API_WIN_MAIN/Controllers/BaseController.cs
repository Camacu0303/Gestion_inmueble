// Usamos los modelos definidos en la solución
using API_WIN_MAIN.Models;

// Utilidades y clases base para crear controladores REST
using Microsoft.AspNetCore.Mvc;

// EF Core para el manejo de entidades y consultas a la base de datos
using Microsoft.EntityFrameworkCore;

// Utilidades básicas del framework: listas, LINQ, y tareas async/await
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_WIN_MAIN.Controllers
{
    // Controlador base genérico, donde TEntity representa cualquier clase (entidad del modelo)
    public class BaseController<TEntity> : ControllerBase where TEntity : class
    {
        // Contexto de base de datos inyectado, permite acceso a entidades y operaciones de EF
        private readonly DbContext _context;

        // DbSet que representa la tabla correspondiente a TEntity
        private readonly DbSet<TEntity> _dbSet;

        // Constructor que recibe el contexto y prepara el DbSet para uso posterior
        public BaseController(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>(); // Vincula TEntity a su tabla correspondiente
        }

        protected DbContext GetContext()
        {
            return _context;
        }

        // GET: api/[controller]
        // Devuelve todas las entidades con sus propiedades de navegación incluidas automáticamente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetAll()
        {
            return await AutoIncludeNavigationProperties().ToListAsync();
        }

        // GET: api/[controller]/{id}
        // Devuelve una entidad por su ID, incluyendo también sus propiedades de navegación
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetById(int id)
        {
            var keyName = GetEntityIdName(); // Obtiene dinámicamente el nombre de la propiedad ID
            var entity = await AutoIncludeNavigationProperties()
                              .FirstOrDefaultAsync(e => EF.Property<int>(e, keyName) == id); // Busca la entidad por ID

            if (entity == null) return NotFound(); // Si no existe, devuelve 404
            return entity; // Si existe, devuelve la entidad con relaciones cargadas
        }

        // Busca la propiedad cuyo nombre contenga "id" (ej. Id, ClienteId, PropiedadId...)
        private string GetEntityIdName()
        {
            var idProperty = typeof(TEntity).GetProperties()
                .FirstOrDefault(p => p.Name.ToLower().Contains("id"));
            return idProperty?.Name;
        }

        // POST: api/[controller]
        // Crea una nueva entidad TEntity, asociando automáticamente sus relaciones si se pasa solo el ID
        [HttpPost]
        public async Task<ActionResult<TEntity>> Create(TEntity entity)
        {
            // Intenta asociar entidades relacionadas usando los IDs que llegaron en el body
            await AutoAssociateNavigationProperties(entity);

            // Marca la nueva entidad como "Agregada" en el contexto
            _context.Entry(entity).State = EntityState.Added;

            // Obtiene el tipo de entidad desde el modelo para acceder a las relaciones
            var entityType = _context.Model.FindEntityType(typeof(TEntity));

            // Recorre todas las propiedades de navegación y las marca como "Unchanged"
            foreach (var navigation in entityType.GetNavigations())
            {
                var navProperty = typeof(TEntity).GetProperty(navigation.Name);
                var relatedEntity = navProperty?.GetValue(entity);
                if (relatedEntity != null)
                {
                    MarkAsUnchangedRecursive(relatedEntity); // Para que no las intente volver a insertar o modificar
                }
            }

            // Persiste la entidad en la base de datos
            await _context.SaveChangesAsync();

            // Devuelve 201 Created junto con la ruta para obtenerla
            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(entity) }, entity);
        }

        // PUT: api/[controller]/{id}
        // Actualiza una entidad existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TEntity updatedEntity)
        {
            if (updatedEntity == null)
            {
                return BadRequest();
            }

            var existingEntity = await _dbSet.FindAsync(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            var propertiesToUpdate = entityType.GetProperties().Where(p => !p.IsShadowProperty() && p.Name != GetEntityIdName()).ToList();
            var navigationProperties = entityType.GetNavigations().ToList();

            foreach (var property in propertiesToUpdate)
            {
                var updatedValue = property.PropertyInfo?.GetValue(updatedEntity);
                var currentValue = property.PropertyInfo?.GetValue(existingEntity);

                // Only update if the updated value is not null and different from the current value
                if (updatedValue != null && !updatedValue.Equals(currentValue))
                {
                    property.PropertyInfo?.SetValue(existingEntity, updatedValue);
                }
            }

            // Handle foreign key properties explicitly if needed
            foreach (var navigation in navigationProperties)
            {
                if (navigation.ForeignKey != null) // Check if the navigation has a foreign key
                {
                    var fkPropertyName = navigation.ForeignKey.Properties.Single().Name;
                    var updatedFkValue = updatedEntity.GetType().GetProperty(fkPropertyName)?.GetValue(updatedEntity);
                    var currentFkValue = existingEntity.GetType().GetProperty(fkPropertyName)?.GetValue(existingEntity);

                    if (updatedFkValue != null && !updatedFkValue.Equals(currentFkValue))
                    {
                        var relatedEntityType = navigation.TargetEntityType.ClrType;
                        var relatedEntity = await _context.FindAsync(relatedEntityType, updatedFkValue);
                        if (relatedEntity != null)
                        {
                            navigation.PropertyInfo?.SetValue(existingEntity, relatedEntity);
                        }
                        // Optionally handle cases where the related entity is not found
                    }
                    else if (updatedFkValue == null)
                    {
                        navigation.PropertyInfo?.SetValue(existingEntity, null); // Allow setting FK to null if the relationship is nullable
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        private bool EntityExists(int id)
        {
            var keyName = GetEntityIdName();
            return _dbSet.Any(e => EF.Property<int>(e, keyName) == id);
        }


        // DELETE: api/[controller]/{id}
        // Elimina una entidad por su ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id); // Busca la entidad
            if (entity == null) return NotFound();   // Si no existe, 404

            _dbSet.Remove(entity); // Elimina la entidad del DbSet
            await _context.SaveChangesAsync(); // Guarda los cambios

            return NoContent(); // 204 OK sin contenido
        }

        // Asocia automáticamente propiedades de navegación usando su clave foránea (si se pasó el ID)
        private async Task AutoAssociateNavigationProperties(TEntity entity)
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            var navigationProperties = entityType.GetNavigations();

            foreach (var navigation in navigationProperties)
            {
                var navProperty = typeof(TEntity).GetProperty(navigation.Name); // Ej: Cliente, Usuario
                var fkPropertyName = navigation.ForeignKey.Properties.First().Name; // Ej: ClienteId

                // Si la propiedad de navegación es nula pero sí hay valor en la FK, lo usa para cargar la entidad
                if (navProperty != null && navProperty.GetValue(entity) == null)
                {
                    var fkValue = typeof(TEntity).GetProperty(fkPropertyName)?.GetValue(entity);
                    if (fkValue != null && Convert.ToInt32(fkValue) != 0)
                    {
                        var relatedEntityType = navigation.TargetEntityType.ClrType; // Tipo de la entidad relacionada
                        var relatedEntity = await _context.FindAsync(relatedEntityType, fkValue); // Carga la entidad por ID
                        navProperty.SetValue(entity, relatedEntity); // Asocia la entidad cargada a la propiedad de navegación
                    }
                }
            }
        }

        // Incluye automáticamente todas las relaciones (propiedades de navegación) de la entidad TEntity
        private IQueryable<TEntity> AutoIncludeNavigationProperties()
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity));
            var query = _dbSet.AsQueryable(); // Empezamos desde el DbSet

            // Agrega un Include para cada propiedad de navegación (relación)
            foreach (var navigation in entityType.GetNavigations())
            {
                query = query.Include(navigation.Name);
            }

            return query; // Devuelve la query con relaciones incluidas
        }

        // Marca recursivamente las entidades relacionadas como "Unchanged" para evitar que EntityFramework intente insertarlas o modificarlas
        private void MarkAsUnchangedRecursive(object entity)
        {
            if (entity == null) return;

            var entry = _context.Entry(entity);

            // Si está desconectada del contexto, se marca como no modificada
            if (entry.State == EntityState.Detached)
                entry.State = EntityState.Unchanged;

            // Recorre relaciones de navegación que NO son colecciones
            var navs = entry.Navigations.Where(n => !n.Metadata.IsCollection).ToList();

            foreach (var nav in navs)
            {
                var navValue = nav.CurrentValue;
                if (navValue != null)
                {
                    MarkAsUnchangedRecursive(navValue); // Marca también las entidades relacionadas
                }
            }
        }

        // Obtiene el valor del ID de la entidad (usado para CreatedAtAction o comparación en PUT)
        private object GetEntityId(TEntity entity)
        {
            var idProperty = entity.GetType().GetProperties()
                .FirstOrDefault(p => p.Name.ToLower().Contains("id"));
            return idProperty?.GetValue(entity);
        }

    }
}
