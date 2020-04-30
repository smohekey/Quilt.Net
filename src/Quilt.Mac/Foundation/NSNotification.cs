namespace Quilt.Mac.Foundation {
	using System;
	using Quilt.Mac.CodeGen;
	using Quilt.Mac.ObjectiveC;

	/// <summary>
	/// A container for information broadcast through a notification center to all registered observers.
	/// </summary>
	[Class]
	public abstract class NSNotification : NSObject<NSNotification, NSNotification.MetaClass> {
		protected NSNotification(IntPtr handle) : base(handle) {

		}

		/// <summary>
		/// The name of the notification.
		/// 
		/// Typically you use this property to find out what kind of notification you are dealing with when you receive a notification.
		/// 
		/// Notification names can be any string. To avoid name collisions, you might want to use a prefix that’s specific to your application.
		/// </summary>
		[Import]
		public abstract NSString Name { get; }

		/// <summary>
		/// The object associated with the notification.
		/// 
		/// This is often the object that posted this notification. It may be null.
		///
		/// Typically you use this method to find out what object a notification applies to when you receive a notification.
		/// </summary>
		[Import]
		public abstract NSObject Object { get; }

		/// <summary>
		/// The user information dictionary associated with the notification.
		/// 
		/// May be null.
		///
		/// The user information dictionary stores any additional objects that objects receiving the notification might use.
		/// </summary>
		[Import]
		public abstract NSDictionary UserInfo { get; }

		public new abstract class MetaClass : NSObject<NSNotification, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}
		}
	}
}
