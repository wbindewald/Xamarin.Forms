using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;

using Xamarin.Forms.Internals;

namespace Xamarin.Forms
{
	public class ShellItem : ShellGroupItem, IShellItemController, IElementConfiguration<ShellItem>, IPropertyPropagationController
	{
		readonly ObservableCollection<Element> _children = new ObservableCollection<Element>();
		ReadOnlyCollection<Element> _logicalChildren;
		Lazy<PlatformConfigurationRegistry<ShellItem>> _platformConfigurationRegistry;

		public ShellSection CurrentItem => (ShellSection)Item.CurrentItem?.GetValue(ShellItemProperty);
		public ShellCollection<ShellSection> Sections { get; }

		internal ShellItem(Item item) : base(item)
		{
			Sections = new ShellCollection<ShellSection> { Inner = new ElementCollection<ShellSection>(_children) };
			((INotifyCollectionChanged)Sections).CollectionChanged += ItemsCollectionChanged;
			_platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<ShellItem>>(() => new PlatformConfigurationRegistry<ShellItem>(this));
		}

		Task IShellItemController.GoToPart(List<string> parts, Dictionary<string, string> queryData)
		{
			var shellSectionRoute = parts[0];

			var sections = Sections;
			for (int i = 0; i < sections.Count; i++)
			{
				var shellSection = sections[i];
				if (Routing.CompareRoutes(shellSection.Route, shellSectionRoute, out var isImplicit))
				{
					Shell.ApplyQueryAttributes(shellSection, queryData, parts.Count == 1);

					if (CurrentItem != shellSection)
						Item.SetValueFromRenderer(Item.CurrentItemProperty, shellSection.Item);
					if (!isImplicit)
						parts.RemoveAt(0);
					if (parts.Count > 0)
						return ((IShellSectionController)shellSection).GoToPart(parts, queryData);
					break;
				}
			}
			return Task.FromResult(true);
		}

		bool IShellItemController.ProposeSection(ShellSection shellSection, bool setValue)
		{
			var controller = (IShellController)Parent;

			if (controller == null)
				return false;

			bool accept = controller.ProposeNavigation(ShellNavigationSource.ShellSectionChanged,
				this,
				shellSection,
				shellSection?.CurrentItem,
				shellSection.Stack,
				true
			);

			if (accept && setValue)
				Item.SetValueFromRenderer(Item.CurrentItemProperty, shellSection.Item);

			return accept;
		}

		void IPropertyPropagationController.PropagatePropertyChanged(string propertyName)
			=> PropertyPropagationExtensions.PropagatePropertyChanged(propertyName, this, Sections);
			

		internal override ReadOnlyCollection<Element> LogicalChildrenInternal
			=> _logicalChildren ?? (_logicalChildren = new ReadOnlyCollection<Element>(_children));

		internal void SendStructureChanged()
		{
			if (Parent is Shell shell)
				shell.SendStructureChanged();
		}

		public IPlatformElementConfiguration<T, ShellItem> On<T>() where T : IConfigPlatform
			=> _platformConfigurationRegistry.Value.On<T>();

		protected override void OnChildAdded(Element child)
		{
			base.OnChildAdded(child);
			if (CurrentItem == null)
				Item.SetValueFromRenderer(Item.CurrentItemProperty, (child as BaseShellItem)?.Item);
		}

		protected override void OnChildRemoved(Element child)
		{
			base.OnChildRemoved(child);
			if (CurrentItem != child)
				return;

			if (Sections.Count == 0)
				Item.ClearValue(Item.CurrentItemProperty);
			else
				Item.SetValueFromRenderer(Item.CurrentItemProperty, Sections[0].Item);
		}

		static void OnCurrentItemChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var shellItem = (ShellItem)bindable;

			if (shellItem.Parent is IShellController shell)
				shell.UpdateCurrentState(ShellNavigationSource.ShellSectionChanged);

			shellItem.SendStructureChanged();
			((IShellController)shellItem?.Parent)?.AppearanceChanged(shellItem, false);
		}

		void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
				foreach (Element element in e.NewItems)
					OnChildAdded(element);

			if (e.OldItems != null)
				foreach (Element element in e.OldItems)
					OnChildRemoved(element);

			SendStructureChanged();
		}

//#if DEBUG
//		[Obsolete("Please dont use this in core code... its SUPER hard to debug when this happens", true)]
//#endif
//		public static implicit operator ShellItem(ShellSection shellSection)
//		{
//			var result = new ShellItem {
//				Route = Routing.GenerateImplicitRoute(shellSection.Route),
//				Items = { shellSection },
//			};

//			result.SetBinding(TitleProperty, new Binding(nameof(Title), BindingMode.OneWay, source: shellSection));
//			result.SetBinding(IconProperty, new Binding(nameof(Icon), BindingMode.OneWay, source: shellSection));
//			result.SetBinding(FlyoutDisplayOptionsProperty, new Binding(nameof(FlyoutDisplayOptions), BindingMode.OneTime, source: shellSection));
//			return result;
//		}

//#if DEBUG
//		[Obsolete("Please dont use this in core code... its SUPER hard to debug when this happens", true)]
//#endif
//		public static implicit operator ShellItem(ShellContent shellContent)
//			=> (ShellSection)shellContent;

//#if DEBUG
//		[Obsolete("Please dont use this in core code... its SUPER hard to debug when this happens", true)]
//#endif
//		public static implicit operator ShellItem(TemplatedPage page)
//			=> (ShellSection)(ShellContent)page;

//#if DEBUG
//		[Obsolete("Please dont use this in core code... its SUPER hard to debug when this happens", true)]
//#endif
//		public static implicit operator ShellItem(MenuItem menuItem)
//			=> new MenuShellItem(menuItem);

	}
}