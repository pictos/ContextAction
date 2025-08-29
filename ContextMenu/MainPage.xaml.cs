#if IOS
using CoreGraphics;
using UIKit;
#elif ANDROID
using Android.Views;
using Android.Widget;
#elif WINDOWS
using Microsoft.UI.Xaml;
#endif



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

		//cv.ItemsSource = list;

		img.HandlerChanged += Img_HandlerChanged;
	}

	void Img_HandlerChanged(object? sender, EventArgs e)
	{
		if (img.Handler is null)
			return;

#if IOS
		var platformView = (UIKit.UIImageView)img.Handler.PlatformView!;
		var interaction = new UIContextMenuInteraction(new InteractionDelegate(platformView));
		platformView.AddInteraction(interaction);
#elif ANDROID
		var platformView = (ImageView)img.Handler.PlatformView!;
		platformView.SetOnCreateContextMenuListener(new MyContextMenuListener());
#elif WINDOWS
		var platformView = (FrameworkElement)img.Handler.PlatformView!;
		platformView.ContextFlyout = CreateMenu();


		static Microsoft.UI.Xaml.Controls.MenuFlyout CreateMenu()
		{
			var myContextMenu = new Microsoft.UI.Xaml.Controls.MenuFlyout();
			var items = myContextMenu.Items;

			var item1 = new Microsoft.UI.Xaml.Controls.MenuFlyoutItem
			{
				Text = "Option 1"
			};

			item1.Click += (s, e) => System.Diagnostics.Debug.WriteLine("Option 1 clicked");

			var item2 = new Microsoft.UI.Xaml.Controls.MenuFlyoutItem
			{
				Text = "Option 2"
			};

			item2.Click += (s, e) => System.Diagnostics.Debug.WriteLine("Option 2 clicked");

			items.Add(item1);
			items.Add(item2);

			return myContextMenu;
		}

#endif
	}
}

#if IOS
class InteractionDelegate : UIContextMenuInteractionDelegate
{
	private readonly UIImageView image;

	public InteractionDelegate(UIImageView imageView)
	{
		this.image = imageView;
	}

	public override UIContextMenuConfiguration? GetConfigurationForMenu(UIContextMenuInteraction interaction, CGPoint location)
	{
		var edit = UIAction.Create("editar", UIImage.ActionsImage, "1", (action) => { Log(0); });
		var send = UIAction.Create("enviar", UIImage.ActionsImage, "2", (action) => { Log(1); });

		var createMenu = UIMenu.Create([edit, send]);

		return UIContextMenuConfiguration.Create(null, new(UIContextMenuContentPreviewProvider), (x) => createMenu);
	}

	UIViewController UIContextMenuContentPreviewProvider()
	{
		var vc = new UIViewController();

		var uiImage = new UIImage("dotnet_bot.png");
		var imageView = new UIImageView(uiImage);
		imageView.ContentMode = UIViewContentMode.Bottom;
		vc.View = imageView;

		var frame = image.Frame.Size;

		imageView.Frame = new CGRect(0, 0, frame.Width * 1.75, frame.Height * 2.75);
		vc.PreferredContentSize = imageView.Frame.Size;
		vc.View.BackgroundColor = UIColor.Clear;


		return vc;
	}

	static void Log(int id)
	{
		Console.WriteLine("###############");
		Console.WriteLine("###############");
		Console.WriteLine($"Pressed {id}");
		Console.WriteLine("###############");
		Console.WriteLine("###############");
	}
}
#elif ANDROID
class MyContextMenuListener : Java.Lang.Object, Android.Views.View.IOnCreateContextMenuListener
{
	public void OnCreateContextMenu(IContextMenu? menu, Android.Views.View? v, IContextMenuContextMenuInfo? menuInfo)
	{
		if (menu == null || v == null)
			return;


		//menu.SetHeaderTitle($"Item {position}");

		var editItem = menu.Add(0, 1, 0, "Edit Item")!;
		var deleteItem = menu.Add(0, 2, 1, "Delete Item")!;
		var shareItem = menu.Add(0, 3, 2, "Share Item")!;
		var addItem = menu.Add(0, 4, 3, "Add Item")!;


		editItem.SetOnMenuItemClickListener(new MenuItemClickListener("Edit"));
		deleteItem.SetOnMenuItemClickListener(new MenuItemClickListener("Delete"));
		shareItem.SetOnMenuItemClickListener(new MenuItemClickListener("Share"));
		addItem.SetOnMenuItemClickListener(new MenuItemClickListener("Add"));
	}

	sealed class MenuItemClickListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
	{
		private readonly string action;

		public MenuItemClickListener(string action)
		{
			this.action = action;
		}
		public bool OnMenuItemClick(IMenuItem item)
		{
			if (item == null) return false;

			System.Diagnostics.Debug.WriteLine($"{action} selected for item");

			// Handle the action based on the type
			switch (action)
			{
				case "Edit":
					// Handle edit action
					System.Diagnostics.Debug.WriteLine($"Handling edit for");
					break;
				case "Delete":
					// Handle delete action
					System.Diagnostics.Debug.WriteLine($"Handling delete for");
					break;
				case "Share":
					// Handle share action
					System.Diagnostics.Debug.WriteLine($"Handling share for");
					break;
				case "Add":
					// Handle add action
					System.Diagnostics.Debug.WriteLine($"Handling add for");
					break;
			}

			return true; // Consume the click event
		}
	}
}
#endif