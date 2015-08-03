using System;
using System.Threading;

namespace Rogero.DoOnce
{
    /// <summary>
    ///     A thread-safe class to ensure an action is only performed one time.
    /// </summary>
    public class DoOnce
    {
        long _isDone = 0; //0 is false, 1 is true.

        /// <summary>
        ///     Runs the provided action only if this method has never been called before.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="resetOnException">
        ///     If true, then if an exception occurs while running the action then the status of this
        ///     gate will be as if the action was never performed
        /// </param>
        /// <returns>True if the action was done, false if the action was not done.</returns>
        public bool Ensure(Action action, bool resetOnException)
        {
            var shouldDo = ShouldDo();
            if (shouldDo)
            {
                PerformAction(action, resetOnException);
            }
            return shouldDo;
        }

        void PerformAction(Action action, bool resetOnException)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                if (resetOnException)
                {
                    Reset();
                }
                ExceptionHandler.Handle(exception);
            }
        }

        /// <summary>
        ///     Returns true if the method has never been called before, returns false if the method has been called.
        /// </summary>
        /// <returns></returns>
        bool ShouldDo()
        {
            if (Interlocked.Exchange(ref _isDone, 1) == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        ///     Resets this object as if the action has never been performed.
        /// </summary>
        void Reset()
        {
            Interlocked.Exchange(ref _isDone, 0);
        }

        /// <summary>
        ///     Returns true if the action still needs to be performed, false otherwise.
        /// </summary>
        public bool Done => Interlocked.Read(ref _isDone) == 1;
    }
}