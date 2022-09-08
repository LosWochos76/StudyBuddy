using StudyBuddy.App.Test.Mocks;

namespace StudyBuddy.App.Test
{
    public class ViewModelBaseTest
    {
        protected ApiMock api;
        protected DialogServiceMock dialog;
        protected NavigationServiceMock navigation;

        protected void InitMocks()
        {
            api = new ApiMock();
            dialog = new DialogServiceMock();
            navigation = new NavigationServiceMock();
        }
    }
}
