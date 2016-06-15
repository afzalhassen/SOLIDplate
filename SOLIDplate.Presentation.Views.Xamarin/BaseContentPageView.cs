using SOLIDplate.Presentation.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZeroFive.Forms.Images;

namespace SOLIDplate.Presentation.Views.Xamarin
{
    public abstract class BaseContentPageView<TViewModel> : ContentPage
		where TViewModel : BaseViewModel, INotifyPropertyChanged
	{
		//Global Variables
		protected readonly string ImageBackgroundIos = "background.png";
		protected readonly string ImageBackgroundAndroid = "background.png";
		protected readonly string ImageBackgroundLandscapeIos = "background-landscape.png";
		protected readonly string ImageBackgroundLandscapeAndroid = "background_Landscape.png";

		protected int ImageSize = 300;
		protected readonly int PhoneLayoutWidth = 300;
		protected readonly int TabletLayoutWidth = 500;

		protected readonly int ActivityMessageHeightWidth = 30;

		//Layouts
		protected readonly StackLayout LoadingLayout = new StackLayout();

		protected readonly StackLayout MainLayout = new StackLayout();
		protected readonly StackLayout ViewLayout = new StackLayout();

		protected readonly StackLayout ViewHeaderLayout = new StackLayout();
		protected readonly StackLayout ViewBodyLayout = new StackLayout();
		protected readonly StackLayout ViewFooterLayout = new StackLayout();

		//Controls
		protected readonly RoundedImage ViewHeaderImage = new RoundedImage();
		protected readonly Label ViewHeaderLabel = new Label();
		protected readonly Label ViewFooterLabel = new Label();

		protected readonly Grid ActivityToastMessageGrid = new Grid();
		protected readonly ActivityIndicator ActivityIndicator = new ActivityIndicator();
		protected readonly Label Toastmessage = new Label();

		private void Initialize()
		{
			BackgroundColor = Color.Black;

			ActivityToastMessageGrid.ColumnDefinitions = new ColumnDefinitionCollection
			{
				new ColumnDefinition
				{
					Width = GridLength.Auto                                
				},
				new ColumnDefinition
				{
					Width = new GridLength(9, GridUnitType.Star)
				}
			};

			LoadingLayout.VerticalOptions = LayoutOptions.StartAndExpand;
			LoadingLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
			LoadingLayout.Children.Add(ActivityToastMessageGrid);

			Toastmessage.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
			Toastmessage.TextColor = Color.White;
			Toastmessage.HeightRequest = ActivityMessageHeightWidth;

			Toastmessage.HorizontalOptions = LayoutOptions.FillAndExpand;
			Toastmessage.LineBreakMode = LineBreakMode.WordWrap;

			Toastmessage.VerticalOptions = LayoutOptions.CenterAndExpand;
			Toastmessage.VerticalTextAlignment = TextAlignment.Center;
			Toastmessage.HorizontalTextAlignment = TextAlignment.Start;

			ActivityIndicator.HorizontalOptions = LayoutOptions.CenterAndExpand;
			ActivityIndicator.WidthRequest = ActivityMessageHeightWidth;
			ActivityIndicator.HeightRequest = ActivityMessageHeightWidth;
			ActivityIndicator.Color = Color.FromHex("5CB8E6");
			ActivityToastMessageGrid.Children.Add(ActivityIndicator, 0, 0);
			ActivityToastMessageGrid.Children.Add(Toastmessage, 1, 0);

			ViewHeaderLabel.VerticalTextAlignment = TextAlignment.Center;
			ViewHeaderLabel.HorizontalTextAlignment = TextAlignment.Center;
			ViewHeaderLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
			ViewHeaderLabel.Opacity = 0.7;
			ViewHeaderLabel.BackgroundColor = Color.Black;
			ViewHeaderLabel.TextColor = Color.White;
			ViewHeaderLabel.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
			ViewHeaderLabel.HeightRequest = 40;
			ViewHeaderLabel.LineBreakMode = LineBreakMode.WordWrap;
			ViewHeaderLabel.WidthRequest = 250;

			var source = new UriImageSource
			{
				CacheValidity = new TimeSpan(120, 0, 0, 0),
				CachingEnabled = true,

			};

			if (Device.Idiom == TargetIdiom.Phone)
			{
				ViewHeaderImage.HeightRequest = 115; //initial default value
				ViewLayout.WidthRequest = PhoneLayoutWidth;
			}
			else
			{
				ImageSize = 500;
				ViewHeaderImage.HeightRequest = 240; //initial default value
				ViewLayout.WidthRequest = TabletLayoutWidth;
			}

			ViewHeaderImage.Aspect = Aspect.AspectFill;
			ViewHeaderImage.BorderThickness = 1;
			ViewHeaderImage.BorderColor = Color.Black;
			ViewHeaderImage.BorderRadius = 20;
			ViewHeaderImage.Source = source;

			ViewFooterLabel.VerticalTextAlignment = TextAlignment.Center;
			ViewFooterLabel.HorizontalTextAlignment = TextAlignment.Center;
			ViewFooterLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
			ViewFooterLabel.TextColor = Color.White;
			ViewFooterLabel.FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label));
			ViewFooterLabel.HeightRequest = 40;
			ViewFooterLabel.Text = @"© Copyright HassenSoft. All Rights Reserved.";
			ViewFooterLabel.WidthRequest = 250;

