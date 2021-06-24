using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleHashing.Net;

namespace StudyBuddy.Model
{
    public class UserViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        public string PasswordHash { get; set; }

        public bool PasswordIsOk
        {
            get 
            {
                if (ID == 0 && string.IsNullOrEmpty(Password))
                    return false;
                
                if (!string.IsNullOrEmpty(Password) && Password != PasswordRepeat)
                    return false;
                
                return true;
            }
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public Role Role { get; set; }
        public SelectList AllRoles 
        { 
            get 
            {
                return new SelectList(Enum.GetValues(typeof(Role)));
            }
         }

        public int? ProgramID { get; set; }
        public StudyProgram Program { get; set; }
        public SelectList AllPrograms { get; set; }

        public ICollection<Team> Teams { get; set; }

        public static UserViewModel FromUser(User obj)
        {
            var user = new UserViewModel();
            user.ID = obj.ID;
            user.Firstname = obj.Firstname;
            user.Lastname = obj.Lastname;
            user.Nickname = obj.Nickname;
            user.Email = obj.Email;
            user.Role = obj.Role;
            user.ProgramID = obj.ProgramID;
            return user;
        }

        public static User ToUser(UserViewModel obj)
        {
            var user = new User();
            user.ID = obj.ID;
            user.Firstname = obj.Firstname;
            user.Lastname = obj.Lastname;
            user.Nickname = obj.Nickname;
            user.Email = obj.Email;
            user.Role = obj.Role;
            user.ProgramID = obj.ProgramID;

            if (!string.IsNullOrEmpty(obj.Password))
            {
                var simpleHash = new SimpleHash();
                user.PasswordHash = simpleHash.Compute(obj.Password);
            }
            else
            {
                user.PasswordHash = obj.PasswordHash;
            }

            return user;
        }
    }
}