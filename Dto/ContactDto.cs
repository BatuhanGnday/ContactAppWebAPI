using ContactApp.Entities;

namespace ContactApp.Dto
{
    public class ContactDto
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string PhoneNumber { get; set; }
        public string OwnerFullName { get; set; }
    }
}