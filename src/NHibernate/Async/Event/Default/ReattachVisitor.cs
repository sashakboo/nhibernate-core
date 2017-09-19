﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



using NHibernate.Action;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Type;

namespace NHibernate.Event.Default
{
	using System.Threading.Tasks;
	using System.Threading;
	public abstract partial class ReattachVisitor : ProxyVisitor
	{

		internal override async Task<object> ProcessComponentAsync(object component, IAbstractComponentType componentType, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			IType[] types = componentType.Subtypes;
			if (component == null)
			{
				await (ProcessValuesAsync(new object[types.Length], types, cancellationToken)).ConfigureAwait(false);
			}
			else
			{
				await (base.ProcessComponentAsync(component, componentType, cancellationToken)).ConfigureAwait(false);
			}

			return null;
		}
	}
}