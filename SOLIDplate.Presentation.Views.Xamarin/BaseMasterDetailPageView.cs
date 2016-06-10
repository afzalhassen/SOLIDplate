using System.ComponentModel;
using System.Threading.Tasks;
using SOLIDplate.Presentation.ViewModels;
using Xamarin.Forms;

namespace SOLIDplate.Presentation.Views.Xamarin
{
	public abstract class BaseMasterDetailPageView<TMasterDetailViewModel> : MasterDetailPage
		where TMasterDetailViewModel : BaseViewModel, INotifyPropertyChanged
	{
		//protected readonly ActivityIndicator ActivityIndicator = new ActivityIndicator();

		private void Initialize()
		{
		}

		protected BaseMasterDetailPageView(TMasterDetailViewModel viewModel)
		{
			Initialize();
			InitializeView();
			BindingContext = viewModel;
			NavigationPage.SetHasBackButton(this, false);
		}

		protected TMasterDetailViewModel ViewModel => BindingContext as TMasterDetailViewModel;

	    protected bool IsPortrait(Page page)
		{
			return (Device.Idiom == TargetIdiom.Phone)
				? page.Width < 400
				: page.Width < 950;

			//return p.Width < ((Device.Idiom == TargetIdiom.Phone) ? 400 : 950);
		}

		protected abstract void InitializeView();

		protected abstract override void OnAppearing();

		protected abstract override void OnDisappearing();

		protected override void OnBindingContextChanged()
		{
			SetBinding(TitleProperty, new Binding(BaseViewModel.TitlePropertyName));
			SetBinding(IconProperty, new Binding(BaseViewModel.IconPropertyName));
			//ActivityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, BaseViewModel.IsBusyPropertyName);
			//ActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, BaseViewModel.IsBusyPropertyName);
		}

		protected async Task PushPage(Page page)
		{
			await Navigation.PushAsync(page);
		}

		protected async Task PopPage()
		{
			await Navigation.PopAsync();
		}

		protected async Task PushModalPage(Page page)
		{
			await Navigation.PushModalAsync(page);
		}

		protected async Task PopModalPage()
		{
			await Navigation.PopModalAsync();
		}

		protected void PushPageNoWait(Page page)
		{
			Navigation.PushAsync(page);
		}

		protected void PopPageNoWait()
		{
			Navigation.PopAsync();
		}
	}
}