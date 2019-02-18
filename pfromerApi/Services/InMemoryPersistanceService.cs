using Microsoft.AspNetCore.Http;
using pfromerApi.DataModels;
using pfromerApi.ErrorsList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pfromerApi.Services
{
    public class InMemoryPersistanceService : PersistanceInterface
    {

        public InMemoryPersistanceService()
        {
            _lastInsertedId = 0;
            _contacts = new List<Contact>();
            _profilePictures = new Dictionary<int, byte[]>();
            InitializeCompanies();
            InitializeCountries();
        }

        private List<Contact> _contacts;
        private int _lastInsertedId;
        private List<Company> _companies;
        private List<Country> _countries;
        private Dictionary<int, byte[]> _profilePictures;

        public ExecutionResult Load(int id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            return contact == null ? ErrorService.ContactNotFound : new ExecutionResult(contact);
        }

        public ExecutionResult GetAllBy(string state, string city)
        {
            if (String.IsNullOrEmpty(state) && String.IsNullOrEmpty(city))
                return new ExecutionResult(new List<Contact>());

            var result = _contacts;

            if (!String.IsNullOrEmpty(state))
                result = result.Where(c => c.Address.State == state).ToList();

            if (!String.IsNullOrEmpty(city))
                result = result.Where(c => c.Address.City == city).ToList();

            return new ExecutionResult(result);
        }

        public ExecutionResult Search(string email, string phoneNumber)
        {

            if (String.IsNullOrEmpty(email) && String.IsNullOrEmpty(phoneNumber))
                return ErrorService.ContactNotFound;

            var result = _contacts;

            if (!String.IsNullOrEmpty(email))
                result = result.Where(c => c.Email == email).ToList();

            if (!String.IsNullOrEmpty(phoneNumber))
                result = result.Where(c => c.PersonalPhoneNumber == phoneNumber).ToList();

            var contact = result.FirstOrDefault();

            //assuming there can only be one contact for a given email or personal phone number
            return contact == null ? ErrorService.ContactNotFound : new ExecutionResult(contact);
        }

        public ExecutionResult Save(Contact contact)
        {
            var status = CheckBusinessConstraints(contact);

            if (BadStatus(status))
            {
                return status;
            }

            _lastInsertedId++;
            Add(contact, _lastInsertedId);

            return new ExecutionResult(contact);
        }

        public ExecutionResult Update(int id, Contact contact)
        {
            var contactToUpdate = GetContactById(id);
            if (contactToUpdate == null)
                return ErrorService.ContactNotFound;

            _contacts.Remove(contactToUpdate);
            var status = CheckBusinessConstraints(contact);

            if (BadStatus(status))
            {
                _contacts.Add(contactToUpdate);
                return status;
            }

            Add(contact, id);

            return new ExecutionResult(contact);
        }

        public ExecutionResult Delete(int id)
        {
            var contactToDelete = GetContactById(id);
            if (contactToDelete == null)
                return ErrorService.ContactNotFound;

            _profilePictures.Remove(id);
            _contacts.Remove(contactToDelete);
            return new ExecutionResult(contactToDelete);
        }

        public ExecutionResult SavePicture(int contactId, IFormFile file) {

            if (GetContactById(contactId) == null)
            {
                return ErrorService.ContactNotFound;
            }

            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    _profilePictures.Add(contactId, fileBytes);
                }
            }

            return new ExecutionResult("File uploaded for contact");
        }


        public ExecutionResult LoadPicture(int contactId)
        {

            if (GetContactById(contactId) == null)
            {
                return ErrorService.ContactNotFound;
            }

            if (!_profilePictures.ContainsKey(contactId))
            {
                return ErrorService.PictureWasNotLoaded;
            }

            return new ExecutionResult(_profilePictures[contactId]);
        }

        #region private methods
        private void InitializeCompanies()
        {
            _companies = new List<Company>();
            _companies.Add(new Company { Id = 1, Name = "Amazon" });
            _companies.Add(new Company { Id = 2, Name = "Google" });
        }

        private void InitializeCountries()
        {
            _countries = new List<Country>();
            _countries.Add(new Country { Id = 1, Name = "United States" });
            _countries.Add(new Country { Id = 2, Name = "Argentina" });
        }

        private ExecutionResult CheckBusinessConstraints(Contact contact)
        {
            //TODO: this should all be transactional 
            var company = CompanyByContact(contact);
            var country = CountryByContact(contact);

            if (_contacts.Where(c => c.Email == contact.Email).Count() > 0)
                return ErrorService.RepeatedEmailError;

            if (_contacts.Where(c => c.PersonalPhoneNumber == contact.PersonalPhoneNumber).Count() > 0)
                return ErrorService.RepeatedPersonalPhoneNumberError;

            if (company == null)
                return ErrorService.UnexistingCompanyError;

            if (country == null)
                return ErrorService.UnexistingCountryError;

            return null;
        }

        private Company CompanyByContact(Contact contact) {
            return _companies.Where(c => c.Id == contact.Company.Id).FirstOrDefault();
        }

        private Country CountryByContact(Contact country)
        {
            return _countries.Where(c => c.Id == country.Company.Id).FirstOrDefault();
        }

        private void Add(Contact contact, int id) {
            contact.Id = id;
            contact.Company = CompanyByContact(contact);
            contact.Address.Country = CountryByContact(contact);
            _contacts.Add(contact);
        }

        private bool BadStatus(ExecutionResult executionResult)
        {
            return executionResult != null && !executionResult.Success;
        }

        private Contact GetContactById(int id)
        {
            return _contacts.FirstOrDefault(c => c.Id == id);
        }
        #endregion
    }
}
