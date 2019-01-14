namespace SGF.Core
{
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    // List<T>
    [Serializable]
	public class Serialization<T> {
		[SerializeField]
		List<T> target;
		public List<T> ToList () { return target; }

		public Serialization (List<T> target) {
			this.target = target;
		}
	}

	// Dictionary<TKey, TValue>
	[Serializable]
	public class Serialization<TKey, TValue> : ISerializationCallbackReceiver {

		public delegate void EachAction (TKey key, TValue vale);

		[SerializeField]
		List<TKey> keys;
		[SerializeField]
		List<TValue> values;

		Dictionary<TKey, TValue> target;
		public Dictionary<TKey, TValue> ToDictionary () { return target; }

		public Serialization () {
			this.target = new Dictionary<TKey, TValue> ();
		}

		public Serialization (Dictionary<TKey, TValue> target) {
			this.target = target;
		}

		public void OnBeforeSerialize () {
			keys = new List<TKey> (target.Keys);
			values = new List<TValue> (target.Values);
		}

		public void OnAfterDeserialize () {
			var count = Math.Min (keys.Count, values.Count);
			target = new Dictionary<TKey, TValue> (count);
			for (var i = 0; i < count; ++i) {
				target.Add (keys[i], values[i]);
			}
		}

		public int Count {
			get {
				return this.keys.Count;
			}
		}

		public void Foreach (EachAction action) {
			if (action == null) return;
			foreach (var item in this.target) {
				action.Invoke (item.Key, item.Value);
			}
		}

		public bool ContainsKey (TKey key) {
			return this.target.ContainsKey (key);
		}

		public void Add (TKey key, TValue val) {
			this.target.Add (key, val);
		}
	}
}