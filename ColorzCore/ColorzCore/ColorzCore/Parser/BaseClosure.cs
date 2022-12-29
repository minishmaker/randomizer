namespace ColorzCore.Parser
{
	internal class BaseClosure : Closure
	{
		private EAParser enclosing;

		public BaseClosure(EAParser enclosing)
		{
			this.enclosing = enclosing;
		}

		public override bool HasLocalLabel(string label)
		{
			return label.ToUpper() == "CURRENTOFFSET" || base.HasLocalLabel(label);
		}
	}
}
