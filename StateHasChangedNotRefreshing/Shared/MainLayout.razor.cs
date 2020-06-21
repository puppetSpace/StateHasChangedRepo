using Microsoft.AspNetCore.Components;
using StateHasChangedNotRefreshing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StateHasChangedNotRefreshing.Shared
{
	public class MainLayoutBase : LayoutComponentBase, Data.IObserver<CounterUpdateNotificationMessage>
	{
		[Inject]
		public EventBroker EventBroker { get; set; }

		[Inject]
		public ObserverBroker ObserverBroker { get; set; }

		public int AmountOfClicks { get; set; }

		public void Handle(CounterUpdateNotificationMessage value)
		{
			UpdateCounter();
		}

		protected override void OnInitialized()
		{
			ObserverBroker.Subscribe(this);
			EventBroker.NotifyEvent += (o, a) =>
			{
				UpdateCounter();
			};
		}

		private void UpdateCounter()
		{
			InvokeAsync(() =>
			{
				AmountOfClicks++;
				StateHasChanged();
			});
		}
	}
}
