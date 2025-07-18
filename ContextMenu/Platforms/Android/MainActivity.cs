using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace ContextMenu;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static Activity Instance { get; private set; } = default!;

    public MainActivity()
    {
        Instance = this;
    }

    public override void OnCreateContextMenu(IContextMenu? menu, Android.Views.View? v, IContextMenuContextMenuInfo? menuInfo)
    {
        if (menu == null || v == null) return;

        // Get the position from the view's tag
        var position = v.Tag as Java.Lang.Integer;
        int itemPosition = position?.IntValue() ?? -1;

        // Set menu header
        menu.SetHeaderTitle($"Item {itemPosition}");
        
        // Add menu items
        menu.Add(0, 1, 0, "Edit Item");
        menu.Add(0, 2, 1, "Delete Item");
        menu.Add(1, 3, 2, "Share Item");
        menu.Add(1, 3, 2, "Add Item");
        
        System.Diagnostics.Debug.WriteLine($"Floating context menu created for position: {itemPosition}");
        
        base.OnCreateContextMenu(menu, v, menuInfo);
    }

    public override bool OnContextItemSelected(IMenuItem item)
    {
        var itemId = item.ItemId;
        
        System.Diagnostics.Debug.WriteLine($"Context menu item {itemId} selected");

        switch (itemId)
        {
            case 1:
                System.Diagnostics.Debug.WriteLine("Edit selected");
                // Handle edit action
                return true;
            case 2:
                System.Diagnostics.Debug.WriteLine("Delete selected");
                // Handle delete action
                return true;
            case 3:
                System.Diagnostics.Debug.WriteLine("Share selected");
                // Handle share action
                return true;
            default:
                return base.OnContextItemSelected(item);
        }
    }
}