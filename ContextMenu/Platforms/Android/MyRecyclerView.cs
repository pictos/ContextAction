using Android.App;
using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Controls.Handlers.Items;

namespace ContextMenu;

class MyCVHandler : CollectionViewHandler
{
    protected override RecyclerView CreatePlatformView()
    {
        return new MyRecyclerView(Context, GetItemsLayout, CreateAdapter);
    }

    protected override ReorderableItemsViewAdapter<ReorderableItemsView, IGroupableItemsViewSource> CreateAdapter()
    {
        return new MyViewAdapter(VirtualView);
    }
}

public class MyRecyclerView : MauiRecyclerView<ReorderableItemsView, GroupableItemsViewAdapter<ReorderableItemsView, IGroupableItemsViewSource>, IGroupableItemsViewSource>
{
    public static int SelectedPosition { get; set; } = -1;

    public MyRecyclerView(Context context, Func<IItemsLayout> getItemsLayout, Func<GroupableItemsViewAdapter<ReorderableItemsView, IGroupableItemsViewSource>> getAdapter) : base(context, getItemsLayout, getAdapter)
    {

    }
}

public class MyViewAdapter : ReorderableItemsViewAdapter<ReorderableItemsView, IGroupableItemsViewSource>
{
    public MyViewAdapter(ReorderableItemsView reorderableItemsView, Func<Microsoft.Maui.Controls.View, Context, ItemContentView>? createView = null) : base(reorderableItemsView, createView)
    {
    }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        base.OnBindViewHolder(holder, position);

        // Store the position in the ViewHolder for context menu
        holder.ItemView.Tag = position;

        // Register EACH individual item view for context menu (this is key!)
        MainActivity.Instance.RegisterForContextMenu(holder.ItemView);
        
        System.Diagnostics.Debug.WriteLine($"Registered item view for floating context menu at position: {position}");
    }

    public override void OnViewRecycled(Java.Lang.Object holder)
    {
        base.OnViewRecycled(holder);
        
        // Clean up context menu registration when view is recycled
        if (holder is RecyclerView.ViewHolder viewHolder)
        {
            MainActivity.Instance.UnregisterForContextMenu(viewHolder.ItemView);
        }
    }
}

