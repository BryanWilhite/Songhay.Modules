namespace Songhay.Modules

open System.IO
open System.Net
open System.Net.Http
open System.Text.Json

/// <summary>
/// Utility functions for <see cref="HttpResponseMessage" />.
/// </summary>
module HttpResponseMessageUtility =

    /// <summary>
    /// Returns true when <see cref="HttpResponseMessage.StatusCode" />
    /// is <see cref="HttpStatusCode.Moved" />
    /// or <see cref="HttpStatusCode.MovedPermanently" />
    /// or <see cref="HttpStatusCode.Redirect" />.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage" />.</param>
    let isMovedOrRedirected (response: HttpResponseMessage) =
        response.StatusCode = HttpStatusCode.Moved ||
        response.StatusCode = HttpStatusCode.MovedPermanently ||
        response.StatusCode = HttpStatusCode.Redirect

    /// <summary>
    /// Tries to download a <see cref="byte[]" />
    /// with <see cref="HttpResponseMessage.Content.ReadAsByteArrayAsync" />.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage" />.</param>
    let tryDownloadToByteArrayAsync (response: HttpResponseMessage) =
        task {
            try
                if response.IsSuccessStatusCode then
                    let! s = response.Content.ReadAsByteArrayAsync().ConfigureAwait(false)

                    return Ok s
                else
                    return Error response.StatusCode
            finally
                response.Dispose()
        }

    /// <summary>
    /// Tries to download a file
    /// with <see cref="HttpResponseMessage.Content.ReadAsStreamAsync" />
    /// to the specified path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="response">The <see cref="HttpResponseMessage" />.</param>
    let tryDownloadToFileAsync (path: string) (response: HttpResponseMessage) =
        let buffer = Array.init 32768 (fun i -> byte(i*i))
        let mutable bytesRead = -1
        task {
            try
                if response.IsSuccessStatusCode then
                    try
                        use! stream = response.Content.ReadAsStreamAsync().ConfigureAwait(false)
                        use fs = File.Create(path)
                        while (not (bytesRead > 0)) do
                            bytesRead <- stream.Read(buffer, 0, buffer.Length)
                            fs.Write(buffer, 0, bytesRead)

                        return Ok ()
                    with
                    | exn -> return Error exn
                else
                    return Error (failwith $"{nameof response}: {response.StatusCode}")
            finally
                response.Dispose()
        }

    /// <summary>
    /// Tries to download a <see cref="System.String" />
    /// with <see cref="HttpResponseMessage.Content.ReadAsStringAsync" />.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage" />.</param>
    let tryDownloadToStringAsync (response: HttpResponseMessage) =
        task {
            try
                if response.IsSuccessStatusCode then
                    let! s = response.Content.ReadAsStringAsync().ConfigureAwait(false)
                    return Ok s
                else
                    return Error response.StatusCode
            finally
                response.Dispose()
        }

    /// <summary>
    /// Tries to download an instance hydrated
    /// from <see cref="JsonSerializer.DeserializeAsync{_}" />
    /// with <see cref="HttpResponseMessage.Content.ReadAsStreamAsync" />.
    /// </summary>
    /// <param name="options">The <see cref="JsonSerializerOptions" />.</param>
    /// <param name="response">The <see cref="HttpResponseMessage" />.</param>
    let tryStreamToInstanceAsync (options: JsonSerializerOptions) (response: HttpResponseMessage) =
        task {
            try
                if response.IsSuccessStatusCode then
                    use! stream = response.Content.ReadAsStreamAsync().ConfigureAwait(false)

                    if (stream = null || stream.CanRead = false) then

                        return Error "The expected stream is not here."
                    else
                        try
                            let! instance = JsonSerializer.DeserializeAsync<_>(stream, options).ConfigureAwait(false)

                            return Ok instance
                        with
                        | exn -> return Error exn.Message
                else
                    return Error (failwith $"{nameof response}: {response.StatusCode}")
            finally
                response.Dispose()
        }
