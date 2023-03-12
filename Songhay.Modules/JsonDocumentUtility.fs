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
        Error <| JsonException $"the expected `{elementName}` element is not here."

    /// <summary>
    /// Converts the first child <see cref="JsonProperty"/>
    /// of <see cref="JsonDocument.RootElement" />
    /// to its property name when it is <see cref="JsonValueKind.Object"/>
    /// or returns <see cref="None" />.
    /// </summary>
    /// <param name="document">The <see cref="JsonDocument" />.</param>
    let toFirstPropertyName (document: JsonDocument) =
        match document.RootElement.ValueKind with
        | JsonValueKind.Object ->
            try
                Some <| document.RootElement.EnumerateObject().First().Name
            with | _ -> None
        | _ -> None

    /// <summary>
    /// Converts the conventional result
    /// to its underlying <see cref="JsonElement"/>
    /// of the specified <see cref="JsonValueKind"/>,
    /// passing it to the specified <see cref="Result.Ok"/> function.
    /// </summary>
    /// <remarks>
    /// This function will return a <see cref="JsonException"/>
    /// when the <see cref="JsonElement"/> <see cref="string"/> value is null.
    ///
    /// Also, recall that <see cref="System.DateTime"/> values appear as strings in JSON.
    /// </remarks>
    let toResultFromJsonElement (isKind: JsonValueKind -> bool) doOk (result: Result<JsonElement,JsonException>) =
        match result with
        | Error ex -> Error ex
        | Ok el when el.ValueKind |> isKind -> el |> doOk |> Ok
        | Ok el -> Error <| JsonException($"The expected {nameof(JsonValueKind)} is not here: {el.ValueKind}")

    /// <summary>
    /// Converts the conventional result
    /// to its underlying <see cref="JsonElement"/>
    /// of the specified <see cref="JsonValueKind.True"/> or <see cref="JsonValueKind.False"/>,
    /// passing it to the specified <see cref="Result.Ok"/> function.
    /// </summary>
    let toResultFromBooleanElement doOk (result: Result<JsonElement,JsonException>) =
        toResultFromJsonElement (fun kind -> kind = JsonValueKind.True || kind = JsonValueKind.False) doOk result

    /// <summary>
    /// Converts the conventional result
    /// to its underlying <see cref="JsonElement"/>
    /// of the specified <see cref="JsonValueKind.Number"/>,
    /// passing it to the specified <see cref="Result.Ok"/> function.
    /// </summary>
    let toResultFromNumericElement doOk (result: Result<JsonElement,JsonException>) =
        toResultFromJsonElement (fun kind -> kind = JsonValueKind.Number) doOk result

    /// <summary>
    /// Converts the conventional result
    /// to its underlying <see cref="JsonElement"/>
    /// of the specified <see cref="JsonValueKind.String"/>,
    /// passing it to the specified <see cref="Result.Ok"/> function.
    /// </summary>
    let toResultFromStringElement doOk (result: Result<JsonElement,JsonException>) =
        toResultFromJsonElement (fun kind -> kind = JsonValueKind.String) doOk result

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
