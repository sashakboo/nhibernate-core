﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.Param
{
	using System.Threading.Tasks;
	using System.Threading;
	public partial class AggregatedIndexCollectionSelectorParameterSpecifications : IParameterSpecification
	{

		//public int Bind(DbCommand statement, QueryParameters qp, ISessionImplementor session, int position)
		//{
		//  int bindCount = 0;

		//  foreach (IParameterSpecification spec in _paramSpecs)
		//  {
		//    bindCount += spec.Bind(statement, qp, session, position + bindCount);
		//  }
		//  return bindCount;
		//}


		public Task BindAsync(DbCommand command, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public Task BindAsync(DbCommand command, IList<Parameter> multiSqlQueryParametersList, int singleSqlParametersOffset, IList<Parameter> sqlQueryParametersList, QueryParameters queryParameters, ISessionImplementor session, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}