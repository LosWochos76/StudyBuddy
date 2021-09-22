using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic.Authorization
{
    public class Permission
    {
        public void Check(User current_user, OperationType operation, Entity obj)
        {
            if (current_user == null)
                throw new System.Exception("Anonymous access not allowed!");

            if (current_user.IsAdmin)
                return;

            
        }
    }
}
