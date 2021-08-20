using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddy.Model
{
    public enum ChallengeProve
    {
        ByTrust = 1,
        ByQrCode = 2,
        ByRandomTeamMember = 3,
        ByLocationCloseToOwner = 4
    }
}