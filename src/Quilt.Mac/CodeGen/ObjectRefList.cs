namespace Quilt.Mac.CodeGen {
  using System;
  using System.Collections.Generic;
  using System.Runtime.InteropServices;

	public class ObjectRefList {
		private readonly List<ObjectRef> _objectRefs = new List<ObjectRef>();
		private readonly int _objectRefOffset;
		private int _firstFree = -1;

		public ObjectRefList(int objectRefOffset) {
			_objectRefOffset = objectRefOffset;
		}
		
		public object GetObject(IntPtr nsObject) {
			var objectRef = Marshal.ReadInt32(nsObject, _objectRefOffset);
			var weakRef = _objectRefs[objectRef].Reference;

			return weakRef.IsAlive ? weakRef.Target : throw new NullReferenceException();
		}

		public void AddReference(IntPtr nsObject, object @object) {
			var objectRef = -1;

			if(_firstFree != -1) {
				objectRef = _firstFree;

				_firstFree = _objectRefs[_firstFree].NextFree;

				_objectRefs[objectRef] = new ObjectRef { Reference = new WeakReference(@object) };
			} else {
				objectRef = _objectRefs.Count;

				_objectRefs.Add(new ObjectRef { Reference = new WeakReference(@object) });
			}

			Marshal.WriteInt32(nsObject, _objectRefOffset, objectRef);
		}

		public void RemoveReference(int objectRef) {
			var nsObject = _objectRefs[objectRef].NSObject;

			_objectRefs[objectRef] = new ObjectRef { NextFree = _firstFree };

			Marshal.WriteInt32(nsObject, _objectRefOffset, -1);

			_firstFree = objectRef;
		}

		private struct ObjectRef {
			public int NextFree;
			public WeakReference Reference;
			public IntPtr NSObject;
		}
	}
}
