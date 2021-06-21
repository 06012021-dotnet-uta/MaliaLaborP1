using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using Project1DbContext;

namespace Project1Tests
{
    public class UnitTest1
    {
        //create in-memory DB
        DbContextOptions<Project1DBContext> options = new DbContextOptionsBuilder<Project1DBContext>().UseInMemoryDatabase(databaseName: "TestingDb").Options;

        [Fact]
        public void Test1()
        {
            using(var context = new Project1DBContext(options))
            {
                // test against in mem db here
                // arrange

                // act

                // assert

            }
        }
    }
}
