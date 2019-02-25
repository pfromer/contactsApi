using RestApi.Controllers;
using RestApi.DataModels;
using RestApi.Services;
using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using RestApi.ErrorsList;

namespace XUnitTestProject1
{
    public class ContactsApiTest : IDisposable
    {
        ContactsController _controller;
        PersistanceInterface _service;
        Contact someContact; 

        public ContactsApiTest()
        {
            Dispose();
            InitContact();
        }

        public void Dispose()
        {
            _service = new InMemoryPersistanceService();
            _controller = new ContactsController(_service);
        }

        [Fact]
        public void CustomerIsCreatedCorrectly()
        {
            var result = _controller.Post(someContact);
            var contactFromResponse = (Contact)((CreatedResult)result).Value;
            Assert.Equal(1, contactFromResponse.Id);
            Assert.Equal(someContact, contactFromResponse);
        }

        [Fact]
        public void ShouldNotCreateCustomerWithRepeatedEmail()
        {
            //create one customers
            _controller.Post(someContact);
            //create second customer with same email
            var response = (BadRequestObjectResult)_controller.Post(someContact);
            var executionResult = GetExecutionResultFromResponse(response);
            Assert.Equal(executionResult, ErrorService.RepeatedEmailError);
        }

        #region private methods
        private void InitContact()
        {
            var address = new Address
            {
                AddressLine1 = "42 Stree 124",
                City = "Manhattan",
                State = "NY",
                Country = new Country { Id = 1, Name = "United States" }
            };

            var company = new Company { Id = 1, Name = "Solstice" };

            someContact = new Contact();
            someContact.Address = address;
            someContact.Name = "Pablo";
            someContact.Email = "pablofromer@gmail.com";
            someContact.Birthdate = new DateTime(1980, 11, 24);
            someContact.Company = company;
            someContact.WorkPhoneNumber = "4711-5996";
            someContact.PersonalPhoneNumber = "4790-3052";
        }

        private ExecutionResult GetExecutionResultFromResponse(BadRequestObjectResult response)
        {
            var value = (JsonResult)((BadRequestObjectResult)response).Value;
            return (ExecutionResult)value.Value;
        }

        #endregion
    }
}
