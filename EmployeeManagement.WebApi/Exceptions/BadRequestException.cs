using System;

namespace EmployeeManagement.WebApi.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
                
        }
    }
}
