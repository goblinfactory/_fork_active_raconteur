﻿using System.Collections.Generic;
using Common;
using FluentSpec;
using NUnit.Framework;
using Raconteur;
using Raconteur.Helpers;
using Raconteur.Parsers;

namespace Specs
{
    [TestFixture]
    public class KeepFeatureLocation
    {
        [Test]

        [TestCase("One Scenario",
            @"
                Scenario: One
            ",
            0, "Scenario: One")]

        [TestCase("Many single line Scenarios",
            @"
                Scenario: One
                Scenario: Two
                Scenario: Three
            ",
            1, "Scenario: Two")]

        [TestCase("Scenarios with tags",
            @"
                Scenario: One
                Scenario: Two
                Scenario: Three
            ",
            1, "Scenario: Two")]

        [TestCase("Scenarios with tags",
            @"
                Scenario: One
                    Step

                @tag
                Scenario: Another
            ",
            1, "@tag")]

        public void Tokenizer_should_include_Scenario_location
        (
            string Example, 
            string Content, 
            int ExpectedIndex, 
            string ExpectedContent
        )
        {
            new ScenarioTokenizerClass
            {
                Content =
                @"
                    Feature: Name
                "
                + Content
            }
            .ScenarioDefinitions[ExpectedIndex].Item2
                .Content.ShouldStartWith(ExpectedContent);
        }

        [Test]
        public void ScenarioParser_should_include_location_in_Scenario()
        {
            var expectedLocation = new Location();

            new ScenarioParserClass()
                .ScenarioFrom
                (
                    new List<string> { "Scenario: Name" }.AsLocations(), 
                    expectedLocation
                )
            .Location.ShouldBe(expectedLocation);
        }

        [Test]

        [TestCase("Simple Step", 0,
            @"
                Step
            ",
            "Step")]

        [TestCase("Repeated Step", 1,
            @"
                Step
                Step
            ",
            "Step")]

        [TestCase("Two Steps", 1,
            @"
                one Step
                another Step
            ",
            "another Step")]

        [TestCase("Step with Args", 0,
            @"
                ""Arg"" Step ""Arg"" 
            ",
            @"""Arg"" Step ""Arg""")]

        [TestCase("Step with Multiline Args", 0,
            @"
                A multiline step arg
                ""
                    line 1
                    line 2
                ""
            ",
            "A multiline step arg")]

        [TestCase("Step with all kind of Args", 4,
            @"
                ""
                    line 1
                    line 2
                ""
                A multiline ""Arg"" step arg
                ""
                    line 1
                    line 2
                ""
            ",
            @"A multiline ""Arg"" step arg")]

        public void ScenarioTokenizer_should_include_Step_locations
        (
            string Example,
            int StepLocation, 
            string Steps, 
            string StepLocationContent
        )
        {
            new ScenarioTokenizerClass
            {
                Content =
                @"
                    Feature: Name

                    Scenario: Name
                "
                + Steps
            }
            .ScenarioDefinitions[0].Item1[StepLocation + 1]
                .Content.ShouldBe(StepLocationContent);

        }
    }
}