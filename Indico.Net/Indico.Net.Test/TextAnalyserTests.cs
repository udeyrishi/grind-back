﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Indico.Net.Test
{
    [TestClass]
    public class TextAnalyserTests
    {
        private static readonly TextAnalyser analyser = new TextAnalyser("http://apiv2.indico.io/", "a037bd9184676c6c70ab32a6e373309c");

        [TestMethod]
        public async Task AnalyseSentiment_PositiveString_Works()
        {
            double result = await analyser.AnalyseSentimentAsync("This is good.");
            result.Should().BeGreaterThan(0.9);
        }

        [TestMethod]
        public async Task AnalyseSentiment_NegativeString_Works()
        {
            double result = await analyser.AnalyseSentimentAsync("This is bad.");
            result.Should().BeLessThan(0.1);
        }


        [TestMethod]
        public async Task AnalyseSentiment_Batch_Works()
        {
            double[] result = await analyser.AnalyseSentimentAsync(new[] { "This is bad.", "This is good." } );
            result[0].Should().BeLessThan(0.1);
            result[1].Should().BeGreaterThan(0.9);
        }

        [TestMethod]
        public async Task AnalyseSentimentHq_PositiveString_Works()
        {
            double result = await analyser.AnalyseSentimentHighQualityAsync("This is good.");
            result.Should().BeGreaterThan(0.9);
        }

        [TestMethod]
        public async Task AnalyseSentimentHq_NegativeString_Works()
        {
            double result = await analyser.AnalyseSentimentHighQualityAsync("This is bad.");
            result.Should().BeLessThan(0.1);
        }


        [TestMethod]
        public async Task AnalyseSentimentHq_Batch_Works()
        {
            double[] result = await analyser.AnalyseSentimentHighQualityAsync(new[] { "This is bad.", "This is good." });
            result[0].Should().BeLessThan(0.1);
            result[1].Should().BeGreaterThan(0.9);
        }

        [TestMethod]
        public async Task TextTags_Works()
        {
            Dictionary<string, double> result = await analyser.GetTextTagsAsync("This is bad.");
            result.Count.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public async Task TextTags_WithOptionalValues_Works()
        {
            Dictionary<string, double> result = await analyser.GetTextTagsAsync(
                "Blog posts about Android tech make better journalism than most news outlets.", 5, 0.1);
            result.Count.Should().BeLessOrEqualTo(5);
            foreach(var kvp in result)
            {
                kvp.Value.Should().BeGreaterThan(0.1);
            }
        }

        [TestMethod]
        public async Task TextTagsBatch_Works()
        {
            Dictionary<string, double>[] result = await analyser.GetTextTagsAsync(new string[] { "This is bad.", "This is good." });
            result[0].Count.Should().BeGreaterThan(0);
            result[1].Count.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public async Task TextTagsBatch_WithOptionalValues_Works()
        {
            Dictionary<string, double>[] results = await analyser.GetTextTagsAsync(
                new string[] { "Blog posts about Android tech make better journalism than most news outlets.",
                "We\u0027re supposed to get up to 24 inches of snow in the storm.."}, 5, 0.1);
            foreach (var result in results)
            {
                result.Count.Should().BeLessOrEqualTo(5);
                foreach (var kvp in result)
                {
                    kvp.Value.Should().BeGreaterThan(0.1);
                }
            }
        }
    }
}
