using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateHasChangedNotRefreshing.Data
{
    public class EventBroker
    {
        public event EventHandler NotifyEvent;

        public void Notify()
		{
            NotifyEvent?.Invoke(this,EventArgs.Empty);
		}
    }
}
