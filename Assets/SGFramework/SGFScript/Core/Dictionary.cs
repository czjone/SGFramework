namespace SGF.Core {

	using System.Collections.Generic;
	using System.Collections;
	using UnityEngine;

	/// <summary>
	/// thread safety dictionary.
	/// </summary>
	/// <typeparam name="K">key type</typeparam>
	/// <typeparam name="V">v type</typeparam>
	public class TDictionary<K, V> {

		private Hashtable hash;

		public delegate void Action<K, V> (K key, V val);

		public TDictionary () {
			hash = Hashtable.Synchronized (new Hashtable ());
		}

		public void Add (K key, V value) {
			this.hash[key] = value;
		}

		public V GetValue (K key) {
			if (this.hash.Contains (key) == false) return default (V);
			return (V) this.hash[key];
		}

		public void SetValue (K key, V value) {
			this.hash[key] = value;
		}

		public V this [K key] {
			get {
				return GetValue (key);
			}
		}

		public bool ContainsKey (K key) {
			return this.hash.ContainsKey (key);
		}

		public bool ContainsValue (V val) {
			return this.hash.ContainsValue (val);
		}

		public void Remove (K key) {
			this.hash.Remove (key);
		}

		public void Foreach (Action<K, V> action) {
			if (action == null) return;
			foreach (DictionaryEntry de in this.hash) {
				action.Invoke ((K) de.Key, (V) de.Value);
			}
		}
	}
}