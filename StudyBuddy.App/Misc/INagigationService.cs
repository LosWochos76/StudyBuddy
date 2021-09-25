using System.Threading.Tasks;

namespace StudyBuddy.App.Misc
{
    public interface INagigationService
    {
        Task GoTo(string path);
    }
}