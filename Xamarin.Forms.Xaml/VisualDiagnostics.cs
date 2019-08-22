using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Xamarin.Forms.Xaml
{
	class VisualDiagnostics
	{
		//FIXME use weak refs
		static Dictionary<object, XamlSourceInfo> sourceInfos = new Dictionary<object, XamlSourceInfo>();
		internal static  void RegisterSourceInfo(object target, Uri uri, int lineNumber, int linePosition)
		{
			sourceInfos[target] = new XamlSourceInfo(uri, lineNumber, linePosition);
		}

		internal static void SendVisualTreeChanged(object parent, object child)
		{
			if (Debugger.IsAttached)
				VisualTreeChanged?.Invoke(parent, new VisualTreeChangeEventArgs(parent, child, -1, VisualTreeChangeType.Add));
		}

		public static event EventHandler<VisualTreeChangeEventArgs> VisualTreeChanged;
		public static XamlSourceInfo GetXamlSourceInfo(object obj) => sourceInfos[obj];
	}

	class XamlSourceInfo
	{
		public XamlSourceInfo(Uri sourceUri, int lineNumber, int linePosition)
		{
			SourceUri = sourceUri;
			LineNumber = lineNumber;
			LinePosition = linePosition;
		}

		public Uri SourceUri { get; }
		public int LineNumber { get; }
		public int LinePosition { get; }
	}

	class VisualTreeChangeEventArgs : EventArgs
	{
		public VisualTreeChangeEventArgs(object parent, object child, int childIndex, VisualTreeChangeType changeType)
		{
			Parent = parent;
			Child = child;
			ChildIndex = childIndex;
			ChangeType = changeType;
		}

		public object Parent { get; }
		public object Child { get; }
		public int ChildIndex { get; }
		public VisualTreeChangeType ChangeType { get; }
	}

	enum VisualTreeChangeType
	{
		Add = 0,
		Remove = 1
	}
}
