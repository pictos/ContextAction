using Android.Content;
using Android.Views;
using Microsoft.Maui.Controls.Handlers.Items;

namespace ContextMenu;

public class MyRecyclerView : MauiRecyclerView<ReorderableItemsView, GroupableItemsViewAdapter<ReorderableItemsView, IGroupableItemsViewSource>, IGroupableItemsViewSource>
{
    public MyRecyclerView(Context context, Func<IItemsLayout> getItemsLayout, Func<GroupableItemsViewAdapter<ReorderableItemsView, IGroupableItemsViewSource>> getAdapter) : base(context, getItemsLayout, getAdapter)
    {
    }


    protected override void OnCreateContextMenu(IContextMenu? menu)
    {
        if (menu is null)
        {
            return;
        }
        
        
        menu.SetHeaderTitle("This is the header");
        menu.Add("FirstItem");
        menu.Add("seconde Item");
    }
    
}