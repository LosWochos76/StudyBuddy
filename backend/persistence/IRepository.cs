namespace StudyBuddy.Persistence
{
    public interface IRepository
    {
        IUserRepository Users { get; }
        ITeamRepository Teams { get; }
        IStudyProgramRepository StudyPrograms { get; }
        ITermRepository Terms { get; }
    }
}