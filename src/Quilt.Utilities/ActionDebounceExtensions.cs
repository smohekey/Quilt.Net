
namespace Quilt.Utilities {
	using System;
	using System.Threading;
	using System.Threading.Tasks;


	/// <summary>
	///   Based on Mike Ward's answer here: https://stackoverflow.com/questions/28472205/c-sharp-event-debounce
	/// </summary>
	public static class ActionDebounceExtensions {
		public static Action Debounce(this Action @this, TimeSpan timeout) {
			CancellationTokenSource? cts = null;

			return () => {
				cts?.Cancel();
				cts = new CancellationTokenSource();

				Task.Delay(timeout, cts.Token).ContinueWith(t => {
					if(t.IsCompletedSuccessfully) {
						@this();
					}
				}, TaskScheduler.Default);
			};
		}

		public static Action<T0> Debounce<T0>(this Action<T0> @this, TimeSpan timeout) {
			CancellationTokenSource? cts = null;

			return (arg0) => {
				cts?.Cancel();
				cts = new CancellationTokenSource();

				Task.Delay(timeout, cts.Token).ContinueWith(t => {
					if(t.IsCompletedSuccessfully) {
						@this(arg0);
					}
				}, TaskScheduler.Default);
			};
		}

		public static Action<T0, T1> Debounce<T0, T1>(this Action<T0, T1> @this, TimeSpan timeout) {
			CancellationTokenSource? cts = null;

			return (arg0, arg1) => {
				cts?.Cancel();
				cts = new CancellationTokenSource();

				Task.Delay(timeout, cts.Token).ContinueWith(t => {
					if(t.IsCompletedSuccessfully) {
						@this(arg0, arg1);
					}
				}, TaskScheduler.Default);
			};
		}

		public static Action<T0, T1, T2> Debounce<T0, T1, T2>(this Action<T0, T1, T2> @this, TimeSpan timeout) {
			CancellationTokenSource? cts = null;

			return (arg0, arg1, arg2) => {
				cts?.Cancel();
				cts = new CancellationTokenSource();

				Task.Delay(timeout, cts.Token).ContinueWith(t => {
					if(t.IsCompletedSuccessfully) {
						@this(arg0, arg1, arg2);
					}
				}, TaskScheduler.Default);
			};
		}

		public static Action<T0, T1, T2, T3> Debounce<T0, T1, T2, T3>(this Action<T0, T1, T2, T3> @this, TimeSpan timeout) {
			CancellationTokenSource? cts = null;

			return (arg0, arg1, arg2, arg3) => {
				cts?.Cancel();
				cts = new CancellationTokenSource();

				Task.Delay(timeout, cts.Token).ContinueWith(t => {
					if(t.IsCompletedSuccessfully) {
						@this(arg0, arg1, arg2, arg3);
					}
				}, TaskScheduler.Default);
			};
		}

		public static Action<T0, T1, T2, T3, T4> Debounce<T0, T1, T2, T3, T4>(this Action<T0, T1, T2, T3, T4> @this, TimeSpan timeout) {
			CancellationTokenSource? cts = null;

			return (arg0, arg1, arg2, arg3, arg4) => {
				cts?.Cancel();
				cts = new CancellationTokenSource();

				Task.Delay(timeout, cts.Token).ContinueWith(t => {
					if(t.IsCompletedSuccessfully) {
						@this(arg0, arg1, arg2, arg3, arg4);
					}
				}, TaskScheduler.Default);
			};
		}

		public static Action<T0, T1, T2, T3, T4, T5> Debounce<T0, T1, T2, T3, T4, T5>(this Action<T0, T1, T2, T3, T4, T5> @this, TimeSpan timeout) {
			CancellationTokenSource? cts = null;

			return (arg0, arg1, arg2, arg3, arg4, arg5) => {
				cts?.Cancel();
				cts = new CancellationTokenSource();

				Task.Delay(timeout, cts.Token).ContinueWith(t => {
					if(t.IsCompletedSuccessfully) {
						@this(arg0, arg1, arg2, arg3, arg4, arg5);
					}
				}, TaskScheduler.Default);
			};
		}

		public static Action<T0, T1, T2, T3, T4, T5, T6> Debounce<T0, T1, T2, T3, T4, T5, T6>(this Action<T0, T1, T2, T3, T4, T5, T6> @this, TimeSpan timeout) {
			CancellationTokenSource? cts = null;

			return (arg0, arg1, arg2, arg3, arg4, arg5, arg6) => {
				cts?.Cancel();
				cts = new CancellationTokenSource();

				Task.Delay(timeout, cts.Token).ContinueWith(t => {
					if(t.IsCompletedSuccessfully) {
						@this(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
					}
				}, TaskScheduler.Default);
			};
		}

		public static Action<T0, T1, T2, T3, T4, T5, T6, T7> Debounce<T0, T1, T2, T3, T4, T5, T6, T7>(this Action<T0, T1, T2, T3, T4, T5, T6, T7> @this, TimeSpan timeout) {
			CancellationTokenSource? cts = null;

			return (arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7) => {
				cts?.Cancel();
				cts = new CancellationTokenSource();

				Task.Delay(timeout, cts.Token).ContinueWith(t => {
					if(t.IsCompletedSuccessfully) {
						@this(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
					}
				}, TaskScheduler.Default);
			};
		}

	}
}