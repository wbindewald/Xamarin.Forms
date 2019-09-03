using System;
using System.ComponentModel;
using Android.Content;
using Android.Views;
using FormsCollectionView = Xamarin.Forms.CollectionView;

namespace Xamarin.Forms.Platform.Android
{
	public class CarouselViewRenderer : ItemsViewRenderer<ItemsView, ItemsViewAdapter<ItemsView, IItemsViewSource>, IItemsViewSource>
	{
		protected CarouselView Carousel;
		IItemsLayout _layout;
		ItemDecoration _itemDecoration;
		bool _isSwipeEnabled;
		bool _isUpdatingPositionFromForms;
		int _oldPosition;
		int _initialPosition;
		bool _scrollingToInitialPosition = true;

		public CarouselViewRenderer(Context context) : base(context)
		{
			FormsCollectionView.VerifyCollectionViewFlagEnabled(nameof(CarouselViewRenderer));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if(_itemDecoration != null)
				{
					_itemDecoration.Dispose();
					_itemDecoration = null;
				}

                _layout = null;
            }

			base.Dispose(disposing);
		}

		protected override void SetUpNewElement(ItemsView newElement)
		{
			base.SetUpNewElement(newElement);

			if (newElement == null)
			{
				Carousel = null;
				return;
			}

			Carousel = newElement as CarouselView;
			_layout = ItemsView.ItemsLayout;

			UpdateIsSwipeEnabled();
			UpdateInitialPosition();
			UpdateItemSpacing();
		}

		protected override void UpdateItemsSource()
		{
			// By default the CollectionViewAdapter creates the items at whatever size the template calls for
			// But for the Carousel, we want it to create the items to fit the width/height of the viewport
			// So we give it an alternate delegate for creating the views
			ItemsViewAdapter = new ItemsViewAdapter<ItemsView, IItemsViewSource>(ItemsView,
				(view, context) => new SizedItemContentView(Context, GetItemWidth, GetItemHeight));

			SwapAdapter(ItemsViewAdapter, false);
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs changedProperty)
		{
			if (changedProperty.Is(CarouselView.PeekAreaInsetsProperty))
				Tracker?.UpdateLayout();
			else if (changedProperty.Is(CarouselView.IsSwipeEnabledProperty))
				UpdateIsSwipeEnabled();
			else if (changedProperty.Is(CarouselView.IsBounceEnabledProperty))
				UpdateIsBounceEnabled();
			else if (changedProperty.Is(ListItemsLayout.ItemSpacingProperty))
				UpdateItemSpacing();
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			// TODO: This doesn't work because we need to interact with the Views
			if (!_isSwipeEnabled)
			{
				return false;
			}
			return base.OnTouchEvent(e);
		}

		public override void OnScrollStateChanged(int state)
		{
			base.OnScrollStateChanged(state);

			if (_isSwipeEnabled)
			{
				if (state == ScrollStateDragging)
					Carousel.SetIsDragging(true);
				else
					Carousel.SetIsDragging(false);
			}
		}

		public override void OnScrolled(int dx, int dy)
		{
			base.OnScrolled(dx, dy);

			UpdatePositionFromScroll();
		}

		protected override ItemDecoration CreateSpacingDecoration(IItemsLayout itemsLayout)
		{
			return new CarouselSpacingItemDecoration(itemsLayout);
		}

		protected override void UpdateItemSpacing()
		{
			if (_layout == null)
			{
				return;
			}

			if (_itemDecoration != null)
			{
				RemoveItemDecoration(_itemDecoration);
			}

			_itemDecoration = CreateSpacingDecoration(_layout);
			AddItemDecoration(_itemDecoration);

			base.UpdateItemSpacing();
		}

		int GetItemWidth()
		{
			var itemWidth = Width;

			if (_layout is ListItemsLayout listItemsLayout && listItemsLayout.Orientation == ItemsLayoutOrientation.Horizontal)
			{
				var numberOfVisibleItems = Carousel.NumberOfSideItems * 2 + 1;
				itemWidth = (int)(Width - Carousel.PeekAreaInsets.Left - Carousel.PeekAreaInsets.Right) / numberOfVisibleItems;
			}

			return itemWidth;
		}

		int GetItemHeight()
		{
			var itemHeight = Height;

			if (_layout is ListItemsLayout listItemsLayout && listItemsLayout.Orientation == ItemsLayoutOrientation.Vertical)
			{
				var numberOfVisibleItems = Carousel.NumberOfSideItems * 2 + 1;
				itemHeight = (int)(Height - Carousel.PeekAreaInsets.Top - Carousel.PeekAreaInsets.Bottom) / numberOfVisibleItems;
			}

			return itemHeight;
		}

		void UpdateIsSwipeEnabled()
		{
			_isSwipeEnabled = Carousel.IsSwipeEnabled;
		}

		void UpdatePosition(int position)
		{
			if (position == -1 || _isUpdatingPositionFromForms)
				return;

			var context = ItemsViewAdapter?.ItemsSource.GetItem(position);

			if (context == null)
				throw new InvalidOperationException("Visible item not found");

			Carousel.SetCurrentItem(context);
		}

		void UpdateIsBounceEnabled()
		{
			OverScrollMode = Carousel.IsBounceEnabled ? OverScrollMode.Always : OverScrollMode.Never;
		}

		void UpdatePositionFromScroll()
		{
			var snapHelper = GetSnapManager()?.GetCurrentSnapHelper();

			if (snapHelper == null)
				return;

			var layoutManager = GetLayoutManager() as LayoutManager;

			var snapView = snapHelper.FindSnapView(layoutManager);

			if (snapView != null)
			{
				int middleCenterPosition = layoutManager.GetPosition(snapView);
				if (_scrollingToInitialPosition)
				{
					_scrollingToInitialPosition = !(_initialPosition == middleCenterPosition);
					return;
				}

				if (_oldPosition != middleCenterPosition)
				{
					_oldPosition = middleCenterPosition;
					UpdatePosition(middleCenterPosition);
				}
			}
		}

		void UpdateInitialPosition()
		{
			_isUpdatingPositionFromForms = true;
			//Goto to the Correct Position
			_initialPosition = Carousel.Position;
			Carousel.ScrollTo(_initialPosition, position: Xamarin.Forms.ScrollToPosition.Center);
			_isUpdatingPositionFromForms = false;
		}
	}
}