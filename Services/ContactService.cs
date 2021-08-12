using System;
using System.Linq;
using ContactApp.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Services
{
    public class ContactService
    {
        private readonly ApplicationContext _applicationContext = new();

        public Contact CreateContact(Contact contact)
        {
            var contactTemp = new Contact();
            
            _applicationContext.Contacts.Add(contact);
            _applicationContext.SaveChanges();
            return contact;
        }

        public Contact GetContactById(string id)
        {
            var guid = new Guid(id);
            var result = _applicationContext.Contacts.First(contact => contact.Guid == guid);
            return result ?? new Contact();
        }
    }
}