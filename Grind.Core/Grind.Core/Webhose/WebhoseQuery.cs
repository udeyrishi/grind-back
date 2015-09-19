using System;
using System.Text;

namespace Grind.Core
{
	public class WebhoseQuery
	{
		private string url;
		private string token;
		private const String format = "json";
		private string query;
		private int performanceScore;
		private string language;


		public WebhoseQuery (string token, string query, int performanceScore, string language)
		{
			this.token = token;
			this.query = query;
			this.performanceScore = performanceScore;
			this.language = language;
		}

		public String Search()
		{
			this.CreateUrl();
			// Hacer el get http a url
			return url;
		}

		private string CreateUrl()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("https://webhose.io/search");

			//Add token
			sb.Append("?token="+token);
			// Add format
			sb.Append("&format="+format);
			// Add query
			sb.Append("&q="+query.Replace(" ","%20")+"%20");
			// Add performance score
			sb.Append("performance_score%3A%3E"+performanceScore.ToString());
			// Add language
			sb.Append("&language="+language);
			
			this.url = sb.ToString();
			return this.url;
		}
	}
}

