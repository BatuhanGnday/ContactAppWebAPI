namespace ContactApp.Services.Models.Contacts
{
    public class CreateContactRequestBody
    {
        public string FullName { get; set; }
        
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
     
        public string Province { get; set; }
        
        public string District { get; set; }
    }
}