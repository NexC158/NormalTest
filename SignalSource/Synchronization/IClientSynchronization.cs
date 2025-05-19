using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource.Synchronization
{
    internal interface IClientSynchronization
    {
        Task StartTransferDataAsync();
    }
}
