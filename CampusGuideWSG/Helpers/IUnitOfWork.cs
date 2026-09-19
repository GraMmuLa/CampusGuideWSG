namespace CampusGuideWSG.Helpers
{
    public interface IUnitOfWork
    {
        public void Execute(Action action);
        public T ExecuteWithResult<T>(Func<T> func);
    }
}
