using System;
using System.Collections;
using System.IO;
using System.Text;

namespace eaep.servicehost.http
{
	public class TemplateParser
	{
		public const string DefaultTokenStart = "${";
		public const string DefaultTokenEnd = "}";

		private TextReader inputReader;
		private TextWriter outputWriter;
		private string tokenStart;
		private string tokenEnd;

		private string currentToken;

		public TemplateParser(Stream input, Stream output)
		{
			inputReader = new StreamReader(input);
			outputWriter = new StreamWriter(output);
			TokenStart = DefaultTokenStart;
			TokenEnd = DefaultTokenEnd;
		}

		public TemplateParser(TextReader input, ref TextWriter output)
		{
			inputReader = input;
			outputWriter = output;
			TokenStart = DefaultTokenStart;
			TokenEnd = DefaultTokenEnd;
		}

		public static string Parse(string input, Hashtable values)
		{
			StringBuilder output = new StringBuilder();

			TextReader inputReader = new StringReader(input);
			TextWriter outputWriter = new StringWriter(output);

			return DoParse(inputReader, outputWriter, values);
		}

		public static string ParseFile(string fileName, Hashtable values)
		{
			StringBuilder output = new StringBuilder();

			TextReader inputReader = new StreamReader(fileName);
			TextWriter outputWriter = new StringWriter(output);

			return DoParse(inputReader, outputWriter, values);
		}

		private static string DoParse(TextReader input, TextWriter output, Hashtable values)
		{
			TemplateParser parser = new TemplateParser(input, ref output);
			while (parser.Read())
			{
				string tokenValue = String.Empty ;

				if (values.ContainsKey(parser.CurrentToken))
				{
					if (values[parser.CurrentToken] != null)
					{
						tokenValue = values[parser.CurrentToken].ToString();
					}
				}
				else
				{
					tokenValue = "###" + parser.CurrentToken + "###";
				}
				parser.ReplaceToken(tokenValue);
			}
			return output.ToString();
		}

		public string TokenStart
		{
			get { return tokenStart; }
			set { tokenStart = value; }
		}

		public string TokenEnd
		{
			get { return tokenEnd; }
			set { tokenEnd = value; }
		}

		public bool Read()
		{
			try
			{
				if (ReadToCharSequence(TokenStart.ToCharArray(), outputWriter))
				{
					StringBuilder tokenName = new StringBuilder();
					StringWriter tokenNameWriter = new StringWriter(tokenName);

					if (ReadToCharSequence(TokenEnd.ToCharArray(), tokenNameWriter))
					{
						currentToken = tokenName.ToString();
						return true;
					}
					else
						outputWriter.Write(tokenName.ToString());
				}
				return false;
			}
			finally
			{
				outputWriter.Flush();
			}
		}

		private bool ReadToCharSequence(char[] sequence, TextWriter output)
		{
			char c;
			char[] buffer = new char[sequence.Length];
			int sequencePosition = 0;

			while (inputReader.Peek() != -1)
			{
				c = (char)inputReader.Read();
				if (c == sequence[sequencePosition])
				{
					buffer[sequencePosition] = c;
					sequencePosition++;
					if (sequencePosition == sequence.Length)
						return true;
				}
				else
				{
					for (int i = 0; i < sequencePosition; i++)
					{
						output.Write(buffer[i]);
					}
					// reset sequence position as not complete.
					sequencePosition = 0;

					output.Write(c);
				}
			}

			for (int i = 0; i < sequencePosition; i++)
				output.Write(buffer[i]);

			return false;
		}

		public string CurrentToken
		{
			get { return currentToken; }
		}

		public void ReplaceToken(string text)
		{
			outputWriter.Write(text);
			outputWriter.Flush();
		}
	}
}
