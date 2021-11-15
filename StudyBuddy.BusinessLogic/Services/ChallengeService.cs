using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NETCore.Encrypt;
using QRCoder;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    class ChallengeService : IChallengeService
    {
        private readonly IBackend backend;

        public ChallengeService(IBackend backend)
        {
            this.backend = backend;
        }

        public IEnumerable<Challenge> All(ChallengeFilter filter)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (filter == null)
                filter = new ChallengeFilter();

            filter.CurrentUserId = backend.CurrentUser.ID;
            return backend.Repository.Challenges.All(filter);
        }

        public Challenge GetById(int id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return backend.Repository.Challenges.ById(id);
        }

        public Challenge Update(Challenge obj)
        {
            if (obj == null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && obj.OwnerID != backend.CurrentUser.ID)
                throw new Exception("Unauthorized");

            backend.Repository.Challenges.Update(obj);
            backend.Repository.Tags.RemoveAllTagsFromChallenge(obj.ID);
            foreach (var tag in backend.TagService.CreateOrFindMultiple(obj.Tags))
                backend.Repository.Tags.AddTagForChallenge(tag.ID, obj.ID);
            
            return obj;
        }

        public IEnumerable<Challenge> GetChallengesOfBadge(int id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return backend.Repository.Challenges.GetChallengesOfBadge(id);
        }

        public Challenge Insert(Challenge obj)
        {
            if (obj == null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            backend.Repository.Challenges.Insert(obj);
            foreach (var tag in backend.TagService.CreateOrFindMultiple(obj.Tags))
                backend.Repository.Tags.AddTagForChallenge(tag.ID, obj.ID);

            return obj;
        }

        public void Delete(int id)
        {
            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            var obj = backend.Repository.Challenges.ById(id);
            if (!backend.CurrentUser.IsAdmin && obj != null && obj.OwnerID != backend.CurrentUser.ID)
                throw new Exception("Unauthorized");

            backend.Repository.Tags.RemoveAllTagsFromChallenge(id);
            backend.Repository.Challenges.Delete(id);
        }

        public void CreateSeries(CreateSeriesParameter param)
        {
            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            var parent = backend.Repository.Challenges.ById(param.ChallengeId);
            if (parent == null)
                throw new Exception("Object not found!");

            if (!backend.CurrentUser.IsAdmin && parent != null && parent.OwnerID != backend.CurrentUser.ID)
                throw new Exception("Unauthorized");

            for (var i = 0; i < param.Count; i++)
            {
                var clone = parent.Clone();
                clone.SeriesParentID = parent.ID;
                clone.ValidityStart = clone.ValidityStart.AddDays((i + 1) * param.DaysAdd);
                clone.ValidityEnd = clone.ValidityEnd.AddDays((i + 1) * param.DaysAdd);
                backend.Repository.Challenges.Insert(clone);
            }
        }

        public Bitmap GetQrCode(int challenge_id)
        {
            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            var challenge = backend.Repository.Challenges.ById(challenge_id);
            if (challenge == null)
                throw new Exception("Challenge not found!");

            if (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != challenge.ID)
                throw new UnauthorizedAccessException("Unauthorized!");

            // Encrypt the challenge_id
            var key_string = Model.Environment.GetOrDefault("JWT_KEY", "thisisasupersecretkey");
            key_string = string.Concat(Enumerable.Repeat(key_string, 32)).Substring(0, 32);
            var encrypted = EncryptProvider.AESEncrypt(challenge_id.ToString(), key_string);

            // create payload
            var payload = "qr:" + Base64.Encode(encrypted);

            // Generate QR-Code
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        public Challenge AcceptFromQrCode(string payload)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (string.IsNullOrEmpty(payload) || !payload.StartsWith("qr:"))
                throw new UnauthorizedAccessException("No valid payload given!");

            var challenge_id = 0;

            try
            {
                // Decrypt the challenge_id
                var encrypted = Base64.Decode(payload.Substring(3));
                var key_string = Model.Environment.GetOrDefault("JWT_KEY", "thisisasupersecretkey");
                key_string = string.Concat(Enumerable.Repeat(key_string, 32)).Substring(0, 32);
                var decrypted = EncryptProvider.AESDecrypt(encrypted, key_string);
                challenge_id = Convert.ToInt32(decrypted);
            }
            catch
            {
                throw new Exception("No valid payload given!");
            }

            var challenge = backend.Repository.Challenges.ById(challenge_id);
            if (challenge == null)
                throw new Exception("Object is null!");

            backend.Repository.Challenges.AddAcceptance(challenge_id, backend.CurrentUser.ID);
            OnChallengeAccepted(challenge, backend.CurrentUser);
            backend.BusinessEvent.TriggerEvent(this, new BusinessEventArgs(BusinessEventType.ChallengeAccepted, challenge));

            return challenge;
        }

        public void RemoveAcceptance(int challenge_id, int user_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized");

            backend.Repository.Challenges.RemoveAcceptance(challenge_id, user_id);
        }

        public void AddAcceptance(int challenge_id, int user_id)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized");

            var challenge = backend.Repository.Challenges.ById(challenge_id);
            if (challenge == null)
                throw new Exception("Object is null!");

            var user = backend.Repository.Users.ById(user_id);
            if (user == null)
                throw new Exception("Object is null!");

            backend.Repository.Challenges.AddAcceptance(challenge_id, user_id);
            OnChallengeAccepted(challenge, user);
            backend.BusinessEvent.TriggerEvent(this, new BusinessEventArgs(BusinessEventType.ChallengeAccepted, challenge));
        }

        public IEnumerable<Challenge> GetAcceptedChallenges()
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized");
            
            return backend.Repository.Challenges.Accepted(backend.CurrentUser.ID);
        }

        public void Accept(int challenge_id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized");

            var challenge = backend.Repository.Challenges.ById(challenge_id);
            if (challenge == null)
                throw new Exception("Object not found!");

            if (challenge.Prove != ChallengeProve.ByTrust)
                throw new Exception("You need to provide a prove!");

            backend.Repository.Challenges.AddAcceptance(challenge_id, backend.CurrentUser.ID);
            OnChallengeAccepted(challenge, backend.CurrentUser);
            backend.BusinessEvent.TriggerEvent(this, new BusinessEventArgs(BusinessEventType.ChallengeAccepted, challenge));
        }

        private void OnChallengeAccepted(Challenge obj, User current_user)
        {
            var badges_of_user = backend.GameBadgeService.GetBadgesOfUser(current_user.ID);
            var badges = backend.Repository.GameBadges.GetBadgesForChallenge(obj.ID);

            foreach (var badge in badges)
            {
                if (!IsInList(badges_of_user, badge))
                {
                    var success_rate = backend.GameBadgeService.GetSuccessRate(badge.ID, current_user.ID);
                    if (success_rate.Success >= badge.RequiredCoverage)
                    {
                        backend.Repository.GameBadges.AddBadgeToUser(current_user.ID, badge.ID);
                        // ToDo: Neuigkeit erzeugen!
                    }
                }
            }
        }

        private bool IsInList(IEnumerable<GameBadge> list, GameBadge badge)
        {
            foreach (var b in list)
                if (b.ID.Equals(badge.ID))
                    return true;

            return false;
        }
    }
}