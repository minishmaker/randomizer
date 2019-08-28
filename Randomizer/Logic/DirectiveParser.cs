using MinishRandomizer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinishRandomizer.Randomizer.Logic
{

    public class DirectiveParser
    {
        List<LogicOption> Options;
        List<LogicDefine> Defines;
        List<EventDefine> EventDefines;

        private string[] SplitDirective(string line)
        {
            // Split define into parameters
            string[] untrimmed = line.Split('-');

            // Trim each parameter down
            string[] trimmed = new string[untrimmed.Length];
            for (int i = 0; i < untrimmed.Length; i++)
            {
                trimmed[i] = untrimmed[i].Trim();
            }

            return trimmed;
        }

        /// <summary>
        /// Check if a directive should be parsed on load or on randomization
        /// </summary>
        /// <param name="line">The line that contains the given directive</param>
        /// <returns>True if the directive should be parsed on load, throws an error if the directive is invalid.</returns>
        public bool ParseOnLoad(string line)
        {
            string directive = SplitDirective(line)[0];
            switch (directive)
            {
                case "!flag":
                case "!number":
                case "!name":
                case "!version":
                case "!date":
                    return true;
                case "!define":
                case "!ifdef":
                case "!ifndef":
                case "!else":
                case "!elseifdef":
                case "!elseifndef":
                case "!endif":
                    return false;
                default:
                    throw new ParserException($"\"{directive}\" is not a valid directive! The logic file may be bad.");
            }
        }

        public void ParseDirective(string line)
        {
            string[] mainDirectiveParts = SplitDirective(line);

            switch (mainDirectiveParts[0])
            {
                case "!flag":
                    Options.Add(ParseFlagDirective(mainDirectiveParts));
                    break;
                case "!number":
                case "!name":
                case "!version":
                case "!date":
                case "!define":
                case "!ifdef":
                case "!ifndef":
                case "!else":
                case "!elseifdef":
                case "!elseifndef":
                case "!endif":
                default:
                    throw new ParserException($"\"{mainDirectiveParts[0]}\" is not a valid directive!");
            }
        }

        private LogicFlag ParseFlagDirective(string[] directiveParts)
        {
            return null;
        }
    }
}
