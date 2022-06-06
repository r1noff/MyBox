#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace MyBox.Internal
{
	[InitializeOnLoad]
	public class MustBeAssignedConditionalFieldExclude
	{
		static MustBeAssignedConditionalFieldExclude()
		{
			MustBeAssignedAttributeChecker.ExcludeFieldFilter += ExcludeCheckIfConditionalFieldHidden;
		}
		
		private static readonly Type ConditionalType = typeof(ConditionalFieldAttribute);
		
		private static bool ExcludeCheckIfConditionalFieldHidden(FieldInfo field, UnityEngine.Object obj)
		{
			if (ConditionalType == null) return false;
			if (!field.IsDefined(ConditionalType, false)) return false;

			// Get a specific attribute of this field
			var conditionalFieldAttribute = field.GetCustomAttributes(ConditionalType, false)
				.Select(a => a as ConditionalFieldAttribute)
				.SingleOrDefault();

			return conditionalFieldAttribute != null &&
			       !ConditionalUtility.IsBehaviourConditionMatch(obj, field.Name, conditionalFieldAttribute.Data);
		}
	}
}
#endif