using System;
using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms
{
	public abstract class BaseShellItem : NavigableElement, IPropertyPropagationController, IVisualController, IFlowDirectionController
	{
		internal static readonly BindableProperty ShellItemProperty =
			BindableProperty.Create("_ShellItem", typeof(BaseShellItem), typeof(BaseShellItem), default(BaseShellItem));

		internal readonly Item _item;
		private protected BaseShellItem(Item item)
		{
			_item = item ?? throw new ArgumentNullException(nameof(item));
			_item.SetValue(ShellItemProperty, this);
			item.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
		}

		public bool IsChecked => _item.IsChecked;
		public ImageSource Icon => _item.Icon;
		public string Route => _item.Route;
		public string Title => _item.Title;
		public bool IsEnabled => _item.IsEnabled;

		//Overriding those will allow renderers to access properties set on the Item as if they were properties
		//of the BaseShellItem themselves. We do not care (much) about overriding SetBinding() as this type is not
		//meant to be used by the users.
		public new void SetValue(BindableProperty property, object value) => _item.SetValue(property, value);
		public new void SetValue(BindablePropertyKey propertyKey, object value) => _item.SetValue(propertyKey, value);
		public new object GetValue(BindableProperty property) => _item.GetValue(property);
		public new void SetValueFromRenderer(BindableProperty property, object value) => _item.SetValueFromRenderer(property, value);



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