using System;
using System.Collections.Generic;
using StudyBuddy.Model;

namespace StudyBuddy.Persistence
{
    public class UserConverter : BaseConverter<User>
    {
        public override User Convert(DataSet set, int row)
        {
            var obj = new User();
            obj.ID = set.GetInt(row, "id");
            obj.Firstname = set.GetString(row, "firstname");
            obj.Lastname = set.GetString(row, "lastname");
            obj.Nickname = set.GetString(row, "nickname");
            obj.Email = set.GetString(row, "email");
            obj.PasswordHash = set.GetString(row, "password_hash");
            obj.Role = (Role)set.GetInt(row, "role");
            obj.EmailConfirmed = set.GetBool(row, "emailconfirmed");
            obj.AccountActive = set.GetBool(row, "accountactive");
            obj.CommonFriends = set.GetInt(row, "commonfriends");
            obj.Created = set.GetDateTime(row, "datecreated");
            return obj;
        }
    }
}