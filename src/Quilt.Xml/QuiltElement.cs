namespace Quilt.Xml {
	using System.ComponentModel;

	public abstract class QuiltElement : INotifyPropertyChanged {
		internal const string INVOKE_PROPERTY_CHANGED_NAME = nameof(InvokePropertyChanged);

		public new QuiltDocument OwnerDocument => (QuiltDocument)base.OwnerDocument;
		protected Element(Application application) {
			Application = application;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected void InvokePropertyChanged(string name) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
