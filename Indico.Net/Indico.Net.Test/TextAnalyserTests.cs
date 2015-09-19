﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Threading.Tasks;

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
    }
}
