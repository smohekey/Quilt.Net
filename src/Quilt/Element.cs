namespace Quilt {
	using System.ComponentModel;

	public abstract class Element : INotifyPropertyChanged {
		internal const string INVOKE_PROPERTY_CHANGED_NAME = nameof(InvokePropertyChanged);

		protected Application Application { get; }

		protected Element(Application application) {
			Application = application;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected void InvokePropertyChanged(string name) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
