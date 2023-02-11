using System.Collections.Generic;

namespace ColorzCore.Parser.Macros
{
    internal class MacroCollection
    {
        public MacroCollection(EaParser parent)
        {
            Macros = new Dictionary<string, Dictionary<int, IMacro>>();
            Parent = parent;

            BuiltInMacros = new Dictionary<string, BuiltInMacro>
            {
                { "String", new String() },
                { "IsDefined", new IsDefined(parent) },
                { "AddToPool", new AddToPool(parent) }
            };
        }

        public Dictionary<string, BuiltInMacro> BuiltInMacros { get; }
        public EaParser Parent { get; }

        private Dictionary<string, Dictionary<int, IMacro>> Macros { get; }

        public bool HasMacro(string name, int paramNum)
        {
            return (BuiltInMacros.ContainsKey(name) && BuiltInMacros[name].ValidNumParams(paramNum)) ||
                   (Macros.ContainsKey(name) && Macros[name].ContainsKey(paramNum));
        }

        public IMacro GetMacro(string name, int paramNum)
        {
            return BuiltInMacros.ContainsKey(name) && BuiltInMacros[name].ValidNumParams(paramNum)
                ? BuiltInMacros[name]
                : Macros[name][paramNum];
        }

        public void AddMacro(IMacro macro, string name, int paramNum)
        {
            if (!Macros.ContainsKey(name))
                Macros[name] = new Dictionary<int, IMacro>();
            Macros[name][paramNum] = macro;
        }

        public void Clear()
        {
            Macros.Clear();
        }

        public bool ContainsName(string name)
        {
            return BuiltInMacros.ContainsKey(name) || Macros.ContainsKey(name);
        }
    }
}
