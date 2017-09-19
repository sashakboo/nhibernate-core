﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

namespace NHibernate.Engine
{
	using System.Threading.Tasks;
	using System.Threading;
	public partial interface IBatcher : IDisposable
	{

		/// <summary>
		/// Get a non-batchable an <see cref="DbCommand"/> to use for inserting / deleting / updating.
		/// Must be explicitly released by <c>CloseCommand()</c>
		/// </summary>
		/// <param name="sql">The <see cref="SqlString"/> to convert to an <see cref="DbCommand"/>.</param>
		/// <param name="commandType">The <see cref="CommandType"/> of the command.</param>
		/// <param name="parameterTypes">The <see cref="SqlType">SqlTypes</see> of parameters
		/// in <paramref name="sql" />.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns>
		/// An <see cref="DbCommand"/> that is ready to have the parameter values set
		/// and then executed.
		/// </returns>
		Task<DbCommand> PrepareCommandAsync(CommandType commandType, SqlString sql, SqlType[] parameterTypes, CancellationToken cancellationToken);

		/// <summary>
		/// Get a batchable <see cref="DbCommand"/> to use for inserting / deleting / updating
		/// (might be called many times before a single call to <c>ExecuteBatch()</c>
		/// </summary>
		/// <remarks>
		/// After setting parameters, call <c>AddToBatch()</c> - do not execute the statement
		/// explicitly.
		/// </remarks>
		/// <param name="sql">The <see cref="SqlString"/> to convert to an <see cref="DbCommand"/>.</param>
		/// <param name="commandType">The <see cref="CommandType"/> of the command.</param>
		/// <param name="parameterTypes">The <see cref="SqlType">SqlTypes</see> of parameters
		/// in <paramref name="sql" />.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns></returns>
		Task<DbCommand> PrepareBatchCommandAsync(CommandType commandType, SqlString sql, SqlType[] parameterTypes, CancellationToken cancellationToken);

		/// <summary>
		/// Add an insert / delete / update to the current batch (might be called multiple times
		/// for a single <c>PrepareBatchStatement()</c>)
		/// </summary>
		/// <param name="expectation">Determines whether the number of rows affected by query is correct.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		Task AddToBatchAsync(IExpectation expectation, CancellationToken cancellationToken);

		/// <summary>
		/// Execute the batch
		/// </summary>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		Task ExecuteBatchAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets an <see cref="DbDataReader"/> by calling ExecuteReader on the <see cref="DbCommand"/>.
		/// </summary>
		/// <param name="cmd">The <see cref="DbCommand"/> to execute to get the <see cref="DbDataReader"/>.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns>The <see cref="DbDataReader"/> from the <see cref="DbCommand"/>.</returns>
		/// <remarks>
		/// The Batcher is responsible for ensuring that all of the Drivers rules for how many open
		/// <see cref="DbDataReader"/>s it can have are followed.
		/// </remarks>
		Task<DbDataReader> ExecuteReaderAsync(DbCommand cmd, CancellationToken cancellationToken);

		/// <summary>
		/// Executes the <see cref="DbCommand"/>. 
		/// </summary>
		/// <param name="cmd">The <see cref="DbCommand"/> to execute.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns>The number of rows affected.</returns>
		/// <remarks>
		/// The Batcher is responsible for ensuring that all of the Drivers rules for how many open
		/// <see cref="DbDataReader"/>s it can have are followed.
		/// </remarks>
		Task<int> ExecuteNonQueryAsync(DbCommand cmd, CancellationToken cancellationToken);
	}
}