using Android.App;
using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Controls.Handlers.Items;
using AndroidX.AppCompat.Widget;

namespace ContextMenu;

class MyCVHandler : CollectionViewHandler
{
    protected override RecyclerView CreatePlatformView()
    {
        return new MyRecyclerView(Context, GetItemsLayout, CreateAdapter);
    }
}

public class MyRecyclerView : MauiRecyclerView<ReorderableItemsView, GroupableItemsViewAdapter<ReorderableItemsView, IGroupableItemsViewSource>, IGroupableItemsViewSource>
{
    private GestureDetector? _gestureDetector;
    
    public MyRecyclerView(Context context, Func<IItemsLayout> getItemsLayout, Func<GroupableItemsViewAdapter<ReorderableItemsView, IGroupableItemsViewSource>> getAdapter) : base(context, getItemsLayout, getAdapter)
    {
        SetupContextMenu();
    }
    
    private void SetupContextMenu()
    {
        _gestureDetector = new GestureDetector(Context, new RecyclerViewGestureListener(this));
        
        // Add an ItemTouchListener that will intercept touch events
        AddOnItemTouchListener(new RecyclerItemClickListener(this, _gestureDetector));
    }
    
    public void ShowContextMenuForPosition(int position, Android.Views.View view)
    {
        try
        {
            var popup = new PopupMenu(MainActivity.Instance, view);
            
            // Add menu items
            popup.Menu?.Add(0, 1, 0, "Edit Item");
            popup.Menu?.Add(0, 2, 1, "Delete Item");
            popup.Menu?.Add(0, 3, 2, "Share Item");
            
            // Handle menu item clicks
            popup.MenuItemClick += (sender, args) =>
            {
                var itemId = args.Item?.ItemId;
                System.Diagnostics.Debug.WriteLine($"Context menu item {itemId} clicked for position {position}");
                
                switch (itemId)
                {
                    case 1:
                        System.Diagnostics.Debug.WriteLine($"Edit selected for item at position {position}");
                        break;
                    case 2:
                        System.Diagnostics.Debug.WriteLine($"Delete selected for item at position {position}");
                        break;
                    case 3:
                        System.Diagnostics.Debug.WriteLine($"Share selected for item at position {position}");
                        break;
                }
            };
            
            popup.Show();
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error showing context menu: {ex.Message}");
        }
    }
}

public class RecyclerItemClickListener : Java.Lang.Object, RecyclerView.IOnItemTouchListener
{
    private readonly MyRecyclerView _recyclerView;
    private readonly GestureDetector _gestureDetector;
    
    public RecyclerItemClickListener(MyRecyclerView recyclerView, GestureDetector gestureDetector)
    {
        _recyclerView = recyclerView;
        _gestureDetector = gestureDetector;
    }
    
    public bool OnInterceptTouchEvent(RecyclerView rv, MotionEvent e)
    {
        return _gestureDetector.OnTouchEvent(e);
    }
    
    public void OnRequestDisallowInterceptTouchEvent(bool disallowIntercept)
    {
        // No implementation needed
    }
    
    public void OnTouchEvent(RecyclerView rv, MotionEvent e)
    {
        // No implementation needed
    }
}

public class RecyclerViewGestureListener : GestureDetector.SimpleOnGestureListener
{
    private readonly MyRecyclerView _recyclerView;
    
    public RecyclerViewGestureListener(MyRecyclerView recyclerView)
    {
        _recyclerView = recyclerView;
    }
    
    public override void OnLongPress(MotionEvent? e)
    {
        if (e == null) return;
        
        System.Diagnostics.Debug.WriteLine("Long press detected in gesture listener");
        
        try
        {
            // Find the view under the touch point
            var childView = _recyclerView.FindChildViewUnder(e.GetX(), e.GetY());
            if (childView != null)
            {
                var position = _recyclerView.GetChildAdapterPosition(childView);
                if (position != RecyclerView.NoPosition)
                {
                    System.Diagnostics.Debug.WriteLine($"Long press on item at position: {position}");
                    _recyclerView.ShowContextMenuForPosition(position, childView);
                }
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in OnLongPress: {ex.Message}");
        }
    }
    
    public override bool OnSingleTapUp(MotionEvent? e)
    {
        // Allow normal tap handling to continue
        return false;
    }
}