using System;
using System.Collections.Generic;
using System.Linq;

namespace SwailSharp {
	public abstract class SwailFunction : SwailObject{
		public IList<string> Params;

		public SwailFunction(string name, IList<string> parameters) : base(name) {
			this.Params = parameters;
		}

		public abstract SwailObject Exec(SwailObject callingEnv, IList<SwailObject> args);

		public SwailObject Call(SwailObject callingEnv, IList<SwailObject> args) {
			if (this.Params.Count > args.Count) {
				return new PartialFunction(this, this.Params, args);
			}
			if (this.Params.Count < args.Count) {
				throw new Exception("Too many arguments to " + this.Name);
			}
			return this.Exec(callingEnv, args);
		}
	}

	public class PartialFunction : SwailFunction {
		public SwailFunction Function;
		public IList<SwailObject> Stored;

		public PartialFunction(SwailFunction function, IList<string> parameters, IList<SwailObject> args)
			: base(function.Name + " (partial)", parameters.Skip(args.Count).ToList()) {
			this.Function = function;
			this.Stored = args;
		}

		public override SwailObject Exec(SwailObject callingEnv, IList<SwailObject> args) {
			int i = 0;
			foreach (SwailObject obj in this.Stored) {
				args.Insert(i, obj);
				i++;
			}
			return this.Function.Call(callingEnv, args);
		}
	}
}

