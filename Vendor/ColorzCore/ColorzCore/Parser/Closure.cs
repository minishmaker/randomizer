using System.Collections.Generic;

namespace ColorzCore.Parser
{
    public class Closure
    {
        public Closure()
        {
            Labels = new Dictionary<string, int>();
        }

        private Dictionary<string, int> Labels { get; }

        public virtual bool HasLocalLabel(string label)
        {
            return Labels.ContainsKey(label);
        }

        public virtual int GetLabel(string label)
        {
            return Labels[label];
        }

        public void AddLabel(string label, int value)
        {
            Labels[label] = value;
        }

        public IEnumerable<KeyValuePair<string, int>> LocalLabels()
        {
            foreach (var label in Labels) yield return label;
        }
    }
}
