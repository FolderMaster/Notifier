﻿namespace ConsoleApp.JiraRules.Extractions
{
    public class FixVersionsFeatureRuleExtraction : BaseReporterRuleExtraction
    {
        public override string Jql => "type = Feature AND fixVersion is EMPTY";
    }
}
