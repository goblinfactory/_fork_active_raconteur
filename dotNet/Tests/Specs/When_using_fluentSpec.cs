using System;
using NUnit.Framework;

namespace Specs
{
    public class When_using_fluentSpec
    {
        static string Name;
        static int Age;

        [SetUp]
        public void SetUpFeatureStep()
        {
            Name = "Fred";
            Age = 10;
        }

        [TestFixture]
        public class Given_PersonX  : FluentSpec.BehaviorOf<PersonX>
        {
            [Test]
            public void ShouldBeAbleToReferToAllPublicMethods()
            {
                When.PrintBadge();
                Should.GETAge();
                Should.GETName();
            }
        }


        public class PersonX
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public int GETAge()
            {
                return 5;
            }
            public string GETName() { return "Sally"; }
            public void PrintBadge()
            {
                int age = GETAge();
                string name = "boo!"; // = GETName();
                Console.WriteLine("Age:{0} Name:{1}", age, name);
            }
        }

    }


}