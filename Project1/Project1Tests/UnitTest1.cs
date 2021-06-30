using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Project1DbContext;
using Domain;
using System.Collections.Generic;

namespace Project1Tests
{
    public class UnitTest1
    {
        //create in-memory DB
        DbContextOptions<Project1DBContext> options = new DbContextOptionsBuilder<Project1DBContext>().UseInMemoryDatabase(databaseName: "TestingDb").Options;

        [Fact]
        public void AddCustomerSuccess()
        {
            using(var context = new Project1DBContext(options))
            {
                // arrange
                bool result;
                Customer customer = new Customer()
                {
                    FirstName = "fname",
                    LastName = "lName",
                    Username = "username",
                    Password = "password"
                };
                // act
                context.Database.EnsureCreated();
                context.Database.EnsureDeleted();
                CustomerHandler customerHandler = new CustomerHandler(context);
                result = customerHandler.Add(customer);

                // assert
                Assert.True(result);
            }
        }

        [Fact]
        public void SearchCustomerSuccess()
        {
            using (var context = new Project1DBContext(options))
            {
                // arrange
                Customer customer = new Customer()
                {
                    FirstName = "fname",
                    LastName = "lName",
                    Username = "username",
                    Password = "password"
                };
                int id = 1;

                // act
                context.Database.EnsureCreated();
                context.Database.EnsureDeleted();
                CustomerHandler customerHandler = new CustomerHandler(context);
                customerHandler.Add(customer);
                Customer test = customerHandler.SearchCustomer(id);

                // assert
                Assert.Equal(customer, test);
            }
        }

        [Fact]
        public void LoginSuccess()
        {
            using (var context = new Project1DBContext(options))
            {
                // arrange
                Customer customer = new Customer()
                {
                    FirstName = "fname",
                    LastName = "lName",
                    Username = "username",
                    Password = "password"
                };
                // act
                context.Database.EnsureCreated();
                context.Database.EnsureDeleted();
                CustomerHandler customerHandler = new CustomerHandler(context);
                customerHandler.Add(customer);
                string test = customerHandler.LoginCustomer(customer.Username, customer.Password);

                // assert
                Assert.Equal("", test);
            }
        }

        [Fact]
        public void LoginIncorrectPw()
        {
            using (var context = new Project1DBContext(options))
            {
                // arrange
                Customer customer = new Customer()
                {
                    FirstName = "fname",
                    LastName = "lName",
                    Username = "username",
                    Password = "password"
                };
                // act
                context.Database.EnsureCreated();
                context.Database.EnsureDeleted();
                CustomerHandler customerHandler = new CustomerHandler(context);
                customerHandler.Add(customer);
                string test = customerHandler.LoginCustomer(customer.Username, "Incorrect password");

                // assert
                Assert.NotEqual("", test);
            }
        }

        [Fact]
        public void LoginIncorrectUsername()
        {
            using (var context = new Project1DBContext(options))
            {
                // arrange
                Customer customer = new Customer()
                {
                    FirstName = "fname",
                    LastName = "lName",
                    Username = "username",
                    Password = "password"
                };
                // act
                context.Database.EnsureCreated();
                context.Database.EnsureDeleted();
                CustomerHandler customerHandler = new CustomerHandler(context);
                customerHandler.Add(customer);
                string test = customerHandler.LoginCustomer("wrongName", customer.Password);

                // assert
                Assert.NotEqual("", test);
            }
        }
        [Fact]
        public void SearchCustomerFail()
        {
            using (var context = new Project1DBContext(options))
            {
                // arrange
                Customer customer = new Customer()
                {
                    FirstName = "fname",
                    LastName = "lName",
                    Username = "username",
                    Password = "password"
                };
                // act
                context.Database.EnsureCreated();
                context.Database.EnsureDeleted();
                CustomerHandler customerHandler = new CustomerHandler(context);
                customerHandler.Add(customer);
                Customer result = customerHandler.SearchCustomer(0);

                // assert
                Assert.Null(result);
            }
        }
        [Fact]
        public void SearchStoreSuccess()
        {
            using (var context = new Project1DBContext(options))
            {
                // arrange
                Store store = new Store()
                {
                    City = "city",
                    Region = "region"
                };
                // act
                context.Database.EnsureCreated();
                context.Database.EnsureDeleted();
                StoreHandler storeHandler = new StoreHandler(context);
                context.Add(store);
                context.SaveChanges();
                Store result = storeHandler.SearchStore(1);

                // assert
                Assert.NotNull(result);
            }
        }
        [Fact]
        public void SearchStoreFail()
        {
            using (var context = new Project1DBContext(options))
            {
                // arrange
                Store store = new Store()
                {
                    City = "city",
                    Region = "region"
                };
                // act
                context.Database.EnsureCreated();
                context.Database.EnsureDeleted();
                StoreHandler storeHandler = new StoreHandler(context);
                context.Add(store);
                context.SaveChanges();
                Store result = storeHandler.SearchStore(0);

                // assert
                Assert.Null(result);
            }
        }
        [Fact]
        public void StoreListNotNull()
        {
            using (var context = new Project1DBContext(options))
            {
                // arrange
                Store store = new Store()
                {
                    City = "city",
                    Region = "region"
                };
                // act
                context.Database.EnsureCreated();
                context.Database.EnsureDeleted();
                StoreHandler storeHandler = new StoreHandler(context);
                context.Add(store);
                context.SaveChanges();
                var result = storeHandler.StoreList();

                // assert
                Assert.NotNull(result);
            }
        }
        [Fact]
        public void SearchStoresNull()
        {
            using (var context = new Project1DBContext(options))
            {
                // arrange
                // act
                context.Database.EnsureCreated();
                context.Database.EnsureDeleted();
                StoreHandler storeHandler = new StoreHandler(context);
                context.SaveChanges();
                var result = storeHandler.StoreList();

                // assert
                Assert.Empty(result);
            }
        }
    }
}
