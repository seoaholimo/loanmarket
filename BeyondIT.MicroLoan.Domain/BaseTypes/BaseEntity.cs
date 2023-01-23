using System;
using BeyondIT.MicroLoan.Domain.Users;
using Newtonsoft.Json;

namespace BeyondIT.MicroLoan.Domain.BaseTypes
{
    public abstract class BaseEntity : IBaseEntity
    {
        [JsonIgnore] public int? CreatedById { get; set; }
        [JsonIgnore] public virtual User CreatedBy { get; set; }
        [JsonIgnore] public int? UpdatedById { get; set; }
        [JsonIgnore] public virtual User UpdatedBy { get; set; }
        [JsonIgnore] public DateTime DateCreated { get; set; }
        [JsonIgnore] public DateTime DateUpdated { get; set; }

        public void SetEntityCreationDetails(int createdById)
        {
            CreatedById = createdById;
            UpdatedById = createdById;

            var now = DateTime.Now;
            DateCreated = now;
            DateUpdated = now;
        }

        public void SetEntityUpdateDetails(int updatedById)
        {
            UpdatedById = updatedById;
            DateUpdated = DateTime.Now;
        }

        public bool IsResourceEdit()
        {
            return CreatedById.HasValue;
        }
    }
}