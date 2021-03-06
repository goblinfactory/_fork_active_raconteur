using System;
using System.Collections.Generic;
using Common;
using FluentSpec;
using NUnit.Framework;
using Raconteur;
using Raconteur.Helpers;
using Raconteur.Parsers;

namespace Specs
{

    [SetUpFixture]
    public class When_testing_Steps_with_Tables
    {
        static Feature Feature;
        static Step Step;

        [SetUp]
        public void SetUpFeatureStep() 
        {
            Feature = Actors.Feature;
            Step = Feature.Scenarios[0].Steps[0];
        }

        [TestFixture]
        public class The_parser : BehaviourOf<StepParserClass>
        {
            [Test] 
            public void should_associate_table_and_step()
            {
                Step = 
                    When.StepFrom("Step table:".AsLocation());
                     And.StepFrom("|X|Y|".AsLocation());
                     And.StepFrom("|2|1|".AsLocation());
                
                Step.Table.Rows.Count.ShouldBe(2);
            }

            [Test] 
            public void should_turn_single_column_table_into_single_row()
            {
                var Step = 
                When.StepFrom("Step table:".AsLocation());
                 And.StepFrom("|0|".AsLocation());
                 And.StepFrom("|1|".AsLocation());
                 And.StepFrom("|2|".AsLocation());
                
                Step.Table.Rows.Count.ShouldBe(1);
            }

            [Test] 
            public void should_include_header_in_Table()
            {
                var Step = 
                When.StepFrom("Step table:".AsLocation());
                 And.StepFrom("[ X | Y ]".AsLocation());

                var Table = Step.Table;

                Table.Rows.Count.ShouldBe(1);
                Table.HasHeader.ShouldBeTrue();
                Table.Header.Count.ShouldBe(2);
            }
        }

        [TestFixture]
        public class by_default
        {
            string Runner;
            
            [SetUp]
            public void SetUp() 
            {
                Step.Table = new Table
                {
                    Rows = new List<List<string>>
                    {
                        new List<string> {"1", "1"},
                        new List<string> {"3", "4"}
                    },
                };

                Runner = ObjectFactory.NewRunnerGenerator(Feature).Code;            
            }

            [Test]
            public void should_create_a_step_and_pass_the_table()
            {
                Runner.ShouldContain(Step.Name);
                Runner.ShouldContain(@"new[] {1, 1},");
                Runner.ShouldContain(@"new[] {3, 4}");
            }
        }

        [TestFixture]
        public class with_Header
        {
            string Runner;
            
            [SetUp]
            public void SetUp() 
            {
                Step.Table = new Table
                {
                    HasHeader = true,
                    Rows = new List<List<string>>
                    {
                        new List<string> {"X", "Y"},
                        new List<string> {"1", "1"},
                        new List<string> {"3", "4"}
                    },
                };

                Runner = ObjectFactory.NewRunnerGenerator(Feature).Code;            
            }

            [Test]
            public void should_skip_the_first_row()
            {
                Runner.ShouldNotContain(Step.Name + @"(""X"", ""Y"");");
            }

            [Test]
            public void should_create_a_step_for_every_row_with_cols_as_Args()
            {
                Runner.ShouldContain(Step.Name + @"(1, 1);");
                Runner.ShouldContain(Step.Name + @"(3, 4);");
            }
        }

        [TestFixture]
        public class Steps_with_Args
        {
            [SetUp]
            public void SetUp() 
            {
                Step.Table = new Table
                {
                    HasHeader = true,
                    Rows = new List<List<string>>
                    {
                        new List<string> {"X", "Y"},
                        new List<string> {"1", "2"},
                        new List<string> {"3", "4"}
                    }
                };
            }

            [Test]
            public void should_combine_Table_Args_with_col_as_Args()
            {
                Step.Args.Add("arg");

                var Runner = ObjectFactory.NewRunnerGenerator(Feature).Code;            

                Runner.ShouldContain(Step.Name + @"(""arg"", 1, 2);");
            }
        }
    }
}