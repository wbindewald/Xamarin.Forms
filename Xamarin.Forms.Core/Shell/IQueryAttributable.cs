using System.Collections.Generic;

namespace Xamarin.Forms
{
	public interface IQueryAttributable
	{
		void ApplyQueryAttributes(IDictionary<string, string> query);
	}
}