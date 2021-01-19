using System;
using System.Linq;
using System.Collections.Generic;
using JPProject.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Bk.Common.Exceptions;
using Bk.Common.Roles;

namespace JPProject.Sso.AspNetIdentity.Models.Identity
{
    public class UserIdentity : IdentityUser, IDomainUser
    {
        private static NotFoundException BusinessNotFound(Guid businessId) => new NotFoundException($"Businesses Not Found against Id: {businessId}");
        private static NotFoundException WorkerNotFound(Guid workerId) => new NotFoundException($"Businesses Not Found against Id: {workerId}");

        public bool ProfileCompleted { get; set; }
        public string DisplayName { get; set; }
        public Gender Gender { get; protected set; }
        public States State { get; protected set; }
        public string Pic { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        #region Constructors
        public UserIdentity(UserIdentity admin, string firstName, string lastName, string email, Gender gender, States state = States.Active)
        {
            FirstName = firstName;
            LastName = lastName;
            DisplayName = $"{firstName.Trim()} {lastName.Trim()}";
            Email = email;
            Gender = gender;
            State = state;
        }
        public UserIdentity(string firstName, string lastName, string email, Gender gender, States state = States.Active)
        {
            DisplayName = $"{firstName.Trim()} {lastName.Trim()}";
            Email = email;
            Gender = gender;
            State = state;
        }
        public UserIdentity(){}
        public UserIdentity(Guid id, string firstName, string lastName, string email, Gender gender, States state = States.Active)
        {
            Id = id.ToString();
            DisplayName = $"{firstName.Trim()} {lastName.Trim()}";
            Email = email;
            Gender = gender;
            State = state;
        }
        #endregion
        private List<Tenant> AssociatedTenants { get;  set; } = new List<Tenant>();
        private List<UserIdentity> ChildUsers { get;  set; } = new List<UserIdentity>();
        public virtual List<UserRoleIdentity> UserRoles { get; protected set; } = new List<UserRoleIdentity>();
        public void CreateBusiness(Tenant tenant)
        {
            //var ownerRole = UserRoles.FirstOrDefault(x => x.Role.Name.Contains(ApplicationRoles.Owner)) 
            //            ?? throw new Exception(@"Role Type of 'owner' not found ");

            //var userRole = new UserRoleIdentity(tenant, ownerRole.Role, this);
            //UserRoles.Add(userRole);
        }

        //public Tenant GetOwnedBusiness(Guid id)
        //{
        //    var userRole  = UserRoles.Where(x => x.TenantId.Equals(id.ToString()) && x.Role.Name.Contains(ApplicationRoles.Owner));
        //    OwnedBusinesses.FirstOrDefault(x => x.Id.Equals(id)) ?? throw BusinessNotFound(id);
        //}
        public UserIdentity Update(string firstName, string lastName, string email, Gender gender, States state)
        {
            FirstName = firstName.Trim(); 
            LastName = lastName.Trim(); 
            DisplayName = $"{firstName.Trim()} {lastName.Trim()}";
            Gender = gender;
            State = state;
            Email = email;
            return this;

        }
        public UserIdentity GetWorker(Guid id)
            => ChildUsers.FirstOrDefault(x => x.Id.Equals(id)) ?? throw WorkerNotFound(id);
        public List<UserIdentity> GetWorkers(List<Guid> ids)
            => ChildUsers.Where(x => ids.Select(x=> x.ToString()).Contains(x.Id)).ToList();
        public List<UserIdentity> GetWorkers()
            => ChildUsers.ToList();
        public void AddWorker(UserIdentity worker)
            => ChildUsers.Add(worker);
        public void Archive()
            => State = States.Inactive;
        public void Restore()
            => State = States.Active;
        public void UpdatePhoto(string pic)
            => Pic = pic;
        public void ConfirmEmail()
        {
            EmailConfirmed = true;
        }
        public void CompleteProfile()
        {
            ProfileCompleted = true;
        }
    }

    public class RoleIdentity : IdentityRole
    {
        public RoleIdentity() : base() { }
        public RoleIdentity(string name) : base(name) { }
        public virtual List<UserRoleIdentity> UserRoles { get; protected set; } = new List<UserRoleIdentity>();
    }
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}