using System;

namespace Grind.Core
{
	public class WebhoseResponse
	{
		private int totalResults;

		private int moreResultsAvailable;

		private string next;

		private int requestsLeft;	

		private List<WebhosePost> posts;

		public WebhoseResponse ()
		{
		}
	}
}

