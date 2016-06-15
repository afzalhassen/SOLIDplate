using System.Collections.Generic;

namespace SOLIDplate.Presentation.ViewModels.Interfaces
{
    public interface IApplicationDataContext
	{
		IDictionary<string, dynamic> CurrentContext { get; }
	}
}
