using System;
using System.ComponentModel.DataAnnotations;

namespace ContactApp.Entities
{
    public class Contact
    {
        [Key]
        public Guid Guid { get; set; }

        public User Owner { get; set; }
        
        public string FullName { get; set; }
        
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
     
        public string Province { get; set; }
        
        public string District { get; set; }

    }
}