using Microsoft.AspNetCore.Http;
using RestApi.DataModels;
using RestApi.ErrorsList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Services
{
    public interface PersistanceInterface
    {
        ExecutionResult Load(int id);
        ExecutionResult GetAllBy(string state, string city);
        ExecutionResult Search(string email, string phoneNumber);
        ExecutionResult Save(Contact contact);
        ExecutionResult Update(int id, Contact contact);
        ExecutionResult Delete(int id);
        ExecutionResult SavePicture(int contactId, IFormFile file);
        ExecutionResult LoadPicture(int contactId);
    }
}
