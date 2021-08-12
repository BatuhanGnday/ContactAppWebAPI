using System;
using System.Collections.Generic;
using System.Linq;
using ContactApp.Entities;
using ContactApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
    [ApiController]
    [Route("/contacts")]
    public class ContactController
    {
        private readonly ContactService _contactService = new();
        
        [HttpGet]
        public IEnumerable<Contact> GetContacts()
        {
            return Enumerable.Empty<Contact>();
        }    
        
        [HttpGet("{guid}")]
        public Contact GetContactById(string guid)
        {
            return _contactService.GetContactById(guid);
        }

        [HttpPost]
        public Contact CreateContact([FromBody] Contact contact)
        {
            return _contactService.CreateContact(contact);
        }
    }
}