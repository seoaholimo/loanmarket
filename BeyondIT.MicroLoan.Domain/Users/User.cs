using System;
using System.Collections.Generic;
using BeyondIT.MicroLoan.Domain.BaseTypes;
using BeyondIT.MicroLoan.Domain.Client;
using BeyondIT.MicroLoan.Domain.Loans;
using BeyondIT.MicroLoan.Domain.Role;

namespace BeyondIT.MicroLoan.Domain.Users
{
    public class User : BaseEntity
    {
        public User()
        {
            Address = new Address();
            Active = true;
            IsFirstLogin = true;
            ClientPageRolesCreatedBy = new HashSet<ClientPageRoles>();
            ClientPageRolesUpdatedBy = new HashSet<ClientPageRoles>();
            ClientUserRolesCreatedBy = new HashSet<ClientUserRoles>();
            ClientUserRolesUpdatedBy = new HashSet<ClientUserRoles>();
            ClientUserRolesUser = new HashSet<ClientUserRoles>();
            ClientsCreatedBy = new HashSet<Clients>();
            ClientsUpdatedBy = new HashSet<Clients>();
            LoanLedger = new HashSet<LoanLedger>();
            RolesCreatedBy = new HashSet<Roles>();
            RolesUpdatedBy = new HashSet<Roles>();

        }

        public int UserId { get; set; }
        public string IdentityDocumentNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string CellphoneNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public virtual Address Address { get; set; }
        public bool IsSuperUser { get; set; }
        public bool Active { get; set; }
        public bool IsFirstLogin { get; set; }
        public Guid? ResetPasswordToken { get; set; }
        public DateTime? TokenExpiryDate { get; set; }
        public string FullName => $"{Name} {Surname}";
        public string Position { get; set; }


        public virtual ICollection<ClientPageRoles> ClientPageRolesCreatedBy { get; set; }
        public virtual ICollection<ClientPageRoles> ClientPageRolesUpdatedBy { get; set; }
        public virtual ICollection<ClientUserRoles> ClientUserRolesCreatedBy { get; set; }
        public virtual ICollection<ClientUserRoles> ClientUserRolesUpdatedBy { get; set; }
        public virtual ICollection<ClientUserRoles> ClientUserRolesUser { get; set; }
        public virtual ICollection<Clients> ClientsCreatedBy { get; set; }
        public virtual ICollection<Clients> ClientsUpdatedBy { get; set; }
        public virtual ICollection<LoanLedger> LoanLedger { get; set; }
        public virtual ICollection<Roles> RolesCreatedBy { get; set; }
        public virtual ICollection<Roles> RolesUpdatedBy { get; set; }

        public static User GetSystemUser()
        {
            return new User
            {
                UserId = 1,
                IsSuperUser = true
            };
        }

        public void UpdatePersonalDetails(PersonalDetails personalDetails, int updatedByUserId)
        {
            IdentityDocumentNumber = personalDetails.IdentityDocumentNumber;
            DateOfBirth = personalDetails.DateOfBirth;
            Name = personalDetails.Name;
            Surname = personalDetails.Surname;
            Email = personalDetails.EmailAddress;
            CellphoneNumber = personalDetails.CellphoneNumber;
            TelephoneNumber = personalDetails.TelephoneNumber;

            SetEntityUpdateDetails(updatedByUserId);
        }

        public void UpdateAddress(Address address, int updatedByUserId)
        {
            Address = address;

            SetEntityUpdateDetails(updatedByUserId);
        }
    }
}
