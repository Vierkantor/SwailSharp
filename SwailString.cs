using System;

namespace SwailSharp {
	public class SwailString : SwailObject {
		public string Value;
		
		public SwailString(string value) : base('"' + value + '"') {
			this.Value = value;
		}
	}
}

