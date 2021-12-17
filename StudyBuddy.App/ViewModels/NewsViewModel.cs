﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using StudyBuddy.Model;

namespace StudyBuddy.App.ViewModels
{
    public class NewsViewModel : Notification , INotifyPropertyChanged
    {
        
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public static NewsViewModel FromNotification(Notification notification)
        {
            return new NewsViewModel()
            {
                Id = notification.Id,
                OwnerId = notification.OwnerId,
                Owner = notification.Owner,
                Title = notification.Title,
                Body = notification.Body,
                Created = notification.Created,
                Updated = notification.Updated,
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    
   
    
}
