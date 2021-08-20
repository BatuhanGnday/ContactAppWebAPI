using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ContactApp.Entities;
using ContactApp.Services.Models.Contacts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Guid;

namespace ContactApp.Services
{
    public interface IContactService
    {
        public ActionResult<Contact> CreateContact(CreateContactRequestBody requestBody, User activeUser);
        public ActionResult<Contact> GetContactById(string id);
        public ActionResult<IEnumerable<Contact>> GetContacts();

        public ActionResult<Contact> UpdateContactPartially(User activeUser, CreateContactRequestBody body,
            string contactGuid);
        public ActionResult<Contact> DeleteContactById(string guid);

        public ActionResult<IEnumerable<Contact>> GetContacts(string? name, string? phoneNumber, string? address,
            string? province, string? district);

        public ActionResult<IEnumerable<Contact>> GetContactsByUserId(string guid);
    }
    public class ContactService: IContactService
    {
        private readonly ApplicationContext _applicationContext = new();

        public ActionResult<Contact> CreateContact(CreateContactRequestBody requestBody, User activeUser)
        {
            var contactTemp = new Contact
            (
                requestBody.FullName,
                requestBody.PhoneNumber,
                requestBody.Address,
                requestBody.Province,
                requestBody.District
            )
            {
                Owner = activeUser
            };

            _applicationContext.Entry(contactTemp.Owner).State = EntityState.Unchanged;
            _applicationContext.Contacts.Add(contactTemp);
            
            _applicationContext.SaveChanges();
            return contactTemp;
        }

        public ActionResult<Contact> GetContactById(string id)
        {
            var tryParse = TryParse(id, out var guid);
            if (!tryParse)
            {
                return new BadRequestResult();
            }

            var result = _applicationContext.Contacts.Find(guid);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return result;
        }

        public ActionResult<IEnumerable<Contact>> GetContacts()
        {
            return _applicationContext.Contacts.Include(p=> p.Owner).ToList();
        }

        public ActionResult<IEnumerable<Contact>> GetContacts(
            string? name,
            string? phoneNumber,
            string? address,
            string? province,
            string? district)
        {
            var results = _applicationContext.Contacts.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                results = results.Where(c => EF.Functions.ILike(c.FullName, $"%{name}%"));
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                results = results.Where(c => EF.Functions.ILike(c.PhoneNumber, $"%{phoneNumber}%"));
            }
            if (!string.IsNullOrEmpty(address))
            {
                results = results.Where(c => EF.Functions.ILike(c.Address, $"%{address}%"));
            }
            if (!string.IsNullOrEmpty(province))
            {
                results = results.Where(c => EF.Functions.ILike(c.Province, $"%{province}%"));
            }
            if (!string.IsNullOrEmpty(district))
            {
                results = results.Where(c => EF.Functions.ILike(c.District, $"%{district}%"));
            }
            
            return results.Include(c=>c.Owner).ToList();
        }

        public ActionResult<IEnumerable<Contact>> GetContactsByUserId(string guid)
        {
            var tryParse = Guid.TryParse(guid, out var id);
            if (!tryParse)
            {
                return new NotFoundResult();
            }

            var contacts = _applicationContext.Contacts
                .Include(e => e.Owner)
                .Where(e => e.Owner.Guid == id)
                .ToList();
            return contacts;
        }

        public ActionResult<Contact> UpdateContactPartially(User activeUser, CreateContactRequestBody body, string contactGuid)
        {
            var id = activeUser.Guid;
            var tryParse = Guid.TryParse(contactGuid, out var contactId);

            if (!tryParse)
            {
                return new NotFoundResult();
            }
            
            var userContacts = _applicationContext.Contacts
                .Include(e => e.Owner)
                .Where(e => e.Owner.Guid == id).ToList();
            
            var contactToUpdate = _applicationContext.Contacts.Find(contactId);
            
            if (!userContacts.Contains(contactToUpdate))
            {
                return new UnauthorizedResult();
            }

            if (!string.IsNullOrEmpty(body.FullName))
            {
                contactToUpdate.FullName = body.FullName;
            }
            if (!string.IsNullOrEmpty(body.PhoneNumber))
            {
                contactToUpdate.PhoneNumber = body.PhoneNumber;
            }
            if (!string.IsNullOrEmpty(body.Address))
            {
                contactToUpdate.Address = body.Address;
            }
            if (!string.IsNullOrEmpty(body.Province))
            {
                contactToUpdate.Province = body.Province;
            }    
            if (!string.IsNullOrEmpty(body.District))
            {
                contactToUpdate.District = body.District;
            }
            
            _applicationContext.Contacts.Update(contactToUpdate);
            _applicationContext.SaveChanges();
            
            
            return contactToUpdate;
        }

        public ActionResult<Contact> DeleteContactById(string guid)
        {
            var tryParse = Guid.TryParse(guid, out var id);
            if (!tryParse)
            {
                return new NotFoundResult();
            }

            var contactToRemove = _applicationContext.Contacts.Find(id);
            _applicationContext.Contacts.Remove(contactToRemove);
            _applicationContext.SaveChanges();
            return contactToRemove;
        }
    }
}