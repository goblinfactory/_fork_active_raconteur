using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayDohs;
using Specs;

// ReSharper disable UnusedMember.Local

namespace Examples.Demo 
{

    public class Employee
    {
        public string 
            FirstName = string.Empty, 
            LastName = string.Empty;
    }

    public partial class EmployeeSearch 
    {
        static readonly List<Employee> ExpectedEmployees = new List<Employee>
        {
            new Employee { FirstName = "Marco", LastName = "Polo" },
            new Employee { FirstName = "Waldo" },
            new Employee { FirstName = "Robinson", LastName = "Crusoe" },
            new Employee { FirstName = "Ali", LastName = "Baba" },
        };

        List<Employee> Employees;

        readonly dynamic EmployeeRepo = new PlayDoh();

        [TestInitialize]
        public void Setup()
        {
            EmployeeRepo
                .Returns(ExpectedEmployees)
                .On.FindByLastName("smith");

            Controller
                .Does((Action) (() => View.Employees = ExpectedEmployees))
                .On.Find();
        }

        void When_I_search_for_existing_Employees() 
        {
            Employees = EmployeeRepo.FindByLastName("smith");
        }
        
        void I_should_be_able_to_find_them() 
        {
            Assert.IsTrue(Employees.Count > 0);
        }

        readonly dynamic EmployeeDAL = new PlayDoh();
        readonly dynamic Browser = new PlayDoh();

        void Given_an_Employee_named(string firstName, string lastName) 
        {
            EmployeeDAL.Insert(firstName, lastName);
        }

        void When_I_search_for_Employees_with_first_name(string firstName) 
        {
            Browser
                .Login()
                .NavigateToEmployeeDirectory()
                .Enter("first_name", firstName)
                .ClickFind();
        }

        void I_should_find_one_with_last_name(string lastName) 
        {
            Browser.AssertExist("last_name", lastName);
        }

        readonly dynamic Controller = new PlayDoh();
        readonly dynamic View = new PlayDoh();

        void When_I_search_for_Employees_whose(string criteria, string oper, string value) 
        {
            View.Criteria = criteria;
            View.Operator = oper;
            View.Value = value;

            Controller.Find();

            Employees = View.Employees;
        }

        void I_should_find_one_whose__is(string property, string value)
        {
            Assert.IsTrue
            (
                Employees.Any(x => x.Has(property, value)),
                "Could not find Employee with " + property + " " + value
            );
        }

        void Given_the_Employees_(string firstName, string lastName)
        {
            Given_an_Employee_named(firstName, lastName);
        }

        void When_I_search_for_Employees_whose_(params string[][] filters) 
        {
            foreach (var filter in filters)
                View.Filters.Add(filter[0], filter[1], filter[2]);

            Controller.Find();

            Employees = View.Employees;
        }

        void I_should_find(string firstName, string lastName) 
        {
            Assert.IsTrue(Employees.Any(x =>
                x.FirstName == firstName && x.LastName == lastName), 
                "Could not find " + firstName + " " + lastName);
        }
    }
}
// ReSharper restore UnusedMember.Local
