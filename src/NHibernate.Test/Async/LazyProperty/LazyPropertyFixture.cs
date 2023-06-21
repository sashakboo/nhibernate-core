﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Cfg;
using NHibernate.Intercept;
using NHibernate.Tuple.Entity;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NHibernate.Linq;

namespace NHibernate.Test.LazyProperty
{
	using System.Threading.Tasks;
	[TestFixture]
	public class LazyPropertyFixtureAsync : TestCase
	{
		private string log;

		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override string[] Mappings
		{
			get { return new[] { "LazyProperty.Mappings.hbm.xml" }; }
		}

		protected override DebugSessionFactory BuildSessionFactory()
		{
			using (var logSpy = new LogSpy(typeof(EntityMetamodel)))
			{
				var factory = base.BuildSessionFactory();
				log = logSpy.GetWholeLog();
				return factory;
			}
		}

		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.GenerateStatistics, "true");
		}

		protected override void OnSetUp()
		{
			Assert.That(
				nameof(Book.FieldInterceptor),
				Is.EqualTo(nameof(IFieldInterceptorAccessor.FieldInterceptor)),
				$"Test pre-condition not met: entity property {nameof(Book.FieldInterceptor)} should have the same " +
				$"name than {nameof(IFieldInterceptorAccessor)}.{nameof(IFieldInterceptorAccessor.FieldInterceptor)}");
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				s.Persist(new Book
				{
					Name = "some name",
					Id = 1,
					ALotOfText = "a lot of text ...",
					Image = new byte[10],
					NoSetterImage = new byte[10],
					FieldInterceptor = "Why not that name?"
				});
				tx.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				s.CreateQuery("delete from Word").ExecuteUpdate();
				s.CreateQuery("delete from Book").ExecuteUpdate();
				tx.Commit();
			}
		}

		[Test]
		public async Task PropertyLoadedNotInitializedAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.LoadAsync<Book>(1));

				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Id"), Is.False);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Name"), Is.False);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.False);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, nameof(Book.FieldInterceptor)), Is.False);

				await (NHibernateUtil.InitializeAsync(book));

				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Id"), Is.True);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Name"), Is.True);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.False);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, nameof(Book.FieldInterceptor)), Is.True);
			}
		}

		[Test]
		public async Task PropertyLoadedNotInitializedWhenUsingGetAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));

				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Id"), Is.True);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Name"), Is.True);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.False);
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, nameof(Book.FieldInterceptor)), Is.True);
			}
		}

		[Test]
		public async Task CanGetValueForLazyPropertyAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));

				Assert.That(book.ALotOfText, Is.EqualTo("a lot of text ..."));
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.True);
			}
		}

		[Test]
		public async Task CanSetValueForLazyPropertyAsync()
		{
			Book book;
			using (ISession s = OpenSession())
			{
				book = await (s.GetAsync<Book>(1));
			}

			book.ALotOfText = "text";

			Assert.That(book.ALotOfText, Is.EqualTo("text"));
			Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.True);
		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task CanUpdateValueForLazyPropertyAsync(bool initializeAfterSet)
		{
			Book book;
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				book = await (s.GetAsync<Book>(1));
				book.ALotOfText = "update-text";
				if (initializeAfterSet)
				{
					var image = book.Image;
				}

				await (tx.CommitAsync());
			}

			using (var s = OpenSession())
			{
				book = await (s.GetAsync<Book>(1));
				var text = book.ALotOfText;
			}

			Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.True);
			Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Image"), Is.True);
			Assert.That(book.ALotOfText, Is.EqualTo("update-text"));
			Assert.That(book.Image, Has.Length.EqualTo(10));
		}

		[TestCase(false)]
		[TestCase(true)]
		public async Task UpdateValueForLazyPropertyToSameValueAsync(bool initializeAfterSet)
		{
			Book book;
			string text;

			using (var s = OpenSession())
			{
				book = await (s.GetAsync<Book>(1));
				text = book.ALotOfText;
			}

			Sfi.Statistics.Clear();

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				book = await (s.GetAsync<Book>(1));
				book.ALotOfText = text;
				if (initializeAfterSet)
				{
					var image = book.Image;
				}

				await (tx.CommitAsync());
			}

			Assert.That(Sfi.Statistics.EntityUpdateCount, Is.EqualTo(initializeAfterSet ? 0 : 1));
			Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.True);
			Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Image"), initializeAfterSet ? (Constraint) Is.True : Is.False);
			Assert.That(book.ALotOfText, Is.EqualTo(text));

			using (var s = OpenSession())
			{
				book = await (s.GetAsync<Book>(1));
				text = book.ALotOfText;
			}

			Assert.That(book.Image, Has.Length.EqualTo(10));
			Assert.That(book.ALotOfText, Is.EqualTo(text));
		}

		[Test]
		public async Task CanGetValueForNonLazyPropertyAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));

				Assert.That(book.Name, Is.EqualTo("some name"));
				Assert.That(book.FieldInterceptor, Is.EqualTo("Why not that name?"));
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.False);
			}
		}

		[Test]
		public async Task CanLoadAndSaveObjectInDifferentSessionsAsync()
		{
			Book book;
			int bookCount;
			using (ISession s = OpenSession())
			{
				bookCount = await (s.Query<Book>().CountAsync());
				book = await (s.GetAsync<Book>(1));
			}

			book.Name += " updated";

			using (ISession s = OpenSession())
			{
				await (s.MergeAsync(book));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				Assert.That(await (s.Query<Book>().CountAsync()), Is.EqualTo(bookCount));
				Assert.That((await (s.GetAsync<Book>(1))).Name, Is.EqualTo(book.Name));
			}
		}

		[Test]
		public async Task CanUpdateNonLazyWithoutLoadingLazyPropertyAsync()
		{
			Book book;
			using (ISession s = OpenSession())
			using (var trans = s.BeginTransaction())
			{
				book = await (s.GetAsync<Book>(1));
				book.Name += "updated";
				book.FieldInterceptor += "updated";

				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.False, "Before flush and commit");
				await (trans.CommitAsync());
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.False, "After flush and commit");
			}

			using (ISession s = OpenSession())
			{
				book = await (s.GetAsync<Book>(1));
				Assert.That(book.Name, Is.EqualTo("some nameupdated"));
				Assert.That(book.FieldInterceptor, Is.EqualTo("Why not that name?updated"));
			}
		}

		[Test]
		public async Task CanMergeTransientWithLazyPropertyAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(2));
				Assert.That(book, Is.Null);
			}

			using (ISession s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var book = new Book
				{
					Name = "some name two",
					Id = 2,
					ALotOfText = "a lot of text two..."
				};
				// This should insert a new entity.
				await (s.MergeAsync(book));
				await (tx.CommitAsync());
			}

			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(2));
				Assert.That(book, Is.Not.Null);
				Assert.That(book.Name, Is.EqualTo("some name two"));
				Assert.That(book.ALotOfText, Is.EqualTo("a lot of text two..."));
			}
		}

		[Test]
		public async Task CacheShouldNotContainLazyPropertiesAsync()
		{
			Book book;

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				book = await (s.CreateQuery("from Book b fetch all properties where b.Id = :id")
				        .SetParameter("id", 1)
				        .UniqueResultAsync<Book>());
				await (tx.CommitAsync());
			}

			Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.True);
			Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Image"), Is.True);

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				book = await (s.GetAsync<Book>(1));
				await (tx.CommitAsync());
			}

			Assert.That(NHibernateUtil.IsPropertyInitialized(book, "ALotOfText"), Is.False);
			Assert.That(NHibernateUtil.IsPropertyInitialized(book, "Image"), Is.False);
		}

		[Test]
		public async Task CanMergeTransientWithLazyPropertyInCollectionAsync()
		{
			Book book;

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				book = new Book
				{
					Name = "some name two",
					Id = 3,
					ALotOfText = "a lot of text two..."
				};
				// This should insert a new entity.
				await (s.MergeAsync(book));
				await (tx.CommitAsync());
			}

			using (var s = OpenSession())
			{
				book = await (s.GetAsync<Book>(3));
				Assert.That(book, Is.Not.Null);
				Assert.That(book.Name, Is.EqualTo("some name two"));
				Assert.That(book.ALotOfText, Is.EqualTo("a lot of text two..."));
			}
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				book.Words = new List<Word>();
				var word = new Word
				{
					Id = 2,
					Parent = book,
					Content = new byte[1] {0}
				};

				book.Words.Add(word);
				await (s.MergeAsync(book));
				await (tx.CommitAsync());
			}

			using (var s = OpenSession())
			{
				book = await (s.GetAsync<Book>(3));
				Assert.That(book.Words.Any(), Is.True);
				Assert.That(book.Words.First().Content, Is.EqualTo(new byte[1] { 0 }));
			}
		}

		[Test]
		public async Task GetLazyPropertyWithNoSetterAccessor_PropertyShouldBeInitializedAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));
				var image = book.NoSetterImage;
				// Fails. Property remains uninitialized after it has been accessed.
				Assert.That(NHibernateUtil.IsPropertyInitialized(book, "NoSetterImage"), Is.True);
			}
		}

		[Test]
		public async Task GetLazyPropertyWithNoSetterAccessorTwice_ResultsAreSameObjectAsync()
		{
			using (ISession s = OpenSession())
			{
				var book = await (s.GetAsync<Book>(1));
				var image = book.NoSetterImage;
				var sameImage = book.NoSetterImage;
				// Fails. Each call to a property getter returns a new object.
				Assert.That(ReferenceEquals(image, sameImage), Is.True);
			}
		}
	}
}
