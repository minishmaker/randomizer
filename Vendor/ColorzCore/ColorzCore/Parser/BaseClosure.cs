namespace ColorzCore.Parser
{
    internal class BaseClosure : Closure
    {
        private EaParser _enclosing;

        public BaseClosure(EaParser enclosing)
        {
            this._enclosing = enclosing;
        }

        public override bool HasLocalLabel(string label)
        {
            return label.ToUpper() == "CURRENTOFFSET" || base.HasLocalLabel(label);
        }
    }
}
