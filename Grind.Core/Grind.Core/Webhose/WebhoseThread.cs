using System;

namespace Grind.Core
{
	public class WebhoseThread
	{
		private string url;
		private string siteFull;
		private string site;
		private string siteSelection;
		private string selectionTitle;
		private string title;
		private string titleFull;
		private string published;
		private string repliesCount;
		private string participantsCount;
		private string siteType;
		private string mainImage;
		private string country;

		public WebhoseThread (
			string url,
			string siteFull,
			string site,
			string siteSelection,
			string selectionTitle,
			string title,
			string titleFull,
			string published,
			string repliesCount,
			string participantsCount,
			string siteType,
			string mainImage,
			string country)
		{
			this.url = url;
			this.siteFull = siteFull;
			this.site = site;
			this.siteSelection = siteSelection;
			this.selectionTitle = selectionTitle;
			this.title = title;
			this.titleFull = titleFull;
			this.published = published;
			this.repliesCount = repliesCount;
			this.participantsCount = participantsCount;
			this.siteType = siteType;
			this.mainImage = mainImage;
			this.country = country;
		}

		public string GetUrl()
		{
			return this.url;
		}

		public string GetSiteFull()
		{
			return this.siteFull;
		}

		public string GetSite()
		{
			return this.site;
		}

		public string GetSiteSelection()
		{
			return this.siteSelection;
		}

		public string GetSelectionTitle()
		{
			return this.GetSelectionTitle;
		}

		public string GetTitle()
		{
			return this.title;
		}

		public string GetTitleFull()
		{
			return this.titleFull;
		}

		public string GetPublished()
		{
			return this.published;
		}

		public string GetRepliesCount()
		{
			return this.repliesCount;
		}

		public string GetParticipantsCount()
		{
			return this.participantsCount;
		}

		public string GetSiteType()
		{
			return this.siteType;
		}

		public string GetMainImage()
		{
			return this.mainImage;
		}

		public string GetCountry()
		{
			return this.country;
		}
	}
}

