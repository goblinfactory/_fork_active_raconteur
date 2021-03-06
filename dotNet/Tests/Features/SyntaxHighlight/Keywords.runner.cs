using MbUnit.Framework;
using Features.StepDefinitions;

namespace Features.SyntaxHighlight 
{
    [TestFixture]
    public partial class HighlightKeywords 
    {
        public FeatureRunner FeatureRunner = new FeatureRunner();
        public HighlightFeature HighlightFeature = new HighlightFeature();

        
        [Test]
        public void FeatureAndScenarios()
        {         
            FeatureRunner.Given_the_Feature_is(
@"
Feature: Name
Scenario: First
Scenario: Second
");        
            HighlightFeature.Raconteur_should_highlight(1, "Feature:", "Keyword");        
            HighlightFeature.Raconteur_should_highlight(2, "Scenario:", "Keyword");
        }

    }
}