			//Layouts

			ViewHeaderLayout.VerticalOptions = LayoutOptions.StartAndExpand;
			ViewBodyLayout.VerticalOptions = LayoutOptions.StartAndExpand;
			ViewBodyLayout.Opacity = 0.85;
			ViewFooterLayout.VerticalOptions = LayoutOptions.EndAndExpand;
			ViewFooterLayout.Padding = new Thickness(0, 0, 0, 5);

			ViewHeaderLayout.Children.Add(ViewHeaderImage);
			ViewHeaderLayout.Children.Add(ViewHeaderLabel);
			ViewFooterLayout.Children.Add(ViewFooterLabel);

			MainLayout.Orientation = StackOrientation.Vertical;
			MainLayout.VerticalOptions = LayoutOptions.FillAndExpand;

			ViewLayout.VerticalOptions = LayoutOptions.FillAndExpand;
			ViewLayout.HorizontalOptions = LayoutOptions.Center;
			ViewLayout.Children.Add(ViewHeaderLayout);
			ViewLayout.Children.Add(ViewBodyLayout);
			ViewLayout.Children.Add(ViewFooterLayout);

			MainLayout.Children.Add(ActivityToastMessageGrid);
			MainLayout.Children.Add(ViewLayout);

			//Debugging Screen layout
			//MainLayout.BackgroundColor = Color.Pink;
			//ViewLayout.BackgroundColor = Color.Blue;
			//ViewHeaderLayout.BackgroundColor = Color.Purple;
			//ViewBodyLayout.BackgroundColor = Color.Yellow;
			//ViewFooterLayout.BackgroundColor = Color.Red;   
		}

		protected BaseContentPageView(TViewModel viewModel)
		{
			Initialize();
			InitializeView();
			BindingContext = viewModel;
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected TViewModel ViewModel => BindingContext as TViewModel;

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

			ActivityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, BaseViewModel.IsBusyPropertyName);
			ActivityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, BaseViewModel.IsBusyPropertyName);

			Toastmessage.SetBinding(Label.TextProperty, BaseViewModel.MessagePropertyName);
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

		//protected void HandleWebException(Exception ex)
		//{
		//	if (ViewModel.IsConnected)
		//	{
		//		ShowMessage(new MessageResponse { Message = "We're currently experiencing issues, try again later.", MessageType = messagetype.Error });

		//		Insights.Report(ex);
		//	}
		//	else
		//	{
		//		ShowMessage(new MessageResponse { Message = "App requires an internet connection to continue.", MessageType = messagetype.Error });
		//	}
		//}

		//protected void ShowMessage(MessageResponse message)
		//{
		//	ActivityToastMessageGrid.Opacity = 1;
		//	ViewModel.Message = " " + message.Message;
		//	switch (message.MessageType)
		//	{
		//		case messagetype.Normal:
		//			ActivityToastMessageGrid.Opacity = 0.7;
		//			ActivityToastMessageGrid.BackgroundColor = Color.FromHex("#000000");
		//			break;
		//		case messagetype.Error:
		//			ActivityToastMessageGrid.Opacity = 0.6;
		//			ActivityToastMessageGrid.BackgroundColor = Color.Red;
		//			break;
		//		case messagetype.Validation:
		//			ActivityToastMessageGrid.BackgroundColor = Color.FromHex("#5CB8E6");
		//			break;
		//		case messagetype.Clear:
		//			ActivityToastMessageGrid.BackgroundColor = Color.Transparent;
		//			break;
		//	}
		//}
	}
}