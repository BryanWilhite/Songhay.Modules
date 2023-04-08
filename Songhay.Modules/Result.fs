namespace Songhay.Modules

open System
open System.Text.Json

/// <summary>
/// Additional functions for <see cref="Result{_,_}"/>
/// </summary>
/// <remarks>
/// The functions here are primarily intended
/// for supporting <see cref="JsonDocumentUtility"/>.
/// </remarks>
[<RequireQualifiedAccess>]
module Result =

    let private getFormatException (typeName: string) = FormatException $"The expected {typeName} value is not here."

    /// <summary>
    /// Maps <see cref="Result{_,_}"/>
    /// to <see cref="JsonException"/>.
    /// </summary>
    let mapToJsonException (r: Result<_,_>) =
        r
        |> Result.mapError (fun ex -> JsonException("See inner exception.", ex))

    /// <summary>
    /// Parses the specified <see cref="string"/>
    /// into <see cref="Result{bool,FormatException}"/>
    /// </summary>
    let parseBoolean (s: string) =
        match s |> Boolean.TryParse with
        | true, b -> b |> Ok
        | _ -> nameof(Boolean) |> getFormatException |> Error

    /// <summary>
    /// Parses the specified <see cref="string"/>
    /// into <see cref="Result{DateTime,FormatException}"/>
    /// </summary>
    let parseDateTime (s: string) =
        match s |> DateTime.TryParse with
        | true, dt -> dt |> Ok
        | _ -> nameof(DateTime) |> getFormatException |> Error

    /// <summary>
    /// Parses the specified <see cref="string"/>
    /// into <see cref="Result{Double,FormatException}"/>
    /// </summary>
    let parseDouble (s: string) =
        match s |> Double.TryParse with
        | true, f -> Ok f
        | _ -> nameof(Double) |> getFormatException |> Error

    /// <summary>
    /// Parses the specified <see cref="string"/>
    /// into <see cref="Result{JsonDocument,FormatException}"/>
    /// </summary>
    let parseJsonDocument (s: string) =
        try s |> JsonDocument.Parse |> Ok with
        | :? JsonException as ex -> ex |> Error

    /// <summary>
    /// Parses the specified <see cref="string"/>
    /// into <see cref="Result{Int32,FormatException}"/>
    /// </summary>
    let parseInt32 (s: string) =
        match s |> Int32.TryParse with
        | true, i -> Ok i
        | _ -> nameof(Int32) |> getFormatException |> Error

    /// <summary>
    /// Parses the specified <see cref="string"/>
    /// into <see cref="Result{Uri,FormatException}"/>
    /// </summary>
    let parseUri (uriKind: UriKind) (s: string) =
        match Uri.TryCreate(s, uriKind) with
        | true, uri -> Ok uri
        | _ -> nameof(Uri) |> getFormatException |> Error
