namespace Domain.Contract
{
    public interface IRepository<Entity>
    {
        public Task<string> Add(Entity entity);
        public Task<int> Update(Entity entity);
        public Task<int> Delete(string identifier); 
        public Task<Entity> GetById(string identifier);
        public IQueryable<Entity> GetAll();
    }
}
