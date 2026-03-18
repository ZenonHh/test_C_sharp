using VinhKhanhFoodTour.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace VinhKhanhFoodTour.Views;

public partial class MapPage : ContentPage
{
    private readonly MapViewModel _viewModel;

    public MapPage(MapViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnCenterUserClicked(object sender, EventArgs e)
    {
        // Kiểm tra FoodMap có null không trước khi gọi để tránh Code 3
        if (FoodMap != null && _viewModel.UserPos != null)
        {
            var region = MapSpan.FromCenterAndRadius(
                _viewModel.UserPos, 
                Distance.FromMeters(200));
                
            FoodMap.MoveToRegion(region);
        }
        else
        {
            await DisplayAlert("Thông báo", "Vui lòng đợi trong giây lát để GPS ổn định...", "OK");
        }
    }
}