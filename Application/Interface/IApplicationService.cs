namespace Application.Interface
{
    public interface IApplicationService<Model,T,M> where Model : class
    {
        public Task<Model> CreateAsync(T model);
        public Task<Model> UpdateAsync(string identifier, M model);
        public Task DeleteAsync(string identifier);
        public Task<List<Model>> GetAllAsync();
        public Task <Model> GetById(string identifier);
    }
}
