using StudyBuddy.Test.Mocks;

namespace StudyBuddy.Test
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
