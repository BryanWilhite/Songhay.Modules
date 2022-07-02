namespace Songhay.Modules

open System
open System.Linq
open System.Collections
open System.Globalization
open System.Net.Http
open System.Text

open Songhay.Modules.MimeTypes

/// <summary>
/// Utility functions for <see cref="HttpRequestMessage" />.
/// </summary>
module HttpRequestMessageUtility =

    /// <summary>
    /// Returns <see cref="HttpRequestMessage" />
    /// for the specified <see cref="Uri" />
    /// and <see cref="HttpMethod" />.
    /// </summary>
    /// <param name="uri">The <see cref="Uri" />.</param>
    /// <param name="method">The <see cref="HttpMethod" />.</param>
    let message (uri: Uri) (method: HttpMethod) =
        new HttpRequestMessage(method, uri)

    /// <summary>
    /// Returns <see cref="HttpRequestMessage" />
    /// for the specified <see cref="Uri" />
    /// and <see cref="HttpMethod.Post" />.
    /// </summary>
    /// <param name="uri">The <see cref="Uri" />.</param>
    let post (uri: Uri) =
        HttpMethod.Post |> message uri

    /// <summary>
    /// Returns <see cref="HttpRequestMessage" />
    /// for the specified <see cref="Uri" />
    /// and <see cref="HttpMethod.Get" />.
    /// </summary>
    /// <param name="uri">The <see cref="Uri" />.</param>
    let get (uri: Uri) =
        HttpMethod.Get |> message uri

    /// <summary>
    /// Returns <see cref="HttpRequestMessage" />
    /// for the specified <see cref="Uri" />
    /// and <see cref="HttpMethod.Put" />.
    /// </summary>
    /// <param name="uri">The <see cref="Uri" />.</param>
    let put (uri: Uri) =
        HttpMethod.Put |> message uri

    /// <summary>
    /// Returns <see cref="HttpRequestMessage" />
    /// for the specified <see cref="Uri" />
    /// and <see cref="HttpMethod.Delete" />.
    /// </summary>
    /// <param name="uri">The <see cref="Uri" />.</param>
    let delete (uri: Uri) =
        HttpMethod.Delete |> message uri

    /// <summary>
    /// Returns <see cref="HttpRequestMessage" />
    /// for the specified <see cref="Uri" />
    /// and <see cref="HttpMethod.Patch" />.
    /// </summary>
    /// <param name="uri">The <see cref="Uri" />.</param>
    let patch (uri: Uri) =
        HttpMethod.Patch |> message uri

    /// <summary>
    /// Returns a <see cref="HttpRequestMessage" />
    /// with <see cref="StringContent" />.
    /// </summary>
    /// <param name="body">The content used to initialize the <see cref="StringContent" />.</param>
    /// <param name="encoding">The encoding to use for the content.</param>
    /// <param name="mediaType">The media type to use for the content.</param>
    /// <param name="message">The <see cref="HttpRequestMessage" />.</param>
    let withStringContent (body: string) (encoding: Encoding) (mediaType: string) (message: HttpRequestMessage) =
        message.Content <- new StringContent(body, encoding, mediaType)

        message

    /// <summary>
    /// Returns a <see cref="HttpRequestMessage" />
    /// with <see cref="StringContent" />
    /// and media-type <see cref="ApplicationFormUrlEncoded" />.
    /// </summary>
    /// <param name="formData">The <see cref="Hashtable" /> used to initialize the <see cref="StringContent" />.</param>
    /// <param name="message">The <see cref="HttpRequestMessage" />.</param>
    let withHtmlFormContent (formData: Hashtable) (message: HttpRequestMessage) =
        let body =
            let sb = StringBuilder()

            formData.OfType<DictionaryEntry>()
            |> Seq.iteri (fun i entry ->
                let s =
                    if i = 0 then String.Format(CultureInfo.InvariantCulture, "{0}={1}", entry.Key, entry.Value)
                    else String.Format(CultureInfo.InvariantCulture, "&{0}={1}", entry.Key, entry.Value)
                sb.Append(s) |> ignore
            )

            sb.ToString()

        message |> withStringContent body Encoding.UTF8 ApplicationFormUrlEncoded

    /// <summary>
    /// Returns a <see cref="HttpRequestMessage" />
    /// with <see cref="StringContent" />
    /// and media-type <see cref="ApplicationJson" />.
    /// </summary>
    /// <param name="body">The content used to initialize the <see cref="StringContent" />.</param>
    /// <param name="message">The <see cref="HttpRequestMessage" />.</param>
    let withJsonStringContent (body: string) (message: HttpRequestMessage) =
        message |> withStringContent body Encoding.UTF8 ApplicationJson

    /// <summary>
    /// Returns a <see cref="HttpRequestMessage" />
    /// with <see cref="StringContent" />
    /// and media-type <see cref="ApplicationXml" />.
    /// </summary>
    /// <param name="body">The content used to initialize the <see cref="StringContent" />.</param>
    /// <param name="message">The <see cref="HttpRequestMessage" />.</param>
    let withXmlStringContent (body: string) (message: HttpRequestMessage) =
        message |> withStringContent body Encoding.UTF8 ApplicationXml
