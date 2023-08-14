using System;

namespace EmployeeManagement.WebApi.Exceptions
{
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException(string message) : base(message)
        {

        }
    }
}
