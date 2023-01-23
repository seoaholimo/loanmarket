using BeyondIT.MicroLoan.Domain.Attributes;

namespace BeyondIT.MicroLoan.Domain.Page
{
    [TsEnum]
    public enum PageKey
    {
        None,
        AccountLogin,
        AccountChangePassword,
        AccountForgotPassword,
        AccountResetPassword,
        AccountLogout,
        Users,
        UsersAdd,
        UsersEdit,
        Clients,
        ClientAdd,
        ClientEdit,
        ClientDetails,
        ClientContactsAdd,
        ClientContactsEdit,
        Roles,
        RolesAdd,
        RolesEdit,
        RolesClientPages,
        RolesClientUsers,
        RolesUserMultipleClients,
        CalendarPublicHolidays,
        CalendarPublicHolidaysAdd,
        CalendarPublicHolidaysDelete,
    }
}