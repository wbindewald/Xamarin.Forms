using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Xamarin.Forms
{
	[ContentProperty(nameof(Items))]
	public class Item : NavigableElement
	{
		readonly ObservableCollection<Element> _children = new ObservableCollection<Element>();
		ReadOnlyCollection<Element> _logicalChildren;
		Lazy<PlatformConfigurationRegistry<Item>> _platformConfigurationRegistry;

		public static readonly BindableProperty CurrentItemProperty =
			BindableProperty.Create(nameof(CurrentItem), typeof(Item), typeof(Item), null, BindingMode.TwoWay,
				propertyChanged: OnCurrentItemChanged);

		internal static readonly BindablePropertyKey IsCheckedPropertyKey =
			BindableProperty.CreateReadOnly(nameof(IsChecked), typeof(bool), typeof(Item), false);

		public static readonly BindableProperty IsCheckedProperty = IsCheckedPropertyKey.BindableProperty;

		public static readonly BindableProperty IsEnabledProperty =
			BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(Item), true, BindingMode.OneWay);

		static readonly BindablePropertyKey ItemsPropertyKey =
			BindableProperty.CreateReadOnly(nameof(Items), typeof(ShellCollection<Item>), typeof(Item), null,
				defaultValueCreator: bo => new ShellCollection<Item> { Inner = new ElementCollection<Item>(((Item)bo)._children) });

		public static readonly BindableProperty ItemsProperty = ItemsPropertyKey.BindableProperty;

		//only valid on ShellItem, ShellSection
		public static readonly BindableProperty FlyoutDisplayOptionsProperty =
			BindableProperty.Create(nameof(FlyoutDisplayOptions), typeof(FlyoutDisplayOptions), typeof(Item), FlyoutDisplayOptions.AsSingleItem, BindingMode.OneTime);

		public static readonly BindableProperty IconProperty =
			BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(Item), null, BindingMode.OneWay);

		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(nameof(Title), typeof(string), typeof(Item), null, BindingMode.OneTime);

		public Item CurrentItem {
			get => (Item)GetValue(CurrentItemProperty);
			set => SetValue(CurrentItemProperty, value);
		}

		public bool IsChecked => (bool)GetValue(IsCheckedProperty);

		public bool IsEnabled {
			get => (bool)GetValue(IsEnabledProperty);
			set => SetValue(IsEnabledProperty, value);
		}

		public ShellCollection<Item> Items => (ShellCollection<Item>)GetValue(ItemsProperty);

		public FlyoutDisplayOptions FlyoutDisplayOptions {
			get => (FlyoutDisplayOptions)GetValue(FlyoutDisplayOptionsProperty);
			set => SetValue(FlyoutDisplayOptionsProperty, value);
		}

		public ImageSource Icon {
			get => (ImageSource)GetValue(IconProperty);
			set => SetValue(IconProperty, value);
		}

		public string Route {
			get => Routing.GetRoute(this);
			set => Routing.SetRoute(this, value);
		}

		public string Title {
			get => (string)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		public Item()
		{
			((INotifyCollectionChanged)Items).CollectionChanged += ItemsCollectionChanged;
			_platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<Item>>(() => new PlatformConfigurationRegistry<Item>(this));
		}


		static void OnCurrentItemChanged(BindableObject bindable, object oldValue, object newValue)
		{
			//var shellItem = (ShellItem)bindable;

			//if (shellItem.Parent is IShellController shell)
			//	shell.UpdateCurrentState(ShellNavigationSource.ShellSectionChanged);

			//shellItem.SendStructureChanged();
			//((IShellController)shellItem?.Parent)?.AppearanceChanged(shellItem, false);
		}

		void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			//if (e.NewItems != null)
			//	foreach (Element element in e.NewItems)
			//		OnChildAdded(element);

			//if (e.OldItems != null)
			//	foreach (Element element in e.OldItems)
			//		OnChildRemoved(element);

			//SendStructureChanged();
		}
	}
}