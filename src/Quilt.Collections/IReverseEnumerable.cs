using System.Collections.Generic;

namespace Quilt.Collections {
	public interface IReverseEnumerable<T> {
		IEnumerator<T> GetReverseEnumerator();
	}
}
