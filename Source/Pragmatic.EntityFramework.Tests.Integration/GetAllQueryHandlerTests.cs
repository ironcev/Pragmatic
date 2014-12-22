// WARNING: !!! Intentionally Bad code !!!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Pragmatic.EntityFramework.Interaction.StandardQueries;
using Pragmatic.EntityFramework.Tests.Integration.Data;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;

namespace Pragmatic.EntityFramework.Tests.Integration
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GetAllQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            using (var db = new PersonsContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM People");
            }
        }

        [Test]
        public void Get_all_persons()
        {
            var ids = new List<Guid>();
            using (var db = new PersonsContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo" },
                    new Person { Name = "Peter Pan" },
                    new Person { Name = "Tom Sawyer" }
                };

                ids.AddRange(persons.Select(person => person.Id));

                foreach (var person in persons)
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();                
            }

            var result = new GetAllQueryHandler<Person>(new PersonsContext())
                             .Execute(new GetAllQuery<Person>());

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(ids.Count));
            Assert.That(result.Count(), Is.EqualTo(ids.Count));
        }

        [Test]
        public void Get_all_persons_orderd_by_id()
        {
            var ids = new List<Guid>();
            using (var db = new PersonsContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo" },
                    new Person { Name = "Peter Pan" },
                    new Person { Name = "Tom Sawyer" }
                };

                ids.AddRange(persons.Select(person => person.Id));

                foreach (var person in persons)
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PersonsContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = Option<OrderBy<Person>>.From(new OrderBy<Person>(x => x.Id))
                             });

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(ids.Count));
            CollectionAssert.AreNotEqual(ids, result.Select(x => x.Id)); // SQL Server and .NET have different strategies for sorting GUIDs.
        }

        [Test]
        public void Get_all_persons_orderd_by_id_descending()
        {
            var ids = new List<Guid>();
            using (var db = new PersonsContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo" },
                    new Person { Name = "Peter Pan" },
                    new Person { Name = "Tom Sawyer" }
                };

                ids.AddRange(persons.Select(person => person.Id));

                foreach (var person in persons)
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PersonsContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = Option<OrderBy<Person>>.From(new OrderBy<Person>(x => x.Id, OrderByDirection.Descending))
                             });

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(ids.Count));
            CollectionAssert.AreNotEqual(ids, result.Select(x => x.Id)); // SQL Server and .NET have different strategies for sorting GUIDs.
        }

        [Test]
        public void Get_all_persons_orderd_by_name()
        {
            var names = new List<string>();
            using (var db = new PersonsContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo" },
                    new Person { Name = "Peter Pan" },
                    new Person { Name = "Tom Sawyer" }
                };

                names.AddRange(persons.Select(person => person.Name));

                foreach (var person in persons)
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PersonsContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = Option<OrderBy<Person>>.From(new OrderBy<Person>(x => x.Name))
                             });

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(names.Count));
            CollectionAssert.AreEqual(names.OrderBy(x => x), result.Select(x => x.Name));
        }

        [Test]
        public void Get_all_persons_orderd_by_name_descending()
        {
            var names = new List<string>();
            using (var db = new PersonsContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo" },
                    new Person { Name = "Peter Pan" },
                    new Person { Name = "Tom Sawyer" }
                };

                names.AddRange(persons.Select(person => person.Name));

                foreach (var person in persons)
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PersonsContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = Option<OrderBy<Person>>.From(new OrderBy<Person>(x => x.Name, OrderByDirection.Descending))
                             });

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(names.Count));
            CollectionAssert.AreEqual(names.OrderByDescending(x => x), result.Select(x => x.Name));
        }

        [Test]
        public void Get_all_persons_with_pagination()
        {
            var ids = new List<Guid>();
            using (var db = new PersonsContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo" },
                    new Person { Name = "Peter Pan" },
                    new Person { Name = "Tom Sawyer" }
                };

                ids.AddRange(persons.Select(person => person.Id));

                foreach (var person in persons)
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PersonsContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = Option<OrderBy<Person>>.From(new OrderBy<Person>(x => x.Name)),
                                 Paging = Option<Paging>.From(new Paging(1, 1))
                             });

            Assert.That(result.CurrentPage, Is.EqualTo(1));
            Assert.That(result.PageSize, Is.EqualTo(1));
            Assert.That(result.TotalCount, Is.EqualTo(ids.Count));
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Get_all_persons_with_pagination_without_order_by_throws_exception()
        {
            var ids = new List<Guid>();
            using (var db = new PersonsContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo" },
                    new Person { Name = "Peter Pan" },
                    new Person { Name = "Tom Sawyer" }
                };

                ids.AddRange(persons.Select(person => person.Id));

                foreach (var person in persons)
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var exception = Assert.Throws<ArgumentException>(() => new GetAllQueryHandler<Person>(new PersonsContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 Paging = Option<Paging>.From(new Paging(1, 1))
                             }));

            Console.WriteLine(exception.Message);

            Assert.That(exception.ParamName, Is.EqualTo("query"));
            Assert.That(exception.Message.Contains("Entity Framework supports pagination only if the ordering is specified."));
        }

        [Test]
        public void Get_persons_with_certain_name()
        {
            var ids = new List<Guid>();
            using (var db = new PersonsContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo" },
                    new Person { Name = "Peter Pan" },
                    new Person { Name = "Tom Sawyer" },
                    new Person { Name = "Han Pan" }
                };

                ids.AddRange(persons.Select(person => person.Id));

                foreach (var person in persons)
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PersonsContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 Criteria = Option<Expression<Func<Person, bool>>>.From(x => x.Name.StartsWith("Han")) 
                             });

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(2));
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.All(x => x.Name.StartsWith("Han")));
        }
    }
    // ReSharper restore InconsistentNaming
}