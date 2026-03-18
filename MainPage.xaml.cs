<<<<<<< HEAD
using System.Collections.ObjectModel;
using DoAnCSharp.Models;
using DoAnCSharp.Services;

namespace DoAnCSharp
{
    public partial class MainPage : ContentPage
    {
        // Sử dụng ObservableCollection để giao diện tự cập nhật khi dữ liệu thay đổi (Đặc trưng C#)
        public ObservableCollection<POI> RecommendedPois { get; set; } = new ObservableCollection<POI>();
        public ObservableCollection<POI> AllPois { get; set; } = new ObservableCollection<POI>();

        public MainPage()
        {
            InitializeComponent();

            // Liên kết dữ liệu (Data Binding)
            BindingContext = this;

            LoadData();
        }

        private void LoadData()
        {
            // Lấy dữ liệu từ POIRepository (C#)
            var data = POIRepository.GetTourPoints();

            foreach (var item in data)
            {
                AllPois.Add(item);
                // Giả lập danh sách gợi ý (lấy 2 phần tử đầu)
                if (RecommendedPois.Count < 2)
                {
                    RecommendedPois.Add(item);
                }
            }
        }

        private async void OnLanguageClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Chọn ngôn ngữ / Select Language", "Hủy", null, "Tiếng Việt", "English");

            if (action == "Tiếng Việt")
            {
                LanguageService.ChangeLanguage("vi");
                await DisplayAlert("Thông báo", "Đã đổi sang Tiếng Việt", "OK");
            }
            else if (action == "English")
            {
                LanguageService.ChangeLanguage("en");
                await DisplayAlert("Notice", "Language changed to English", "OK");
            }
        }

        // Xử lý khi nhấn vào một địa điểm (Tương đương ListTile onTap trong Flutter)
        private async void OnItemTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is POI selectedPoi)
            {
                await DisplayAlert("Thông tin", $"Bạn đã chọn: {selectedPoi.Name}", "Đóng");
            }
        }
    }
=======
﻿namespace VinhKhanhFoodTour_Clean;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	private void OnCounterClicked(object? sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
>>>>>>> fb7b8b1 (firts)
}
