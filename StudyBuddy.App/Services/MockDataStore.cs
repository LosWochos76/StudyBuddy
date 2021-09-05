using StudyBuddy.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyBuddy.App.Services
{
    class MockDataStore
    {
        public List<Challenge> GetMockChallenges()
        {
            var list = new List<Challenge>();

            list.Add(new Challenge(1, "TestChallenge 1", "das ist der erste Test", 15, 5, "student"));
            list.Add(new Challenge(2, "TestChallenge 2", "das ist der zweite Test", 10, 5, "student"));
            list.Add(new Challenge(3, "TestChallenge 3", "das ist der dritte Test", 25, 5, "student"));

            return list;
        }
    }
}
