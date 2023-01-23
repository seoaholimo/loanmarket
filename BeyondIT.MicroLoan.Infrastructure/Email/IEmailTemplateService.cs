namespace BeyondIT.MicroLoan.Infrastuture.Email
{
    public interface IEmailTemplateService
    {
        string GetEmailTemplate<T>(string viewName, TemplateModel<T> templateModel) where T : class;
    }
}