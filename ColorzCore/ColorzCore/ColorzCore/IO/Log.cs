using System;
using System.Collections.Generic;
using System.IO;
using ColorzCore.DataTypes;

namespace ColorzCore.IO
{
	public class Log
	{
		public enum MsgKind
		{
			ERROR,
			WARNING,
			NOTE,
			MESSAGE,
			DEBUG
		}

		protected static readonly Dictionary<MsgKind, LogDisplayConfig> KIND_DISPLAY_DICT =
			new Dictionary<MsgKind, LogDisplayConfig>
			{
				{ MsgKind.ERROR, new LogDisplayConfig { tag = "error", tagColor = ConsoleColor.Red } },
				{ MsgKind.WARNING, new LogDisplayConfig { tag = "warning", tagColor = ConsoleColor.Magenta } },
				{ MsgKind.NOTE, new LogDisplayConfig { tag = "note", tagColor = null } },
				{ MsgKind.MESSAGE, new LogDisplayConfig { tag = "message", tagColor = ConsoleColor.Blue } },
				{ MsgKind.DEBUG, new LogDisplayConfig { tag = "debug", tagColor = ConsoleColor.Green } }
			};

		public bool HasErrored { get; private set; }
		public bool WarningsAreErrors { get; set; } = false;

		public bool NoColoredTags { get; set; } = false;

		public List<MsgKind> IgnoredKinds { get; } = new List<MsgKind>();

		public TextWriter Output { get; set; } = Console.Error;

		public void Message(string message)
		{
			Message(MsgKind.MESSAGE, null, message);
		}

		public void Message(MsgKind kind, string message)
		{
			Message(kind, null, message);
		}

		public void Message(MsgKind kind, Location? source, string message)
		{
			if (WarningsAreErrors && kind == MsgKind.WARNING) kind = MsgKind.ERROR;

			HasErrored |= kind == MsgKind.ERROR;

			if (!IgnoredKinds.Contains(kind))
				if (KIND_DISPLAY_DICT.TryGetValue(kind, out var config))
				{
					if (!NoColoredTags && config.tagColor.HasValue)
						Console.ForegroundColor = config.tagColor.Value;

					Output.Write("{0}: ", config.tag);

					if (!NoColoredTags)
						Console.ResetColor();

					if (source.HasValue)
						Output.Write("{0}:{1}:{2}: ", source.Value.file, source.Value.lineNum, source.Value.colNum);

					Output.WriteLine(message);
				}
		}

		protected struct LogDisplayConfig
		{
			public string tag;
			public ConsoleColor? tagColor;
		}
	}
}
