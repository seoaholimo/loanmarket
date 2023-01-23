using BeyondIT.MicroLoan.Domain.Attributes;

namespace MoiFleet.Domain.Clients.enums
{
    [TsEnum]
    public enum ClientStatusEnumType
    {
        Prospective = 1,
        Live,
        BlockedOrInactive
    }
}
