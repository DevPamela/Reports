using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using System.Net.Http.Headers;

namespace Reports.Tests.TestHelpers;

/// <summary>
/// Provides helper methods for working with HTML documents.
/// </summary>
public static class HtmlHelpers
{
    /// <summary>
    /// Retrieves an HTML document asynchronously from the provided HTTP response.
    /// </summary>
    /// <param name="response">The HTTP response containing the HTML content.</param>
    /// <returns>The HTML document as an <see cref="IHtmlDocument"/>.</returns>
    public static async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var document = await BrowsingContext.New()
            .OpenAsync(ResponseFactory, CancellationToken.None);
        return (IHtmlDocument)document;

        void ResponseFactory(VirtualResponse htmlResponse)
        {
            htmlResponse
                .Address(response.RequestMessage?.RequestUri)
                .Status(response.StatusCode);

            MapHeaders(response.Headers);
            MapHeaders(response.Content.Headers);

            htmlResponse.Content(content);

            void MapHeaders(HttpHeaders headers)
            {
                foreach (var header in headers)
                {
                    foreach (var value in header.Value)
                    {
                        htmlResponse.Header(header.Key, value);
                    }
                }
            }
        }
    }
}
