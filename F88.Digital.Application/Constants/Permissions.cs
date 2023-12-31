﻿using System.Collections.Generic;

namespace F88.Digital.Application.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static class Dashboard
        {
            public const string View = "Permissions.Dashboard.View";
            public const string Create = "Permissions.Dashboard.Create";
            public const string Edit = "Permissions.Dashboard.Edit";
            public const string Delete = "Permissions.Dashboard.Delete";
        }
      

        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }
        public static class Brands
        {
            public const string View = "Permissions.Brands.View";
            public const string Create = "Permissions.Brands.Create";
            public const string Edit = "Permissions.Brands.Edit";
            public const string Delete = "Permissions.Brands.Delete";
        }
        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
        }
       
        public static class UserOTP
        {
            public const string View = "Permissions.UserOTP.View";
            public const string Create = "Permissions.UserOTP.Create";
            public const string Edit = "Permissions.UserOTP.Edit";
            public const string Delete = "Permissions.UserOTP.Delete";
        }

        public static class UserProfiles
        {
            public const string View = "Permissions.UserProfiles.View";
            public const string Create = "Permissions.UserProfiles.Create";
            public const string Edit = "Permissions.UserProfiles.Edit";
            public const string Delete = "Permissions.UserProfiles.Delete";
        }

        public static class AppPartner
        {
            public const string EMAIL_REQUEST_TOKEN = "superadmin@gmail.com";

            public const string PASSWORD_REQUEST_TOKEN = "123Pa$$word!";
        }
    
    }
}