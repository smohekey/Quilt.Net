namespace Quilt.Mac.AppKit {
	using System;
  using Quilt.Mac.CodeGen;
  using Quilt.Mac.Foundation;
  using Quilt.Mac.ObjectiveC;

	public delegate void GlobalEventHandler(NSEvent @event);
	public delegate NSEvent LocalEventHandler(NSEvent @event);

	[Class]
	public abstract class NSEvent : NSObject<NSEvent, NSEvent.MetaClass> {
		protected NSEvent(IntPtr handle) : base(handle) {

		}

		public new abstract class MetaClass : NSObject<NSEvent, MetaClass>.MetaClass {
			protected MetaClass(Class @class) : base(@class) {

			}

			/// <summary>
			///		Installs an event monitor that receives copies of events posted to other applications.
			/// </summary>
			/// <remarks>
			///		Events are delivered asynchronously to your app and you can only observe the event; you cannot modify or otherwise prevent the event from being delivered to its original target application.
			///		Key-related events may only be monitored if accessibility is enabled or if your application is trusted for accessibility access (see AXIsProcessTrusted).
			///		Note that your handler will not be called for events that are sent to your own application.
			/// </remarks>
			/// <param name="mask">An event mask specifying which events you wish to monitor. <see cref="NSEventMask"/> for possible values.</param>
			/// <param name="handler">The event handler block object. It is passed the event to monitor. You are unable to change the event, merely observe it.</param>
			/// <returns>An event handler object.</returns>
			[Import]
			public abstract NSObject AddGlobalMonitorForEventsMatching(NSEventMask mask, GlobalEventHandler handler);

			/// <summary>
			///		Installs an event monitor that receives copies of events posted to this application before they are dispatched.
			/// </summary>
			/// <remarks>
			///		Your handler will not be called for events that are consumed by nested event-tracking loops such as control tracking, menu tracking, or window dragging; only events that are dispatched through the applications sendEvent: method will be passed to your handler.
			/// </remarks>
			/// <param name="mask">An event mask specifying which events you wish to monitor. <see cref="NSEventMask"/> for possible values.</param>
			/// <param name="handler">
			///		The event handler block object. It is passed the event to monitor.
			///		You can return the event unmodified, create and return a new NSEvent object, or return nil to stop the dispatching of the event.
			/// </param>
			/// <returns>An event handler object.</returns>
			[Import]
			public abstract NSObject AddLocalMonitorForEventsMatching(NSEventMask mask, LocalEventHandler handler);
		}
	}
}
