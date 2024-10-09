using Domain.Contract;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Repository
{
    public class RepositoryLease : IRepositoryLease
    {
        private readonly Context.Context _context;
        private readonly IConfiguration _configuration;

        public RepositoryLease(IConfiguration configuration)
        {
            var options = new DbContextOptions<Context.Context>();
            _context = new Context.Context(configuration, options);
            _configuration = configuration;
        }

        public async Task<string> Add(Lease entity)
        {
            var entry = await _context.Leases.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            return entry.Entity.Identifier;
        }

        public async Task<int> Delete(string identifier)
        {
            var entity = await _context.Leases.FindAsync(identifier);
            if (entity != null)
            {
                _context.Leases.Remove(entity);
            }
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public IQueryable<Lease> GetAll()
        {
            return _context.Leases.AsNoTracking();
        }

        public async Task<Lease> GetById(string identifier)
        {
            var entity = await _context.Leases.FindAsync(identifier);
#pragma warning disable CS8603 
            return entity;
#pragma warning restore CS8603 
        }

        public async Task<int> Update(Lease entity)
        {
            var entry = _context.Leases.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result;
        }
    }
}
