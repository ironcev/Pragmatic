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
using SwissKnife.Collections;

namespace Pragmatic.EntityFramework.Tests.Integration
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GetAllQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            using (var db = new PragmaticDbContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM People");
            }
        }

        [Test]
        public void Get_all_persons()
        {
            var ids = new List<Guid>();
            using (var db = new PragmaticDbContext())
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

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
                             .Execute(new GetAllQuery<Person>());

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(ids.Count));
            Assert.That(result.Count(), Is.EqualTo(ids.Count));
        }

        [Test]
        public void Get_all_persons_orderd_by_age()
        {
            var ages = new List<int>();
            using (var db = new PragmaticDbContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo", Age = 30 },
                    new Person { Name = "Peter Pan", Age = 40 },
                    new Person { Name = "Tom Sawyer", Age = 50 }
                };

                ages.AddRange(persons.Select(person => person.Age));

                // Let's create persons in some random order.
                foreach (var person in persons.Randomize())
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = new OrderBy<Person>(x => x.Age)
                             });

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(ages.Count));
            CollectionAssert.AreEqual(ages, result.Select(x => x.Age));
        }

        [Test]
        public void Get_all_persons_orderd_by_age_descending()
        {
            var ages = new List<int>();
            using (var db = new PragmaticDbContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo", Age = 30 },
                    new Person { Name = "Peter Pan", Age = 40 },
                    new Person { Name = "Tom Sawyer", Age = 50 }
                };

                ages.AddRange(persons.Select(person => person.Age));

                // Let's create persons in some random order.
                foreach (var person in persons.Randomize())
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = new OrderBy<Person>(x => x.Age, OrderByDirection.Descending)
                             });

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(ages.Count));
            CollectionAssert.AreEqual(ages.OrderByDescending(x => x), result.Select(x => x.Age));
        }

        [Test]
        public void Get_all_persons_orderd_by_name()
        {
            var names = new List<string>();
            using (var db = new PragmaticDbContext())
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

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
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
            using (var db = new PragmaticDbContext())
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

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
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
        public void Get_all_persons_orderd_by_is_administrator()
        {
            var isAdministratorValues = new List<bool>();
            using (var db = new PragmaticDbContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo", IsAdministrator = false },
                    new Person { Name = "Peter Pan", IsAdministrator = false },
                    new Person { Name = "Tom Sawyer", IsAdministrator = true }
                };

                isAdministratorValues.AddRange(persons.Select(person => person.IsAdministrator));

                foreach (var person in persons)
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = Option<OrderBy<Person>>.From(new OrderBy<Person>(x => x.IsAdministrator))
                             });

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(isAdministratorValues.Count));
            CollectionAssert.AreEqual(isAdministratorValues.OrderBy(x => x), result.Select(x => x.IsAdministrator));
        }

        [Test]
        public void Get_all_persons_orderd_by_is_administrator_descending()
        {
            var isAdministratorValues = new List<bool>();
            using (var db = new PragmaticDbContext())
            {
                Person[] persons =
                {
                    new Person { Name = "Han Solo", IsAdministrator = false },
                    new Person { Name = "Peter Pan", IsAdministrator = false },
                    new Person { Name = "Tom Sawyer", IsAdministrator = true }
                };

                isAdministratorValues.AddRange(persons.Select(person => person.IsAdministrator));

                foreach (var person in persons)
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = Option<OrderBy<Person>>.From(new OrderBy<Person>(x => x.IsAdministrator, OrderByDirection.Descending))
                             });

            Assert.That(result.CurrentPage, Is.EqualTo(Paging.None.Page));
            Assert.That(result.PageSize, Is.EqualTo(Paging.None.PageSize));
            Assert.That(result.TotalCount, Is.EqualTo(isAdministratorValues.Count));
            CollectionAssert.AreEqual(isAdministratorValues.OrderByDescending(x => x), result.Select(x => x.IsAdministrator));
        }

        [Test]
        public void Get_all_persons_with_pagination()
        {
            var ids = new List<Guid>();
            using (var db = new PragmaticDbContext())
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

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
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
            using (var db = new PragmaticDbContext())
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

            var exception = Assert.Throws<ArgumentException>(() => new GetAllQueryHandler<Person>(new PragmaticDbContext())
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
            using (var db = new PragmaticDbContext())
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

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
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

        [Test]
        public void Get_all_persons_ordered_by_name_and_age()
        {
            var namesAndAges = new[]
            {
                new { Name = "Anne", Age = 50 },
                new { Name = "Anne", Age = 60 },
                new { Name = "Bob", Age = 30 },
                new { Name = "Bob", Age = 40 },
                new { Name = "Clark", Age = 10 },
                new { Name = "Clark", Age = 20 }
            };

            // Let's add persons into the database in a random order.
            using (var db = new PragmaticDbContext())
            {
                foreach (var person in namesAndAges.Select(x => new Person { Name = x.Name, Age = x.Age }).Randomize())
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = new OrderBy<Person>(person => person.Name).ThenBy(person => person.Age)
                             });

            var expectedNamesAndAges = namesAndAges;
            var actualNamesAndAges = result.Select(person => new { person.Name, person.Age });

            CollectionAssert.AreEqual(expectedNamesAndAges, actualNamesAndAges);


            result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = new OrderBy<Person>(person => person.Age).ThenBy(person => person.Name)
                             });

            expectedNamesAndAges = namesAndAges.OrderBy(x => x.Age).ThenBy(x => x.Name).ToArray();
            actualNamesAndAges = result.Select(person => new { person.Name, person.Age });

            CollectionAssert.AreEqual(expectedNamesAndAges, actualNamesAndAges);
        }

        /// <summary>
        /// Covers the buggy behavior that we had in the QueryableExtensions.OrderByWithExpressionTransform()
        /// method, that in case when more than two order by properties were specified,
        /// only the first and the last were actually taken.
        /// So we are testing here if sorting works for more than two properties.
        /// </summary>
        [Test]
        public void Get_all_persons_ordered_by_name_and_age_and_date_of_birth()
        {
            var namesAgesAndDatesOfBirth = new[]
            {
                new { Name = "Anne", Age = 50, DateOfBirth = new DateTime(2000, 12, 1) },
                new { Name = "Anne", Age = 50, DateOfBirth = new DateTime(2000, 12, 2) },
                new { Name = "Anne", Age = 60, DateOfBirth = new DateTime(2000, 12, 3) },
                new { Name = "Anne", Age = 60, DateOfBirth = new DateTime(2000, 12, 4) },
                new { Name = "Bob", Age = 30, DateOfBirth = new DateTime(2000, 12, 1) },
                new { Name = "Bob", Age = 30, DateOfBirth = new DateTime(2000, 12, 2) },
                new { Name = "Bob", Age = 40, DateOfBirth = new DateTime(2000, 12, 3) },
                new { Name = "Bob", Age = 40, DateOfBirth = new DateTime(2000, 12, 4) },
                new { Name = "Clark", Age = 10, DateOfBirth = new DateTime(2000, 12, 1) },
                new { Name = "Clark", Age = 10, DateOfBirth = new DateTime(2000, 12, 2) },
                new { Name = "Clark", Age = 20, DateOfBirth = new DateTime(2000, 12, 3) },
                new { Name = "Clark", Age = 20, DateOfBirth = new DateTime(2000, 12, 4) }
            };

            // Let's add persons into the database in a random order.
            using (var db = new PragmaticDbContext())
            {
                foreach (var person in namesAgesAndDatesOfBirth.Select(x => new Person { Name = x.Name, Age = x.Age, DateOfBirth = x.DateOfBirth }).Randomize())
                {
                    db.Persons.Add(person);
                }
                db.SaveChanges();
            }

            var result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = new OrderBy<Person>(person => person.Name)
                                     .ThenBy(person => person.Age)
                                     .ThenBy(person => person.DateOfBirth)
                             });

            var expectedNamesAndAges = namesAgesAndDatesOfBirth;
            var actualNamesAndAges = result.Select(person => new { person.Name, person.Age, person.DateOfBirth });

            CollectionAssert.AreEqual(expectedNamesAndAges, actualNamesAndAges);


            result = new GetAllQueryHandler<Person>(new PragmaticDbContext())
                             .Execute(new GetAllQuery<Person>
                             {
                                 OrderBy = new OrderBy<Person>(person => person.DateOfBirth)
                                    .ThenBy(person => person.Age)
                                    .ThenBy(person => person.Name)
                             });

            expectedNamesAndAges = namesAgesAndDatesOfBirth.OrderBy(x => x.DateOfBirth).ThenBy(x => x.Age).ThenBy(x => x.Name).ToArray();
            actualNamesAndAges = result.Select(person => new { person.Name, person.Age, person.DateOfBirth });

            CollectionAssert.AreEqual(expectedNamesAndAges, actualNamesAndAges);
        }
    }
    // ReSharper restore InconsistentNaming
}