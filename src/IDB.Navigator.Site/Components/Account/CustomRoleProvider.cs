using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web.Security;
using Simple.Data;
using IDB.Navigator.Domain;
using IDB.Navigator.Site.Components.Formatter;
using IDB.Navigator.Resources;

namespace IDB.Navigator.Site.Components.Account
{
    public class CustomRoleProvider : RoleProvider
    {
        public const int RoleNameMaxLength = 255;

        public override bool IsUserInRole(string username, string roleName)
        {
            if (username == null) throw new ArgumentNullException();
            if (roleName == null) throw new ArgumentNullException();

            User user = Database.Default.User.FindByEmail(username);
            if (user == null) throw new ProviderException(ValidationMessages.UserNotFoundWithUsername.Format(new { username }));

            Role role = Database.Default.Role.FindByName(roleName);
            CheckRolNotNull(role, roleName);
            return user.Role == role;
        }

        public override string[] GetRolesForUser(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException();

            User user = Database.Default.User.FindByEmail(username);
            return new[] { user.Role.Name };
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
            //CheckRoleNameArgument(roleName);
            //if (RoleRepository.GetByMultiplexAndName(roleName) != null) throw new ProviderException(string.Format("Ya existe un rol con el nombre {0}", roleName));

            //RoleRepository.SaveOrUpdate(new Role {Name = roleName});
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentException();
            return Database.Default.Role.FindByName(roleName) != null;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            CheckRoleNameArgument(roleName);
            Role role = Database.Default.Role.FindByName(roleName);
            CheckRolNotNull(role, roleName);

            IList<User> usersInRole = Database.Default.User.FindyByRol(role);
            return usersInRole.Select(u => u.Email).ToArray();
        }

        public override string[] GetAllRoles()
        {
            IList<Role> roles = Database.Default.Role.All();
            return roles.Select(r => r.Name).ToArray();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }

        private static void CheckRoleNameArgument(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentException();
            if (roleName.Contains(",") || roleName.Length > RoleNameMaxLength)
                throw new ArgumentException(ValidationMessages.InvalidRolename.Format(new { roleMaxLength = RoleNameMaxLength }));
        }

        private static void CheckRolNotNull(Role rol, string roleName)
        {
            if (rol == null)
            {
                throw new ProviderException(ValidationMessages.RoleNotFound.Format(new { roleName }));
            }
        }
    }
}