namespace ArduBoy.Compiler.Models.Script
{
    public interface INode
    {
        public INode Parent { get; set; }
		public List<T> FindTypes<T>(List<Type>? stopIf = null, bool ignoreFirst = false);
        public void FindTypes<T>(List<T> returnSet, List<Type>? stopIf = null, bool ignoreFirst = false);
    }
}
