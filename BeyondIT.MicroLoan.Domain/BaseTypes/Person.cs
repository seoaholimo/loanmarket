namespace BeyondIT.MicroLoan.Domain.BaseTypes
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public string EmailAddress { get; set; }
        public string CellphoneNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public virtual Address Address { get; set; }
        public string FullName => $"{Name} {Surname}";
    }
}
