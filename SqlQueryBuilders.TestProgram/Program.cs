using SqlQueryBulders.Core;
using System;

namespace SqlQueryBuilders.TestProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            //string query = SqlQueryBulder.BuildSelectAllQuery<Person>();

            string query = SqlQueryBulder.BuildSelectQuery<Person>(
                x => new { x.Name, x.Email}, 
                x => new { x.Id } );

            //string query = SqlQueryBulder.BuildUpdateQuery<Person>();

            //string query = SqlQueryBulder.BuildUpdateQuery<Person>(x => new { x.Email });

            string insertQuery = SqlQueryBulder.BuildInsertQuery<Person>();

            Console.WriteLine(query);

            Console.ReadLine();
        }
    }

    public class Person
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
    }
}
