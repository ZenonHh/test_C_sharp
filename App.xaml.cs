<<<<<<< HEAD
namespace DoAnCSharp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Thiết lập trang chính là MainPage
            MainPage = new AppShell();
        }
    }
}
=======
﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace VinhKhanhFoodTour_Clean;

public partial class App : Application
{
    public App()
    {
        // Hàm này rất quan trọng để liên kết với file App.xaml bạn vừa gửi
        InitializeComponent();

        // Thiết lập trang chính của ứng dụng
        MainPage = new AppShell();
    }
}
>>>>>>> fb7b8b1 (firts)
