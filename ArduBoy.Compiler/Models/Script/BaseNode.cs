using System.Collections;
using System.Reflection;

namespace ArduBoy.Compiler.Models.Script
{
	public abstract class BaseNode : INode
	{
		public INode? Parent { get; set; }

		protected BaseNode()
		{
			Parent = null;
		}

		protected BaseNode(INode parent)
		{
			Parent = parent;
		}

		internal List<PropertyInfo> _metaInfo = new List<PropertyInfo>();
		private void CacheMetaInfo()
		{
			if (_metaInfo.Count > 0)
				return;
			_metaInfo = GetType().GetProperties().ToList();
			_metaInfo.RemoveAll(x => x.PropertyType.IsPrimitive || x.Name == "Parent");
		}

		public List<T> FindTypes<T>(List<Type>? stopIf = null, bool ignoreFirst = false)
		{
			List<T> returnSet = new List<T>();
			FindTypes(returnSet, stopIf, ignoreFirst);
			return returnSet;
		}
		public void FindTypes<T>(List<T> returnSet, List<Type>? stopIf = null, bool ignoreFirst = false)
		{
			if (stopIf != null && !ignoreFirst && stopIf.Contains(GetType()))
				return;

			if (this is T self)
				returnSet.Add(self);

			CacheMetaInfo();

			foreach (var prop in _metaInfo)
			{
				if (prop.PropertyType.IsAssignableTo(typeof(INode)))
				{
					var testNode = prop.GetValue(this);
					if (testNode is INode valueNode)
						valueNode.FindTypes(returnSet, stopIf, false);
				}
				else if (IsList(prop.PropertyType))
				{
					var value = prop.GetValue(this);
					if (value is IEnumerable enu)
						foreach (var innerValueNode in enu)
							if (innerValueNode is INode actualInnerValueNode)
								actualInnerValueNode.FindTypes(returnSet, stopIf, false);
				}
			}
		}

		private bool IsList(Type o)
		{
			return o.IsGenericType &&
				   o.GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
		}

		public void SetParents()
		{
			CacheMetaInfo();

			foreach (var prop in _metaInfo)
			{
				if (prop.PropertyType.IsAssignableTo(typeof(INode)))
				{
					var testNode = prop.GetValue(this);
					if (testNode is INode valueNode)
					{
						valueNode.Parent = this;
						valueNode.SetParents();
					}
				}
				else if (IsList(prop.PropertyType))
				{
					var value = prop.GetValue(this);
					if (value is IEnumerable enu)
					{
						foreach (var innerValueNode in enu)
						{
							if (innerValueNode is INode actualInnerValueNode)
							{
								actualInnerValueNode.Parent = this;
								actualInnerValueNode.SetParents();
							}
						}
					}
				}
			}
		}

		public void Replace(INode node, INode with)
		{
			foreach (var prop in _metaInfo)
			{
				var value = prop.GetValue(this);
				if (value == node)
					prop.SetValue(this, with);
			}
		}
	}
}
