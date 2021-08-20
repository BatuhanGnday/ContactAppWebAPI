using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactApp.Entities
{
    public class Contact
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }

        public User Owner { get; set; }
        
        public string FullName { get; set; }
        
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
     
        public string Province { get; set; }
        
        public string District { get; set; }

        public Contact(string fullName, string phoneNumber, string address, string province, string district)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Address = address;
            Province = province;
            District = district;
        }
    }
}