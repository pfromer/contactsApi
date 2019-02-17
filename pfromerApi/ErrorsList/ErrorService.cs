using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pfromerApi.ErrorsList
{
    public static class ErrorService
    {
        public static ExecutionResult UnexistingCompanyError = new ExecutionResult (1, "There is no company for the provided Id.");
        public static ExecutionResult UnexistingCountryError = new ExecutionResult (2, "There is no country for the provided Id.");
        public static ExecutionResult RepeatedEmailError = new ExecutionResult (3, "There is an existing contact with the provided email in our records.");
        public static ExecutionResult RepeatedPersonalPhoneNumberError = new ExecutionResult (4, "There is an existing contact with the provided personal phone number in our records.");
        public static ExecutionResult ContactNotFound = new ExecutionResult (5, "There is no contact for the provided id");
        public static ExecutionResult ContactNotFoundForGivenEmailAndPersonalPhoneNumber = new ExecutionResult  (6, "There is no contact for the provided email and personal phone number.");
        public static ExecutionResult PictureWasNotLoaded = new ExecutionResult(7, "There is no picture uploaded for the given contact id.");        
    }

    public class ExecutionResult
    {
        public ExecutionResult(int code, string description) {
            Success = false;
            Code = Code;
            Description = description;
        }

        public ExecutionResult(object queryResult) {
            Code = 0;
            Success = true;
            QueryResult = queryResult;
            Description = "Query Succeeded.";
        }

        public bool Success { get;  }
        public int Code { get; }
        public string Description { get; }
        public object QueryResult { get; }

    }
}
