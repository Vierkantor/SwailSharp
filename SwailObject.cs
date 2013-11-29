using System;
using System.Collections.Generic;

namespace SwailSharp
{
	// a thing that code in Swail can possibly reach
	// Most things should be SwailObjects, even things like the Parser or ShellWindow
	public class SwailObject {
		public static SwailObject None = new SwailObject("None");

		// Swail is object-oriented now: every object has its own members
		public IDictionary<string, SwailObject> Members;
		// the name under which the object was defined
		public string Name;

		public SwailObject(string name)
		{
			this.Name = name;
			this.Members = new Dictionary<string, SwailObject>();
			
			this.Members.Add("this", this);
		}
	}
}
