using System;
using BeyondIT.MicroLoan.Domain.Users;

namespace BeyondIT.MicroLoan.Domain.BaseTypes
{
    public interface IBaseEntity
    {
        User CreatedBy { get; set; }
        int? CreatedById { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        User UpdatedBy { get; set; }
        int? UpdatedById { get; set; }

        bool IsResourceEdit();
        void SetEntityCreationDetails(int createdById);
        void SetEntityUpdateDetails(int updatedById);
    }
}