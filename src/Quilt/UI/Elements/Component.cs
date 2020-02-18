namespace Quilt.UI.Elements {
	public abstract class Component : Element {
		[QuiltAttribute]
		public virtual float Left { get; set; }

		[QuiltAttribute]
		public virtual float Top { get; set; }

		[QuiltAttribute]
		public virtual float Width { get; set; }

		[QuiltAttribute]
		public virtual float Height { get; set; }

		protected Component(Application application) : base(application) {

		}

		//public abstract void Render(Context context);
	}
}
