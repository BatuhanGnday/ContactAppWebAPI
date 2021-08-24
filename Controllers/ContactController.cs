using System;
using System.Collections.Generic;
using ContactApp.Dto;
using ContactApp.Entities;
using ContactApp.Helpers;
using ContactApp.Helpers.Auth;
using ContactApp.Services;
using ContactApp.Services.Models.Contacts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
    [ApiController]
    [Route("/contacts")]
    public class ContactController
    {
        private readonly IContactService _contactService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public ContactController(IHttpContextAccessor httpContextAccessor, IContactService contactService)
        {
            _httpContextAccessor = httpContextAccessor;
            _contactService = contactService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Contact>> GetContacts(
            [FromQuery] string? name,
            [FromQuery] string? phoneNumber,
            [FromQuery] string? address,
            [FromQuery] string? province,
            [FromQuery] string? district)
        {
            return _contactService.GetContacts(name, phoneNumber, address, province, district);
        }

        [HttpGet("{guid}")]
        public ActionResult<Contact> GetContactById(string guid)
        {
            return _contactService.GetContactById(guid);
        }

        [HttpDelete("{guid}")]
        public ActionResult<Contact> DeleteContactById(string guid)
        {
            return _contactService.DeleteContactById(guid);
        }
        [HttpPost]
        public ActionResult<ContactDto> CreateContact([FromBody] CreateContactRequestBody requestBody)
        {
            if (_httpContextAccessor.HttpContext == null) return new UnauthorizedResult();
            var user = (User)_httpContextAccessor.HttpContext.Items["User"]!;
            return _contactService.CreateContact(requestBody, user);
        }

        [HttpPatch("{contactGuid}")]
        public ActionResult<Contact> UpdateContactPartially([FromBody] CreateContactRequestBody body, string contactGuid)
        {
            if (_httpContextAccessor.HttpContext == null) return new UnauthorizedResult();
            var user = (User)_httpContextAccessor.HttpContext.Items["User"]!;
            return _contactService.UpdateContactPartially(user, body, contactGuid);
        }
    }
}