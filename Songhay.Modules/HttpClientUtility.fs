namespace Songhay.Modules

open System.Net.Http

/// <summary>
/// Utility functions for <see cref="HttpClient" />.
/// </summary>
module HttpClientUtility =

    /// <summary>
    /// Calls <see cref="HttpClient.SendAsync(HttpRequestMessage)" />
    /// </summary>
    /// <param name="message">The <see cref="HttpRequestMessage" />.</param>
    /// <param name="client">The <see cref="HttpClient" />.</param>
    let trySendAsync (message: HttpRequestMessage) (client: HttpClient) =
        task {
            try
                try
                    let! response = client.SendAsync(message).ConfigureAwait(false)

                    return Ok response
                with
                | exn -> return Error exn
            finally
                message.Dispose()
        }
