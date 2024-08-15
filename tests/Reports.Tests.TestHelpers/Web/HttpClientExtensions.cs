using AngleSharp.Html.Dom;

namespace Reports.Tests.TestHelpers.Web;

/// <summary>
/// Provides extension methods for HttpClient related operations.
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// Submits a form asynchronously.
    /// </summary>
    /// <param name="client">The HttpClient instance.</param>
    /// <param name="form">The HTML form element to submit.</param>
    /// <param name="submitButton">The HTML element representing the submit button.</param>
    /// <returns>A task representing the response of the form submission.</returns>
    public static async Task<HttpResponseMessage> SubmitFormAsync(
        this HttpClient client,
        IHtmlFormElement form,
        IHtmlElement submitButton)
    {
        var submission = form.GetSubmission(submitButton)!;
        var target = (Uri)submission.Target;
        if (submitButton.HasAttribute("formaction"))
        {
            var formaction = submitButton.GetAttribute("formaction")!;
            target = new Uri(formaction, UriKind.Relative);
        }

        using var requestMessage = new HttpRequestMessage(new HttpMethod(submission.Method.ToString()), target);
        requestMessage.Content = new StreamContent(submission.Body);

        foreach (var header in submission.Headers)
        {
            requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            requestMessage.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return await client.SendAsync(requestMessage);
    }
}
