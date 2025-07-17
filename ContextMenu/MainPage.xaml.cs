namespace ContextMenu;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        
        var list = new List<string>();

        for (var i = 0; i < 100; i++)
        {
            list.Add($"Item {i}");
        }

        cv.ItemsSource = list;
    }
}