namespace Application.Interface
{
    internal interface IBusinessValidation<Entity>
    {
        public void RegisterBusinessValidation(Entity entity);
        public void UpdateBusinessValidation(Entity entity);
    }
}
