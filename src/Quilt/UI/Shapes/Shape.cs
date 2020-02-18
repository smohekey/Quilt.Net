namespace Quilt.UI.Shapes {
	[QuiltElement(Namespace.URI)]
	public abstract class Shape : Element {
		[QuiltAttribute]
		public virtual float Left { get; set; }

		[QuiltAttribute]
		public virtual float Top { get; set; }

		[QuiltAttribute]
		public virtual float Width { get; set; }

		[QuiltAttribute]
		public virtual float Height { get; set; }

		protected Shape(Application application) : base(application) {

		}
	}
}
