using System;
using System.Collections.Generic;
using System.Text;

namespace Acceptor
{
    public interface IReceiver<T>
    {
        void Receive(
            Func<T, MessageProcessResponse> onProcess,
            Action<Exception> onError,
            Action onWait);
    }
}
