namespace Xamarin.Forms
{
	//we probably could get rid of this class
	public abstract class ShellGroupItem : BaseShellItem
	{
		internal ShellGroupItem(Item item) : base(item)
		{
		}

		public FlyoutDisplayOptions FlyoutDisplayOptions => Item.FlyoutDisplayOptions;
	}
}