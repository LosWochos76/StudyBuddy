using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.BusinessLogic
{
    class ImageService : IImageService
    {
        private readonly IBackend backend;

        public ImageService(IBackend backend)
        {
            this.backend = backend;
        }

        public IEnumerable<PersistentImage> All(ImageFilter filter)
        {
            if (backend.CurrentUser == null || !backend.CurrentUser.IsAdmin)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (filter == null)
                filter = new ImageFilter();

            return backend.ImageService.All(filter);
        }

        public void Delete(int id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            var obj = backend.ImageService.GetById(id);
            if (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != obj.UserID)
                throw new UnauthorizedAccessException("Unauthorized!");

            backend.ImageService.Delete(id);
        }

        public PersistentImage GetById(int id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return backend.ImageService.GetById(id);
        }

        public PersistentImage Insert(PersistentImage obj)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != obj.UserID)
                throw new UnauthorizedAccessException("Unauthorized!");

            return backend.Repository.Images.Insert(obj);
        }

        public PersistentImage Update(PersistentImage obj)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != obj.UserID)
                throw new UnauthorizedAccessException("Unauthorized!");

            return backend.Repository.Images.Update(obj);
        }

        public PersistentImage GetOfUser(int user_id)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            return backend.Repository.Images.OfUser(user_id);
        }

        public PersistentImage SaveOfUser(PersistentImage image)
        {
            if (backend.CurrentUser == null)
                throw new UnauthorizedAccessException("Unauthorized!");

            if (!backend.CurrentUser.IsAdmin && backend.CurrentUser.ID != image.UserID)
                throw new UnauthorizedAccessException("Unauthorized!");

            var old_image = backend.Repository.Images.OfUser(image.UserID);
            if (old_image != null)
            {
                old_image.Content = image.Content;
                return backend.Repository.Images.Update(old_image);
            }
            else
            {
                return backend.Repository.Images.Insert(image);
            }
        }
    }
}