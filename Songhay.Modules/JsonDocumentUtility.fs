namespace Songhay.Modules

/// <summary>
/// Utility functions for <see cref="JsonDocument" />.
/// </summary>
module JsonDocumentUtility =

    open System.Linq
    open System.Text.Json

    open Songhay.Modules.Models

    /// <summary>
    /// Wraps <see cref="JsonException" /> property
    /// in <see cref="Error" />.
    /// </summary>
    /// <param name="elementName">The <see cref="JsonElement" /> name.</param>
    let resultError (elementName: string) =
        Error(JsonException $"the expected `{elementName}` element is not here.")

    /// <summary>
    /// Converts the <see cref="JsonDocumentOrElement" />
    /// to its underlying <see cref="JsonElement"/>.
    /// </summary>
    /// <param name="documentOrElement">The <see cref="JsonDocumentOrElement" />.</param>
    let toJsonElement (documentOrElement: JsonDocumentOrElement) =
            match documentOrElement with
            | JDocument doc -> doc.RootElement
            | JElement el -> el

    /// <summary>
    /// Converts the <see cref="JsonDocument.RootElement" />
    /// to its property name when it is <see cref="JsonValueKind.Object"/>
    /// or returns <see cref="None" />.
    /// </summary>
    /// <param name="document">The <see cref="JsonDocument" />.</param>
    /// <remarks>
    /// The current <see cref="JsonElement.EnumerateObject()"/> method
    /// behaves differently for <see cref="JsonDocument.RootElement"/>
    /// as it returns a <see cref="JsonProperty"/> representing itself
    /// when <see cref="JsonDocument.RootElement.ValueKind"/> is
    /// <see cref="JsonValueKind.Object"/>.
    /// 
    /// As of this writing, descendant <see cref="JsonElement"/> cannot do this.
    /// This implies that a descendant <see cref="JsonElement"/> cannot know what its name is
    /// because it cannot return a <see cref="JsonProperty"/> representing itself.
    /// </remarks>
    let toPropertyName (document: JsonDocument) =
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
    /// </remarks>
    let toResultFromStringElement doOk (result: Result<JsonElement,JsonException>) =
        match result with
        | Error ex -> Error ex
        | Ok el when el.ValueKind = JsonValueKind.Null -> Error(JsonException("The expected date-time value is not here."))
        | Ok el when el.ValueKind <> JsonValueKind.String -> Error(JsonException("The expected date-time serialized type is not here."))
        | Ok el -> Ok (el |> doOk)

    /// <summary>
    /// Tries to return the <see cref="JsonElement" /> property
    /// of the specified <see cref="JsonDocumentOrElement" /> object.
    /// </summary>
    /// <param name="elementName">The <see cref="JsonElement" /> name.</param>
    /// <param name="documentOrElement">The <see cref="JsonDocumentOrElement" />.</param>
    let rec tryGetProperty (elementName: string) (documentOrElement: JsonDocumentOrElement) =
        if not documentOrElement.isJsonValueKindObject then
            resultError elementName
        else
            match documentOrElement with
            | JElement element ->
                match element.TryGetProperty elementName with
                | false, _ -> resultError elementName
                | true, el -> Ok (JElement el)
            | JDocument document ->
                match document |> toPropertyName with
                | None _ -> resultError elementName
                | Some rootName ->
                    match document.RootElement.TryGetProperty rootName with
                    | false, _ -> resultError elementName
                    | true, el -> JElement el |> tryGetProperty elementName

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
