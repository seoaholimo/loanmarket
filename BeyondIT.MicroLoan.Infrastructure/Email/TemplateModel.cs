namespace BeyondIT.MicroLoan.Infrastuture.Email
{ 
    public class TemplateModel<T> where T : class
    {
        public TemplateModel(T model)
        {
            Model = model;
        }

        public T Model { get; }
    }
}