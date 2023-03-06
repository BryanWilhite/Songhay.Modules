namespace Songhay.Modules.Models

open System.Text.Json

/// <summary>
/// Defines a <see cref="JsonDocument"/>
/// or a <see cref="JsonElement"/>
/// </summary>
/// <remarks>
/// For way too much detail, see my notes
/// [ https://github.com/BryanWilhite/jupyter-central/blob/master/funkykb/fsharp/json/system.text.json.ipynb ]
/// </remarks>
type JsonDocumentOrElement =
    | JDocument of JsonDocument
    | JElement of JsonElement
