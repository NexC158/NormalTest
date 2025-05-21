using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalSource._example_.events;


internal class EventsBuilder
{
    public double BuildTypeOne()
    {
         throw new NotImplementedException();
    }

    public byte[] BuildTypeTwo()
    {
        throw new NotImplementedException();
    }

}


internal class EventSyncronizator
{
    public event Action TimeToSendTypeOne;
}
