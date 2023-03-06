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
    let toPropertyName (document: JsonDocument) =
        if document.RootElement.ValueKind <> JsonValueKind.Object then None
        else
            try
                Some (document.RootElement.EnumerateObject().First().Name)
            with | _ -> None

    /// <summary>
    /// Tries to return the <see cref="JsonElement" /> property
    /// of the specified <see cref="JsonDocumentOrElement" /> object.
    /// </summary>
    /// <param name="elementName">The <see cref="JsonElement" /> name.</param>
    /// <param name="documentOrElement">The <see cref="JsonDocumentOrElement" />.</param>
    let rec tryGetProperty (elementName: string) (documentOrElement: JsonDocumentOrElement) =
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
