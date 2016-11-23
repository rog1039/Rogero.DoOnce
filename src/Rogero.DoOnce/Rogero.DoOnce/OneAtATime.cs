using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rogero.DoOnce
{
    public class OneAtATime
    {
        private long _isInUse = 0; //0 is false, 1 is true

        /// <summary>
        /// Request permission to reserve this object.
        /// </summary>
        /// <returns></returns>
        public PermissionResponse RequestPermission()
        {
            var notInUse = IsNotinUse();
            return notInUse ? PermissionResponse.Granted(Reset) : PermissionResponse.Denied();
        }

        private bool IsNotinUse()
        {
            if (Interlocked.Exchange(ref _isInUse, 1) == 0)
                return true;
            else
                return false;
        }

        private void Reset()
        {
            Interlocked.Exchange(ref _isInUse, 0);
        }
    }

    public class PermissionResponse
    {
        public bool PermissionGranted { get; }
        public bool PermissionDenied => !PermissionGranted;

        private readonly Action _action;

        private PermissionResponse(bool permissionGranted, Action action)
        {
            _action = action;
            PermissionGranted = permissionGranted;
        }

        public static PermissionResponse Granted(Action a) => new PermissionResponse(true, a);
        public static PermissionResponse Denied() => new PermissionResponse(false, null);

        /// <summary>
        /// Resets the OneAtATime object so another operation can use it.
        /// </summary>
        public void ReleaseHold()
        {
            _action();
        }
    }
}
