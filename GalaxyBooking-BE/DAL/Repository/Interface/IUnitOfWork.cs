namespace DAL.Repository.Interface
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}
