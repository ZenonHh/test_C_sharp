using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DoAnCSharp.Services;

public interface ILanguageService : INotifyPropertyChanged
{
    string CurrentLocale { get; }
    string T(string key);
    void ChangeLanguage(string langCode);
    // SỬA LỖI CS1061: Thêm khai báo hàm khởi tạo
    Task Initialize();
    string this[string key] { get; }
}

public class LanguageService : ILanguageService
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _currentLocale = "vi";
    public string CurrentLocale => _currentLocale;

    public string this[string key] => T(key);
    private readonly Dictionary<string, Dictionary<string, string>> _localizedValues = new()
{
    {
        "vi", new Dictionary<string, string>
        {
            { "app_title", "Vĩnh Khánh Food Tour" },
            { "home", "🏠 Trang chủ" },
            { "search", "Tìm kiếm" },
            { "search_hint", "Tìm quán ăn, món ốc..." },
            { "map", " 🗺️Bản đồ" },
            { "profile", "👤 Tôi" },
            { "logout", "ĐĂNG XUẤT" },
            { "login", "ĐĂNG NHẬP" },
            { "hint_email", "Nhập số điện thoại/email" },
            { "hint_pass", "Nhập mật khẩu" },
            { "narrating", "Đang thuyết minh..." },
            { "direction", "Chỉ đường đến quán" },
            { "language_choice", "Chọn ngôn ngữ" },
            { "welcome", "Xin chào," },
            { "recommend", "Gợi ý cho bạn" },
            { "categories", "Danh mục món ăn" },
            { "popular", "Yêu thích nhất" },
            { "start_tour", "Bắt đầu Tour" },
            { "snails", "Ốc" },
            { "grill", "Đồ nướng" },
            { "drinks", "Đồ uống" },
            { "all", "Tất cả" },
            { "desc_quan_1", "Chào mừng bạn đến với Quán Ốc Đào..." },
            { "desc_quan_2", "Quán Ốc Oanh là điểm dừng chân lý tưởng..." }
        }
    },
    {
        "en", new Dictionary<string, string>
        {
            { "app_title", "Vinh Khanh Food Tour" },
            { "home", "🏠 Home" },
            { "search", "Search" },
            { "search_hint", "Find restaurants, snails..." },
            { "map", " 🗺️Map" },
            { "profile", "👤 Me" },
            { "logout", "LOGOUT" },
            { "login", "LOGIN" },
            { "hint_email", "Enter phone/email" },
            { "hint_pass", "Enter password" },
            { "narrating", "Narrating..." },
            { "direction", "Get directions" },
            { "language_choice", "Select Language" },
            { "welcome", "Welcome," },
            { "recommend", "Recommended for you" },
            { "categories", "Categories" },
            { "popular", "Most Popular" },
            { "start_tour", "Start Tour" },
            { "snails", "Snails" },
            { "grill", "Grill" },
            { "drinks", "Drinks" },
            { "all", "All" },
            { "desc_quan_1", "Welcome to Dao Snail Restaurant..." },
            { "desc_quan_2", "Oanh Snail Restaurant is an ideal stop..." }
        }
    },
    {
        "ja", new Dictionary<string, string>
        {
            { "app_title", "ヴィンカン・フードツアー" },
            { "home", "🏠 ホーム" },
            { "search", "検索" },
            { "search_hint", "レストラン、貝料理を検索..." },
            { "map", "🗺️ 地図" },
            { "profile", "👤 プロフィール" },
            { "logout", "ログアウト" },
            { "login", "ログイン" },
            { "hint_email", "電話番号/メールを入力" },
            { "hint_pass", "パスワードを入力" },
            { "narrating", "ナレーション中..." },
            { "direction", "お店への道順" },
            { "language_choice", "言語を選択" },
            { "welcome", "こんにちは、" },
            { "recommend", "おすすめ" },
            { "categories", "カテゴリー" },
            { "popular", "人気" },
            { "start_tour", "ツアー開始" },
            { "snails", "貝料理" },
            { "grill", "焼き物" },
            { "drinks", "飲み物" },
            { "all", "全て" },
            { "desc_quan_1", "Quán Ốc Đàoへようこそ。新鮮な貝料理を楽しめるお店です。" },
            { "desc_quan_2", "Quán Ốc Oanhは、本格的なシーフードを体験できる理想的な場所です。" }
        }
    },
    {
        "ko", new Dictionary<string, string>
        {
            { "app_title", "빈칸 푸드 투어" },
            { "home", "🏠 홈" },
            { "search", "검색" },
            { "search_hint", "식당, 조개 요리 검색..." },
            { "map", "🗺️ 지도" },
            { "profile", "👤 프로필" },
            { "logout", "로그아웃" },
            { "login", "로그인" },
            { "hint_email", "전화번호/이메일 입력" },
            { "hint_pass", "비밀번호 입력" },
            { "narrating", "설명 중..." },
            { "direction", "식당 길 찾기" },
            { "language_choice", "언어 선택" },
            { "welcome", "안녕하세요, " },
            { "recommend", "추천 항목" },
            { "categories", "카테고리" },
            { "popular", "가장 인기 있는" },
            { "start_tour", "투어 시작" },
            { "snails", "조개 요리" },
            { "grill", "구이" },
            { "drinks", "음료" },
            { "all", "전체" },
            { "desc_quan_1", "Quán Ốc Đào에 오신 것을 환영합니다. 신선한 달팽이 요리를 즐길 수 있습니다." },
            { "desc_quan_2", "Quán Ốc Oanh은 정통 해산물을 경험하기에 이상적인 장소입니다." }
        }
    }
};

    // SỬA LỖI CS1061: Triển khai hàm Initialize
    public Task Initialize()
    {
        // Mẹo 8GB RAM: Dùng Preferences để lưu lại lựa chọn của người dùng
        // Nếu lần đầu mở App, mặc định sẽ là Tiếng Việt ("vi")
        var savedLanguage = Preferences.Default.Get("selected_language", "vi");
        _currentLocale = savedLanguage;
        
        OnPropertyChanged(nameof(CurrentLocale));
        return Task.CompletedTask;
    }

    public string T(string key)
    {
        if (_localizedValues.ContainsKey(_currentLocale) && _localizedValues[_currentLocale].ContainsKey(key))
        {
            return _localizedValues[_currentLocale][key];
        }
        return key;
    }

    public void ChangeLanguage(string langCode)
    {
        if (_currentLocale != langCode)
        {
            _currentLocale = langCode;
            
            // Lưu lại lựa chọn vào bộ nhớ máy
            Preferences.Default.Set("selected_language", langCode);
            
            OnPropertyChanged(nameof(CurrentLocale));
            OnPropertyChanged("Item"); 
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
