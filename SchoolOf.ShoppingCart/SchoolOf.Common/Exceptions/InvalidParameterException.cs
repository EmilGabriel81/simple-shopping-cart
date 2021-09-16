using System;


namespace SchoolOf.Common.Exceptions
{
    public class InvalidParameterException : ArgumentException
    {
        public InvalidParameterException(String msg):base(msg)
        {
            //To be used with Global Exception Filter
        }

    }
}
