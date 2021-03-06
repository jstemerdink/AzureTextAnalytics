﻿namespace AzureTextAnalytics.Domain.Azure
{
    using System.Collections.Generic;

    public class AzureSentimentBatchResult
    {
        public IEnumerable<AzureSentimentResult> SentimentBatch { get; set; }

        public IEnumerable<AzureErrorRecord> Errors { get; set; }
    }
}