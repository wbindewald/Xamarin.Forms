using System;
using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms
{
	public abstract class BaseShellItem : NavigableElement, IPropertyPropagationController, IVisualController, IFlowDirectionController
	{
		internal static readonly BindableProperty ShellItemProperty =
			BindableProperty.Create("_ShellItem", typeof(BaseShellItem), typeof(BaseShellItem), default(BaseShellItem));

		public Item Item { get; }

		private protected BaseShellItem(Item item)
		{
			Item = item ?? throw new ArgumentNullException(nameof(item));
			Item.SetValue(ShellItemProperty, this);
			item.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
		}

		public bool IsChecked => Item.IsChecked;
		public ImageSource Icon => Item.Icon;
		public string Route => Item.Route;
		public string Title => Item.Title;
		public bool IsEnabled => Item.IsEnabled;

		//Overriding those will allow renderers to access properties set on the Item as if they were properties
		//of the BaseShellItem themselves. We do not care (much) about overriding SetBinding() as this type is not
		//meant to be used by the users.
		public new void SetValue(BindableProperty property, object value) => Item.SetValue(property, value);
		public new void SetValue(BindablePropertyKey propertyKey, object value) => Item.SetValue(propertyKey, value);
		public new object GetValue(BindableProperty property) => Item.GetValue(property);
		public new void SetValueFromRenderer(BindableProperty property, object value) => Item.SetValueFromRenderer(property, value);

		//TODO, review everything below this line
		IVisual _effectiveVisual = Xamarin.Forms.VisualMarker.Default;
		IVisual IVisualController.EffectiveVisual {
			get => _effectiveVisual;
			set {
				_effectiveVisual = value;
				OnPropertyChanged(VisualElement.VisualProperty.PropertyName);
			}
		}
		IVisual IVisualController.Visual => Xamarin.Forms.VisualMarker.MatchParent;

		void IPropertyPropagationController.PropagatePropertyChanged(string propertyName)
			=> PropertyPropagationExtensions.PropagatePropertyChanged(propertyName, this, LogicalChildren);

		EffectiveFlowDirection _effectiveFlowDirection = default(EffectiveFlowDirection);
		EffectiveFlowDirection IFlowDirectionController.EffectiveFlowDirection
		{
			get { return _effectiveFlowDirection; }
			set
			{
				if (value == _effectiveFlowDirection)
					return;

				_effectiveFlowDirection = value;

				var ve = (Parent as VisualElement);
				ve?.InvalidateMeasureInternal(InvalidationTrigger.Undefined);
				OnPropertyChanged(VisualElement.FlowDirectionProperty.PropertyName);
			}
		}

		bool IFlowDirectionController.ApplyEffectiveFlowDirectionToChildContainer => true;
		double IFlowDirectionController.Width => (Parent as VisualElement)?.Width ?? 0;

		internal virtual void ApplyQueryAttributes(IDictionary<string, string> query)
		{
		}
	}
}