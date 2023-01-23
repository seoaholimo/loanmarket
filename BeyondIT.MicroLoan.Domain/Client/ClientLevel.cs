

using BeyondIT.MicroLoan.Domain.BaseTypes;

namespace BeyondIT.MicroLoan.Domain.Client
{
    public class ClientLevel : BaseEntity
    {
        public int ClientLevelId { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public int LevelNumber { get; set; }
        public string Name { get; set; }
        public string PluralName { get; set; }
        public bool Active { get; set; }

        public ClientLevel()
        {
            Active = true;
        }
    }
}
