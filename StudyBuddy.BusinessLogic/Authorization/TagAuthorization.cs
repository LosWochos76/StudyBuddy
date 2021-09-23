using System;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    public class TagAuthorization
    {
        public static void CheckGetAll(User current_user)
        {
            if (current_user == null || !current_user.IsAdmin)
                throw new Exception("Unauthorized!");
        }

        public static void CheckGetCount(User current_user)
        {
            CheckGetAll(current_user);
        }

        public static void CheckGetById(User current_user)
        {
            if (current_user == null)
                throw new Exception("Unauthorized!");
        }

        public static void CheckUpdate(User current_user)
        {
            CheckGetAll(current_user);
        }

        public static void CheckInsert(User current_user)
        {
        }

        public static void CheckDelete(User current_user)
        {
            CheckGetAll(current_user);
        }

        public static void CheckCreateOrFindSigle(User current_user)
        {
            CheckGetById(current_user);
        }
    }
}
