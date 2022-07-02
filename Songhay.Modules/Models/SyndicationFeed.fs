namespace Songhay.Modules.Models

open System

/// <summary>
/// Defines an item of <see cref="SyndicationFeed.feedItems" />.
/// </summary>
type SyndicationFeedItem =
    {
        /// <summary>
        /// The Item title.
        /// </summary>
        title: string

        /// <summary>
        /// The Item link,
        /// a <see cref="System.String" /> representation
        /// of a valid <see cref="System.Uri" />.
        /// </summary>
        link: string

        /// <summary>
        /// The optional brief passage or excerpt
        /// of the Feed Item.
        /// </summary>
        extract: string option

        /// <summary>
        /// The optional Feed Item Publication <see cref="System.DateTime" />.
        /// </summary>
        publicationDate: DateTime option
    }

/// <summary>
/// Represents a simplified variation
/// of Microsoft’s <c>SyndicationFeed</c> class
/// </summary>
/// <remarks>
/// See https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.syndication.syndicationfeed?view=dotnet-plat-ext-6.0
/// </remarks>
type SyndicationFeed =
    {
        /// <summary>
        /// The optional Feed Image link,
        /// a <see cref="System.String" /> representation
        /// of a valid <see cref="System.Uri" />.
        /// </summary>
        feedImage: string option

        /// <summary>
        /// The list
        /// of <see cref="SyndicationFeedItem" />.
        /// </summary>
        feedItems: SyndicationFeedItem list

        /// <summary>
        /// The Feed Title.
        /// </summary>
        feedTitle: string

        /// <summary>
        /// The Feed modification <see cref="System.DateTime" />.
        /// </summary>
        modificationDate: DateTime
    }
