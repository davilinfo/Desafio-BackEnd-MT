using Domain.Contract;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Repository
{
    public class RepositoryDeliver : IRepositoryDeliver
    {
        private readonly Context.Context _context;
        private readonly IConfiguration _configuration;

        public RepositoryDeliver(IConfiguration configuration)
        {
            var options = new DbContextOptions<Context.Context>();
            _context = new Context.Context(configuration, options);
            _configuration = configuration;
        }

        public async Task<string> Add(Deliver entity)
        {
            var entry = await _context.Delivers.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return entry.Entity.Identifier;
        }

        public async Task<int> Delete(string identifier)
        {
            var entity = await _context.Delivers.FindAsync(identifier);
            if (entity != null)
            {
                _context.Delivers.Remove(entity);
            }
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public IQueryable<Deliver> GetAll()
        {
            return _context.Delivers.AsNoTracking();
        }

        public async Task<Deliver> GetById(string identifier)
        {
            var entity = await _context.Delivers.FindAsync(identifier);
#pragma warning disable CS8603 
            return entity;
#pragma warning restore CS8603
        }

        public async Task<int> Update(Deliver entity)
        {
            var entry = _context.Delivers.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result;
        }
    }
}
