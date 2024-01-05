using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvoType;

class Scanner
{
	readonly string source;
	private const int limit = 1000;

	public Scanner(string path)
	{
		source = File.ReadAllText(path)[..limit];
	}

	public char[] GetKeys()
	{
		List<char> chars = new();

		foreach (char c in source)
		{
			if (char.IsWhiteSpace(c) || (!Keyboard.allKeys.Contains(c)) || c == '\0') continue;
			chars.Add(c);
		}
		return chars.ToArray();
	}

	public static char Normalize(char c)
	{
		c = char.ToLower(c);
		switch (c)
		{
			case '{':
				c = '[';
				break;
			case '}':
				c = ']';
				break;
			case ':':
				c = ';';
				break;
			case '"':
				c = '\'';
				break;
			case '<':
				c = ',';
				break;
			case '>':
				c = '.';
				break;
		}
		return c;
	}
}

