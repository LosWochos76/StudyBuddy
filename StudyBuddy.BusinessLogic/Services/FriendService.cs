using System;
using StudyBuddy.Persistence;

namespace StudyBuddy.BusinessLogic
{
    public class FriendService
    {
        private IRepository repository;

        public FriendService(IRepository repository)
        {
            this.repository = repository;
        }
    }
}
