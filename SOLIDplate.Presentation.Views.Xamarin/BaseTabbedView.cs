using System.ComponentModel;
using System.Threading.Tasks;
using SOLIDplate.Presentation.ViewModels;
using Xamarin.Forms;

namespace SOLIDplate.Presentation.Views.Xamarin
{
	public abstract class BaseTabbedView<TViewModel> : TabbedPage
		where TViewModel : BaseViewModel, INotifyPropertyChanged
	{
		protected TViewModel ViewModel => BindingContext as TViewModel;

	    private void Initialize()
		{
		}

		protected BaseTabbedView(TViewModel viewModel)
		{
			Initialize();
			InitializeView();
			BindingContext = viewModel;
		}

		protected abstract void InitializeView();

		protected abstract override void OnAppearing();

		protected abstract override void OnDisappearing();

		protected override void OnBindingContextChanged()
		{
			SetBinding(TitleProperty, new Binding(BaseViewModel.TitlePropertyName));
			SetBinding(IconProperty, new Binding(BaseViewModel.IconPropertyName));
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