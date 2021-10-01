using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NETCore.Encrypt;
using QRCoder;
using StudyBuddy.Model;
using StudyBuddy.Persistence;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StudyBuddy.BusinessLogic
{
    public class ChallengeService
    {
        private IRepository repository;
        private User current_user;

        public ChallengeService(IRepository repository, User current_user)
        {
            this.repository = repository;
            this.current_user = current_user;
        }

        public IEnumerable<Challenge> All()
        {
            if (current_user == null || current_user.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (current_user.IsAdmin)
                return repository.Challenges.All();
            
            if (current_user.IsInstructor)
                return repository.Challenges.OfOwner(current_user.ID);

            return null;
        }

        public IEnumerable<Challenge> ForToday(string tag_string)
        {
            if (current_user == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return repository.Challenges.ForToday(DateTime.Now.Date);
        }

        public Challenge GetById(int id)
        {
            if (current_user == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return repository.Challenges.ById(id);
        }

        public IEnumerable<Challenge> GetByText(string text)
        {
            if (current_user == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (current_user == null || !current_user.IsAdmin)
                return repository.Challenges.OfOwnerByText(current_user.ID, text);
            else
                return repository.Challenges.ByText(text);
        }

        public Challenge Update(Challenge obj)
        {
            if (current_user == null || current_user.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (obj == null)
                throw new Exception("Object invalid!");

            if (!current_user.IsAdmin && obj.OwnerID != current_user.ID)
                throw new Exception("Unauthorized");

            repository.Challenges.Update(obj);
            return obj;
        }

        public IEnumerable<Challenge> OfBadge(int id)
        {
            if (current_user == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return repository.Challenges.OfBadge(id);
        }

        public IEnumerable<Challenge> NotOfBadge(int id)
        {
            if (current_user == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return repository.Challenges.NotOfBadge(id);
        }

        public Challenge Insert(Challenge obj)
        {
            if (current_user == null || current_user.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            repository.Challenges.Insert(obj);
            return obj;
        }

        public void Delete(int id)
        {
            if (current_user == null || current_user.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            var obj = repository.Challenges.ById(id);
            if (!current_user.IsAdmin && obj != null && obj.OwnerID != current_user.ID)
                throw new Exception("Unauthorized");

            repository.Challenges.Delete(id);
        }

        public void CreateSeries(CreateSeriesParameter param)
        {
            if (current_user == null || current_user.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            var parent = repository.Challenges.ById(param.ChallengeId);
            if (parent == null)
                throw new Exception("Object not found!");

            if (!current_user.IsAdmin && parent != null && parent.OwnerID != current_user.ID)
                throw new Exception("Unauthorized");

            for (int i = 0; i < param.Count; i++)
            {
                var clone = parent.Clone();
                clone.SeriesParentID = parent.ID;
                clone.ValidityStart = clone.ValidityStart.AddDays((i + 1) * param.DaysAdd);
                clone.ValidityEnd = clone.ValidityEnd.AddDays((i + 1) * param.DaysAdd);
                repository.Challenges.Insert(clone);
            }
        }

        public Bitmap GetQrCode(int challenge_id)
        {
            if (current_user == null || current_user.IsStudent)
                throw new UnauthorizedAccessException("Unauthorized!");

            var challenge = repository.Challenges.ById(challenge_id);
            if (challenge == null)
                throw new Exception("Challenge not found!");

            if (!current_user.IsAdmin && current_user.ID != challenge.ID)
                throw new UnauthorizedAccessException("Unauthorized!");

            // Encrypt the challenge_id
            var key_string = Model.Environment.GetOrDefault("JWT_KEY", "thisisasupersecretkey");
            key_string = string.Concat(Enumerable.Repeat(key_string, 32)).Substring(0, 32);
            var encrypted = EncryptProvider.AESEncrypt(challenge_id.ToString(), key_string);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encrypted);

            // create payload
            var payload = "qr:" + Convert.ToBase64String(plainTextBytes);

            // Generate QR-Code
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        public void AcceptFromQrCode(string payload)
        {
            if (current_user == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (!payload.StartsWith("qr:"))
                throw new UnauthorizedAccessException("No valid payload given!");

            int challenge_id = 0;

            try
            {
                // Decrypt the challenge_id
                var plainTextBytes = Convert.FromBase64String(payload.Substring(3));
                var encrypted_string = System.Text.Encoding.UTF8.GetString(plainTextBytes);
                
                var key_string = Model.Environment.GetOrDefault("JWT_KEY", "thisisasupersecretkey");
                key_string = string.Concat(Enumerable.Repeat(key_string, 32)).Substring(0, 32);
                
                var decrypted = EncryptProvider.AESDecrypt(encrypted_string, key_string);
                challenge_id = Convert.ToInt32(decrypted);
            }
            catch
            {
                throw new Exception("No valid payload given!");
            }

            repository.Challenges.AddAcceptance(challenge_id, current_user.ID);

            // ToDo: Neuigkeit erzeugen
        }
    }
}