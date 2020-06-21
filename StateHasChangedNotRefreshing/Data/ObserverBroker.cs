using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateHasChangedNotRefreshing.Data
{
	public interface IObserver
	{

	}

	public interface IObserver<TE> : IObserver
	{
		void Handle(TE value);
	}

	internal class UnSubscriber<TE> : IDisposable
	{
		private readonly Dictionary<Type, List<IObserver>> _observers;
		private readonly IObserver<TE> _observer;

		public UnSubscriber(Dictionary<Type, List<IObserver>> observers, IObserver<TE> observer)
		{
			_observers = observers;
			_observer = observer;
		}

		public void Dispose()
		{
			lock (_observers)
			{
				if (_observers.ContainsKey(typeof(TE)) && _observers[typeof(TE)].FirstOrDefault(x => x.GetType() == _observer.GetType()) is var observer)
				{
					_observers[typeof(TE)].Remove(observer);
					if (!_observers[typeof(TE)].Any())
						_observers.Remove(typeof(TE));
				}
			}
		}
	}

	public class ObserverBroker
	{
		private readonly Dictionary<Type, List<IObserver>> _observers = new Dictionary<Type, List<IObserver>>();

		public void Publish<TE>(TE value)
		{
			if (_observers.ContainsKey(typeof(TE)))
			{
				foreach (IObserver<TE> observer in _observers[typeof(TE)])
					observer.Handle(value);
			}
		}

		public IDisposable Subscribe<TE>(IObserver<TE> observer)
		{
			lock (_observers)
			{
				if (_observers.ContainsKey(typeof(TE)) && _observers[typeof(TE)] is object)
				{
					if (!_observers[typeof(TE)].Any(x => x.GetType() == observer.GetType()))
						_observers[typeof(TE)].Add(observer);
				}
				else
					_observers[typeof(TE)] = new List<IObserver> { observer };

			}
			return new UnSubscriber<TE>(_observers, observer);
		}
	}
}

