using System;

namespace Rogero.DoOnce
{
    /// <summary>
    /// A class that allows the user to provide an Action to be performed when an exception is raised in this component.
    /// </summary>
    public static class ExceptionHandler
    {
        internal static void Handle(Exception exception)
        {
            if (ExceptionLogger == null)
            {
                Console.WriteLine(exception);
            }
            else
            {
                ExceptionLogger(exception);
            }
        }

        /// <summary>
        /// The method called when an exception is raised in this component.
        /// </summary>
        public static Action<Exception> ExceptionLogger { get; set; } 
    }
}