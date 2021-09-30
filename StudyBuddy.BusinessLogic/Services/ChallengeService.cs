using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using QRCoder;
using StudyBuddy.Model;
using StudyBuddy.Persistence;

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
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }
    }
}