using System;
using UnityEditor.MemoryProfiler;
using System.Collections.Generic;
using System.Linq;

namespace MemoryProfilerWindow
{
	static class TypeTools
	{
		public enum FieldFindOptions
		{
			OnlyInstance,
			OnlyStatic
		}

		static public IEnumerable<FieldDescription> AllFieldsOf (TypeDescription typeDescription, TypeDescription[] typeDescriptions, FieldFindOptions findOptions)
		{
			if (typeDescription.isArray)
				yield break;
			
			if (findOptions != FieldFindOptions.OnlyStatic && typeDescription.baseOrElementTypeIndex != -1 && !typeDescription.isValueType)
			{
				var baseTypeDescription = typeDescriptions [typeDescription.baseOrElementTypeIndex];
				foreach(var field in AllFieldsOf(baseTypeDescription, typeDescriptions, findOptions))
					yield return field;
			}

			foreach (var field in typeDescription.fields)
			{
				if (!FieldMatchesOptions(field, findOptions))
					continue;

				if (field.typeIndex == typeDescription.typeIndex && typeDescription.isValueType)
				{
					// this happens in primitive types like System.Single, which is a weird type that has a field of its own type.
					continue;
				}

				if (field.offset == -1)
				{
					// this is how we encode TLS fields. We don't support TLS fields yet.
					continue;
				}

				yield return field;
			}
		}

		static bool FieldMatchesOptions(FieldDescription field, FieldFindOptions options)
		{
			if (field.isStatic && options == FieldFindOptions.OnlyStatic)
				return true;
			if (!field.isStatic && options == FieldFindOptions.OnlyInstance)
				return true;

			return false;

		}
	}
}

