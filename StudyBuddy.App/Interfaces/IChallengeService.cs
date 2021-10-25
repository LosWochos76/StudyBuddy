﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StudyBuddy.App.ViewModels;

namespace StudyBuddy.App.Api
{
    public interface IChallengeService
    {
        Task ForToday(ObservableCollection<ChallengeViewModel> list, string search_string, bool reload = false);
        Task<bool> AcceptFromQrCode(string code);
        Task<ChallengeViewModel> GetById(int challenge_id);
    }
}