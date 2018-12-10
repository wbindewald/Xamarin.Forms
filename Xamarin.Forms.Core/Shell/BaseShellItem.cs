using System;
using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms
{
	public class BaseShellItem : NavigableElement, IPropertyPropagationController, IVisualController, IFlowDirectionController
	{
		internal static readonly BindablePropertyKey IsCheckedPropertyKey =
			BindableProperty.CreateReadOnly(nameof(IsChecked), typeof(bool), typeof(BaseShellItem), false);

		public static readonly BindableProperty IsCheckedProperty = IsCheckedPropertyKey.BindableProperty;

		public static readonly BindableProperty IconProperty =
			BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(BaseShellItem), null, BindingMode.OneWay);

		public static readonly BindableProperty IsEnabledProperty =
			BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(BaseShellItem), true, BindingMode.OneWay);

		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(nameof(Title), typeof(string), typeof(BaseShellItem), null, BindingMode.OneTime);

		public bool IsChecked => (bool)GetValue(IsCheckedProperty);

		public ImageSource Icon
		{
			get => (ImageSource)GetValue(IconProperty);
			set => SetValue(IconProperty, value);
		}

		public bool IsEnabled
		{
			get => (bool)GetValue(IsEnabledProperty);
			set => SetValue(IsEnabledProperty, value);
		}

		public string Route
		{
			get => Routing.GetRoute(this);
			set => Routing.SetRoute(this, value);
		}

		public string Title
		{
			get => (string)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		IVisual _effectiveVisual = Xamarin.Forms.VisualMarker.Default;
		IVisual IVisualController.EffectiveVisual {
			get => _effectiveVisual;
			set {
				_effectiveVisual = value;
				OnPropertyChanged(VisualElement.VisualProperty.PropertyName);
			}
		}
		IVisual IVisualController.Visual => Xamarin.Forms.VisualMarker.MatchParent;

		void IPropertyPropagationController.PropagatePropertyChanged(string propertyName) => PropertyPropagationExtensions.PropagatePropertyChanged(propertyName, this, LogicalChildren);

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