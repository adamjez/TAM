using System;

namespace OnRadio.App.Exceptions
{
    public class AppException : Exception
    {
        public AppException(String message, string userFriendlyMessage, string title) 
            : base(message)
        {
            UserFriendlyMessage = userFriendlyMessage;
            Title = title;
        }

        public AppException(String message, Exception innerException, string userFriendlyMessage, string title) 
            : base(message, innerException)
        {
            UserFriendlyMessage = userFriendlyMessage;
            Title = title;
        }

        public string UserFriendlyMessage { get; set; }
        public string Title { get; set; }
    }
}