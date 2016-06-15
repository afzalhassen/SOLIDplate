using SOLIDplate.Presentation.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SOLIDplate.Presentation.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
	{
		//private readonly INetworkConnectivityService _networkConnectivityService;
		protected readonly IApplicationDataContext ApplicationDataContext;

		public event PropertyChangedEventHandler PropertyChanged;

		private string _title = string.Empty;
		public const string TitlePropertyName = "Title";
		public string Title
		{
			get { return _title; }
			set { SetProperty(ref _title, value, TitlePropertyName); }
		}

		public const string IconPropertyName = "Icon";
		private string _icon;
		public string Icon
		{
			get { return _icon; }
			set { SetProperty(ref _icon, value, IconPropertyName); }
		}

		public const string IsBusyPropertyName = "IsBusy";
		private bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			set { SetProperty(ref _isBusy, value, IsBusyPropertyName); }
		}

		//Lock buttons while performing the functions for saving or removing 
		public const string IsEnablePropertyName = "IsEnable";
		private bool _isEnable = true;
		public bool IsEnable
		{
			get { return _isEnable; }
			set { SetProperty(ref _isEnable, value, IsEnablePropertyName); }
		}

		public const string IsVisiblePropertyName = "IsVisible";
		private bool _isVisible;
		public bool IsVisible
		{
			get { return _isVisible; }
			set { SetProperty(ref _isVisible, value, IsVisiblePropertyName); }
		}

		private string _message = string.Empty;
		public const string MessagePropertyName = "Message";
		public string Message
		{
			get { return _message; }
			set { SetProperty(ref _message, value, MessagePropertyName); }
		}

		public void Busy()
		{
			IsBusy = true;
			IsEnable = false;
		}
		public void Finished()
		{
			IsBusy = false;
			IsEnable = true;
		}

		//public bool IsConnected
		//{
		//	get { return _networkConnectivityService.Current.IsConnected; }
		//}

		//public Task<bool> IsReachable
		//{
		//	get { return _networkConnectivityService.Current.IsReachable("https://tfg-prd-wa-giftregistry-webapi-1-0.azurewebsites.net/", 5000); }
		//}

		protected BaseViewModel(/*INetworkConnectivityService networkConnectivityService,*/ IApplicationDataContext applicationDataContext)
		{
			//_networkConnectivityService = networkConnectivityService;
			ApplicationDataContext = applicationDataContext;
		}

		private void OnPropertyChanged(string propertyName)
		{
		    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public abstract Task Refresh();

		protected void SetProperty<T>(ref T backingStore, T value, string propertyName, Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return;

			backingStore = value;

		    onChanged?.Invoke();

		    OnPropertyChanged(propertyName);
		}

		public void Dispose()
		{
			OnDispose();
		}

		protected virtual void OnDispose()
		{
		}
	}
}
