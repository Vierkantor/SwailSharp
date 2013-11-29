using System;
using System.Collections.Generic;

namespace SwailSharp {
	// thrown when a parser encounters something wrong
	public class SyntaxError : Exception {
		public SyntaxError(string message) : base (message) {
		}

		public SyntaxError(string expected, string received) : base ("Expected: '" + expected + "', received: '" + received + "'") {
		}
	}

	// a representation of the syntax tree after a parser has been run
	public class ParseState : SwailObject {
		public string RemainingText;
		public SwailObject Contents;

		public ParseState(string name) : base(name) {
			this.RemainingText = "";
			this.Contents = null;
		}

		public ParseState(ParseState copyFrom) : base(copyFrom.Name) {
			this.RemainingText = copyFrom.RemainingText;
			this.Contents = copyFrom.Contents;
		}
		
		public ParseState(string text, SwailObject contents) : base(contents.Name) {
			this.RemainingText = text;
			this.Contents = contents;
		}
	}

	// a representation of the part of the syntax tree after a small part has been matched
	public class MatchState : SwailObject {
		public string RemainingText;
		public string Rule;
		public SwailObject Contents;

		public MatchState(string name, string text, string rule, SwailObject contents) : base(name) {
			this.RemainingText = text;
			this.Rule = rule;
			this.Contents = contents;
		}
	}

	// can match a string to anything (like a name, or a string, etc.)
	// if matching doesn't work, throws a SyntaxError, otherwise it returns a MatchState
	public abstract class Match : SwailObject {
		public Match(string name) : base(name) {

		}

		public abstract MatchState MatchString(string text);
	}

	// a series of Matches
	public class ParseRule : SwailObject {
		IList<Match> Matches;
		SwailFunction Resolver;
		
		public ParseRule(string name, IList<Match> list, SwailFunction resolver) : base(name) {
			this.Matches = list;
			this.Resolver = resolver;
		}
		
		// will try to apply all its rules to the text
		// when succesful, resolves the parsed syntax tree
		public ParseState Match(SwailObject env, string text) {
			SwailDictionary<string, SwailObject> result = new SwailDictionary<string, SwailObject>("result");
			foreach (Match m in this.Matches) {
				MatchState state = m.MatchString(text);
				result[state.Rule] = state.Contents;
				text = state.RemainingText;
			}
			IList<SwailObject> args = new List<SwailObject>();
			args.Add(result);
			return new ParseState(text, Resolver.Call(env, args));
		}
	}
	
	// resolves a string into a SwailObject
	// the parser for a line is a specialization of this parser
	public class Parser : SwailObject {
		public static string SkipWhitespace(string text) {
			while (text.Length > 0) {
				if (text[0] == ' ' || text[0] == '\t' || text[0] == '\n') {
					text = text.Substring(1);
				} else if (text[0] == '/' && text[1] == '/') {
					while (text.Length > 0 && text[0] != '\n') {
						text = text.Substring(1);
					}
				} else {
					break;
				}
			}
			return text;
		}
		
		public IDictionary<string, IList<ParseRule>> Rules;

		public Parser(string name) : base(name) {
			this.Rules = new Dictionary<string, IList<ParseRule>>();
		}
	}
	
	// a Match that matches (only) the specified literal text
	public class LiteralMatch : Match {
		public string Text;
		
		public LiteralMatch(string text) : base("'" + text + "'") {
			this.Text = text;
		}
		
		override public MatchState MatchString(string text) {
			text = Parser.SkipWhitespace(text);
			if (text.StartsWith(this.Text)) {
				return new MatchState(this.Name, text.Substring(this.Text.Length), this.Text, new SwailString(this.Text));
			} else {
				throw new SyntaxError(this.Text, text);
			}
		}
	}

	// a Match that matches any rule in the Parser with the right name
	public class SubMatch : Match {
		public Parser ParentParser;
		public string Rule;
		public SwailObject Env;

		public SubMatch(string name, SwailObject env, Parser parent) : base("<" + name + ">") {
			this.Rule = name;
			this.Env = env;
			this.ParentParser = parent;
		}

		override public MatchState MatchString(string text) {
			SyntaxError lastError = null;

			foreach(ParseRule rule in this.ParentParser.Rules[this.Rule]) {
				try {
					ParseState state = rule.Match(this.Env, text);
					return new MatchState(this.Rule, state.RemainingText, this.Rule, state.Contents);
				} catch(SyntaxError err) {
					lastError = err;
				}
			}

			if (lastError == null) {
				throw lastError;
			}
			
			throw new SyntaxError("No rule defined for " + this.Name);
		}
	}
	
	// a few utility classes for common tokens
	
	// matches only the end of a string
	public class EndMatch : Match {
		public EndMatch() : base("<end>") {
		}
		
		override public MatchState MatchString(string text) {
			text = Parser.SkipWhitespace(text);
			if (text.Length == 0) {
				return new MatchState("<end>", text, "end", SwailObject.None);
			}
			
			throw new SyntaxError("<end>", text);
		}
	}
	
	// matches either a series of letters or anything up to the next whitespace
	public class NameMatch : Match {
		public NameMatch() : base("<name>") {
		}
		
		override public MatchState MatchString(string text) {
			text = Parser.SkipWhitespace(text);
			if (text.Length == 0) {
				throw new SyntaxError("<name>", "<end>");
			}
			
			string result = "";
			if (Char.IsLetter(text[0])) {
				while (text.Length > 0 && Char.IsLetterOrDigit(text[0])) {
					result = result + text[0];
					text = text.Substring(1);
				}
			} else {
				while (text.Length > 0 && !Char.IsWhiteSpace(text[0])) {
					result = result + text[0];
					text = text.Substring(1);
				}
			}
			return new MatchState("<name>", text, "name", new SwailString(text));
		}
	}
	
}

