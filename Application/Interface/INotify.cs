namespace Application.Interface
{
    public interface INotify<T>
    {
        public void NotifyMessage(T message);        
    }
}
