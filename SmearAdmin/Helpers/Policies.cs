using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmearAdmin.Helpers
{
    public static class Policies
    {
        public static class Users
        {
            public const string Add = "users.add.policy";
            public const string View = "users.view.policy";
            public const string ManageUser = "users.manage.policy";
        }

        public static class Admins
        {
            public const string ManageAdmin = "admins.manage.policy";
        }
    }

    public static class Permissions
    {
        public static class Users
        {
            //public const string Add = "permissions.users.add";
            //public const string View = "permissions.users.view";
            //public const string Edit = "permissions.users.edit";
            //public const string Delete = "permissions.users.delete";
            public const string Add = "Add";
            public const string View = "View";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
        }

        public static class Admins
        {
            //public const string CRUD = "permissions.admins.CRUD";
            public const string CRUD = "CRUD";

        }

        public static class Roles
        {
            public const string ADMIN = "administrator";
            public const string USER = "user";
        }

        public static class RolePermission
        {
            public const string Add = "Add";
            public const string View = "View";
            public const string Edit = "Edit";
            //public const string Update = "Update";
            public const string Delete = "Delete";
        }
    }
}
