using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Shell), typeof(ShellRenderer))]

[assembly: ExportRenderer(typeof(NativeViewWrapper), typeof(NativeViewWrapperRenderer))]
[assembly: ExportCell(typeof(Cell), typeof(CellRenderer))]
[assembly: ExportCell(typeof(EntryCell), typeof(EntryCellRenderer))]
[assembly: ExportCell(typeof(SwitchCell), typeof(SwitchCellRenderer))]
[assembly: ExportCell(typeof(TextCell), typeof(TextCellRenderer))]
[assembly: ExportCell(typeof(ImageCell), typeof(ImageCellRenderer))]
[assembly: ExportCell(typeof(ViewCell), typeof(ViewCellRenderer))]
[assembly: ExportImageSourceHandler(typeof(FileImageSource), typeof(FileImageSourceHandler))]
[assembly: ExportImageSourceHandler(typeof(StreamImageSource), typeof(StreamImagesourceHandler))]
[assembly: ExportImageSourceHandler(typeof(UriImageSource), typeof(ImageLoaderSourceHandler))]
[assembly: ExportImageSourceHandler(typeof(FontImageSource), typeof(FontImageSourceHandler))]
[assembly: Xamarin.Forms.Dependency(typeof(Deserializer))]
[assembly: Xamarin.Forms.Dependency(typeof(ResourcesProvider))]
[assembly: Preserve]
[assembly: InternalsVisibleTo("Xamarin.Forms.Platform")]
[assembly: InternalsVisibleTo("Xamarin.Forms.Material")]