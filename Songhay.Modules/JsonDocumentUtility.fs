namespace Songhay.Modules

/// <summary>
/// Utility functions for <see cref="JsonDocument" />.
/// </summary>
module JsonDocumentUtility =

    open System.Linq
    open System.Text.Json

    /// <summary>
    /// Wraps <see cref="JsonException" /> property
    /// in <see cref="Error" />.
    /// </summary>
    /// <param name="elementName">The <see cref="JsonElement" /> name.</param>
    let resultError (elementName: string) =
        Error(JsonException $"the expected `{elementName}` element is not here.")

    /// <summary>
    /// Converts the first child <see cref="JsonProperty"/>
    /// of <see cref="JsonDocument.RootElement" />
    /// to its property name when it is <see cref="JsonValueKind.Object"/>
    /// or returns <see cref="None" />.
    /// </summary>
    /// <param name="document">The <see cref="JsonDocument" />.</param>
    let toFirstPropertyName (document: JsonDocument) =
        if document.RootElement.ValueKind <> JsonValueKind.Object then None
        else
            try
                Some (document.RootElement.EnumerateObject().First().Name)
            with | _ -> None

    /// <summary>
    /// Converts the conventional result
    /// to its underlying <see cref="JsonElement"/> of <see cref="JsonValueKind.String"/>,
    /// passing it to the specified <see cref="Result.Ok"/> function.
    /// </summary>
    /// <remarks>
    /// This function will return a <see cref="JsonException"/>
    /// when the <see cref="JsonElement"/> <see cref="string"/> value is null.
    ///
    /// Also recall that <see cref="System.DateTime"/> values appear as strings in JSON.
    /// </remarks>
    let toResultFromStringElement doOk (result: Result<JsonElement,JsonException>) =
        match result with
        | Error ex -> Error ex
        | Ok el when el.ValueKind = JsonValueKind.Null -> Error(JsonException("The expected non-null value is not here."))
        | Ok el when el.ValueKind <> JsonValueKind.String -> Error(JsonException("The expected string value is not here."))
        | Ok el -> Ok (el |> doOk)

    /// <summary>
    /// Tries to return the <see cref="JsonElement" /> property
    /// of the specified <see cref="JsonElement" /> object.
    /// </summary>
    /// <param name="elementName">The <see cref="JsonElement" /> name.</param>
    /// <param name="element">The <see cref="JsonElement" />.</param>
    let tryGetProperty (elementName: string) (element: JsonElement) =
        if element.ValueKind <> JsonValueKind.Object then
            resultError elementName
        else
            match element.TryGetProperty elementName with
            | false, _ -> resultError elementName
            | true, el -> Ok el

    /// <summary>
    /// Tries to return the <see cref="JsonDocument.RootElement" />
    /// after parsing the specified JSON document.
    /// </summary>
    /// <param name="rawDocument">The JSON document.</param>
    let tryGetRootElement (rawDocument: string) =
        try
            let document = rawDocument |> JsonDocument.Parse
            Ok document.RootElement
        with | exn -> Error exn
