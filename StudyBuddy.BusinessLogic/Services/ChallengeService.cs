using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Api.Gax;
using NETCore.Encrypt;
using SkiaSharp;
using SkiaSharp.QrCode;
using SkiaSharp.QrCode.Image;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    class ChallengeService : IChallengeService
    {
        private readonly IBackend backend;
        public event ChallengeCompletedEventHandler ChallengeCompleted;

        public ChallengeService(IBackend backend)
        {
            this.backend = backend;
        }

        public ChallengeList All(ChallengeFilter filter)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (filter == null)
                filter = new ChallengeFilter();
             
            if (!backend.CurrentUser.IsAdmin || filter.CurrentUserId == 0)
                filter.CurrentUserId = backend.CurrentUser.ID;

            if (!backend.CurrentUser.IsAdmin) 
                filter.includeSystemProve = false;

            var count = backend.Repository.Challenges.GetCount(filter);
            var objects = backend.Repository.Challenges.All(filter);

            if (filter.WithOwner)
            {
                foreach (var obj in objects)
                {
                    obj.Owner = backend.Repository.Users.ById(obj.OwnerID);
                }
            }

            return new ChallengeList() { Count = count, Objects = objects };
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
            foreach (var tag in backend.TagService.CreateOrFindMultiple(obj.Tags).Objects)
                backend.Repository.Tags.AddTagForChallenge(tag.ID, obj.ID);
            
            return obj;
        }

        public ChallengeList GetChallengesOfBadge(int id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            var objects = backend.Repository.Challenges.GetChallengesOfBadge(id);
            return new ChallengeList()
            {
                Count = objects.Count(),
                Objects = objects
            };  
        }

        public Challenge Insert(Challenge obj)
        {
            if (obj == null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            backend.Repository.Challenges.Insert(obj);

            foreach (var tag in backend.TagService.CreateOrFindMultiple(obj.Tags).Objects)
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

        public MemoryStream GetQrCode(int challenge_id)
        {
            if (backend.CurrentUser == null || backend.CurrentUser.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            var challenge = backend.Repository.Challenges.ById(challenge_id);
            if (challenge == null)
                throw new Exception("Challenge not found!");

            if (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != challenge.OwnerID)
                throw new UnauthorizedAccessException("Unauthorized!");

            // Encrypt the challenge_id
            var key_string = Model.Environment.GetOrDefault("JWT_KEY", "thisisasupersecretkey");
            key_string = string.Concat(Enumerable.Repeat(key_string, 32)).Substring(0, 32);
            var encrypted = EncryptProvider.AESEncrypt(challenge_id.ToString(), key_string);

            // create payload
            var payload = "qr:" + Base64.Encode(encrypted);

            // Generate QR-Code
            var output = new MemoryStream();
            var qrCode = new QrCode(payload, new Vector2Slim(256, 256), SKEncodedImageFormat.Png);
            qrCode.GenerateImage(output);
            return output;
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

            Accept(challenge, backend.CurrentUser);
            return challenge;
        }

        public bool AcceptWithAddendum(int challenge_id, string prove_addendum)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            var challenge = backend.Repository.Challenges.ById(challenge_id);
            if (challenge == null)
                throw new Exception("Object is null!");

            if (challenge.Prove == ChallengeProve.ByLocation)
            {
                var expected_coordinates = GeoCoordinate.FromString(challenge.ProveAddendum);
                var delivered_coordinates = GeoCoordinate.FromString(prove_addendum);
                if (expected_coordinates.IsInRadius(delivered_coordinates))
                {
                    Accept(challenge, backend.CurrentUser);
                    return true;
                }
            }

            if (challenge.Prove == ChallengeProve.ByKeyword)
            {
                if (prove_addendum.ToLower().Equals(challenge.ProveAddendum.ToLower()))
                {
                    Accept(challenge, backend.CurrentUser);
                    return true;
                }
            }

            return false;
        }

        public AcceptChallengeByLocationResultDTO AcceptWithLocation(AcceptChallengeByLocationRequestDTO obj)
        {
            if (obj == null)
                throw new Exception("Object is null!");

            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            var challenge = backend.Repository.Challenges.ById(obj.ChallengeID);
            if (challenge == null)
                throw new Exception("Object is null!");

            if (challenge.Prove != ChallengeProve.ByLocation)
                throw new Exception("Prove is wrong!");

            var expected_coordinates = GeoCoordinate.FromString(challenge.ProveAddendum);
            var is_in_radius = expected_coordinates.IsInRadius(obj.UserPosition);

            if (is_in_radius)
                Accept(challenge, backend.CurrentUser);

            return new AcceptChallengeByLocationResultDTO()
            {
                Success = is_in_radius,
                UserPosition = obj.UserPosition,
                TargetPosition = expected_coordinates
            };
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

            Accept(challenge, user);
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

            Accept(challenge, backend.CurrentUser);
        }

        private void Accept(Challenge challenge, User user)
        {
            backend.Logging.LogDebug(String.Format("User {0} accepted challenge {1}.", user.ID, challenge.ID));
            backend.Repository.Challenges.AddAcceptance(challenge.ID, user.ID);
            RaiseChallengeCompletedEvent(challenge, user);
        }

        private void RaiseChallengeCompletedEvent(Challenge c, User u)
        {
            if (ChallengeCompleted is not null)
                ChallengeCompleted(this, new ChallengeCompletedEventArgs(c, u));
        }
    }
}