using System;
using System.Collections.Generic;
using System.Text;

namespace IUGOCare.Application.Common.Exceptions
{
    public class SendEmailException: Exception
    {
        public SendEmailException()
            :base()
        {
        }

        public SendEmailException(string message)
            :base(message)
        {

        }
    }
}
