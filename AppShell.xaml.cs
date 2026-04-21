using DoAnCSharp.Services;
using DoAnCSharp.Views; 

namespace DoAnCSharp;

public partial class AppShell : Shell
{
    
    public AppShell(ILanguageService langService)
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ScanQRPage), typeof(ScanQRPage));
        Routing.RegisterRoute("PaymentPage", typeof(Views.PaymentPage));
        Routing.RegisterRoute("HistoryPage", typeof(Views.HistoryPage));
        Routing.RegisterRoute("EditProfilePage", typeof(Views.EditProfilePage));

        BindingContext = langService;
    }
}