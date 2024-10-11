using Domain.Contract;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Repository
{
    public class RepositoryMotocycleBike : IRepositoryMotocycleBike
    {
        private readonly Context.Context _context;
        private readonly IConfiguration _configuration;        

        public RepositoryMotocycleBike(IConfiguration configuration)
        {
            _configuration = configuration;
            var options = new DbContextOptions<Context.Context>();
            _context = new Context.Context(configuration, options);
        }

        public async Task<string> Add(MotocycleBike entity)
        {
            var entry = await _context.MotocycleBikes.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return entry.Entity.Identifier;
        }

        public async Task<int> Delete(string identifier)
        {
            var entity = await _context.MotocycleBikes.FindAsync(identifier);
            if (entity != null)
            {
                _context.MotocycleBikes.Remove(entity);
            }
            var result = await _context.SaveChangesAsync();
            return result;            
        }

        public IQueryable<MotocycleBike> GetAll()
        {
            return _context.MotocycleBikes.AsNoTracking();
        }

        public async Task<MotocycleBike> GetById(string identifier)
        {
            var entity = await _context.MotocycleBikes.FindAsync(identifier);
#pragma warning disable CS8603
            return entity;
#pragma warning restore CS8603
        }

        public async Task<int> Update(MotocycleBike entity)
        {
            var entry = _context.MotocycleBikes.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result;
        }
    }
}
