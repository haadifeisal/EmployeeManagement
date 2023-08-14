using System;

namespace EmployeeManagement.WebApi.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
              
        }
    }
}
