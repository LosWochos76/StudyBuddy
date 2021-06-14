namespace StudyBuddy.Persistence
{
    public interface IRepository
    {
        IUserRepository Users { get; }
    }
}