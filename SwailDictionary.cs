using System;
using System.Collections;
using System.Collections.Generic;

namespace SwailSharp {
	public class SwailDictionary<K, V> : SwailObject, IDictionary<K, V> {
		public IDictionary<K, V> Wrapped;

		public SwailDictionary(string name) : base(name) {
			this.Wrapped = new Dictionary<K, V>();
		}

		public void Add(K key, V value) {
			this.Wrapped.Add(key, value);
		}
		
		public void Add(KeyValuePair<K, V> pair) {
			this.Wrapped.Add(pair);
		}
		
		public void Clear() {
			this.Wrapped.Clear();
		}

		public bool Remove(K key) {
			return this.Wrapped.Remove(key);
		}
		
		public bool Remove(KeyValuePair<K, V> pair) {
			return this.Wrapped.Remove(pair);
		}

		public bool ContainsKey(K key) {
			return this.Wrapped.ContainsKey(key);
		}
		
		public bool Contains(KeyValuePair<K, V> pair) {
			return this.Wrapped.Contains(pair);
		}

		public bool TryGetValue(K key, out V value) {
			return this.Wrapped.TryGetValue(key, out value);
		}
		
		IEnumerator IEnumerable.GetEnumerator() {
			return this.Wrapped.GetEnumerator();
		}
		
		public IEnumerator<KeyValuePair<K, V>> GetEnumerator() {
			return this.Wrapped.GetEnumerator();
		}
		
		public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) {
			this.Wrapped.CopyTo(array, arrayIndex);
		}
		
		public ICollection<K> Keys {
			get {
				return this.Wrapped.Keys;
			}
		}
		
		public ICollection<V> Values {
			get {
				return this.Wrapped.Values;
			}
		}
		
		public int Count {
			get {
				return this.Wrapped.Count;
			}
		}
		
		public bool IsReadOnly {
			get {
				return this.Wrapped.IsReadOnly;
			}
		}

		public V this[K key] {
			get {
				return this.Wrapped[key];
			}

			set {
				this.Wrapped[key] = value;
			}
		}
	}
}

