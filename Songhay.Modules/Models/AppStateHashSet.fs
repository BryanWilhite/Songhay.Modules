namespace Songhay.Modules.Models

open System
open System.Collections.Generic

/// <summary>defines the <see cref="'state"/> collection, naming all states off the app</summary>
[<NoComparison; CustomEquality>]
type AppStateHashSet<'state when 'state : comparison> =
    /// <summary>the <see cref="'state"/> collection</summary>
    | AppStates of HashSet<'state>

    interface IEquatable<AppStateHashSet<'state>> with
        member this.Equals other = other.states.SetEquals(this.states)

    /// <summary>returns an empty collection of <see cref="'state"/></summary>
    static member initialize = AppStates <| HashSet<'state>()

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    override this.Equals other =
        match other with
        | :? AppStateHashSet<'state> as p -> (this :> IEquatable<_>).Equals p
        | _ -> false

    /// <summary>Serves as the default hash function.</summary>
    override this.GetHashCode () = this.states.GetHashCode()

    /// <summary>adds the specified <see cref="'state"/></summary>
    member this.addState state =
        this.states.Add(state) |> ignore
        AppStates <| this.states

    /// <summary>adds the specified list of <see cref="'state"/></summary>
    member this.addStates ([<ParamArray>]states :'state[]) =
        states |> Array.iter (fun state -> this.addState state |> ignore)
        AppStates <| this.states

    /// <summary>chooses the first <see cref="'state"/> with the specified getter</summary>
    member this.chooseState (getter: 'state -> 'o option) =
            this.states
            |> List.ofSeq
            |> List.choose getter
            |> List.head

    /// <summary>returns <c>true</c> when the specified <see cref="'state"/> is collected</summary>
    member this.hasState state = this.states.Contains(state)

    /// <summary>removes the specified <see cref="'state"/></summary>
    member this.removeState state =
        this.states.Remove(state) |> ignore
        AppStates <| this.states

    /// <summary>adds the specified list of <see cref="'state"/></summary>
    member this.removeStates ([<ParamArray>]states :'state[]) =
        states |> Array.iter (fun state -> this.removeState state |> ignore)
        AppStates <| this.states

    /// <summary>returns the underlying <see cref="HashSet{T}"/></summary>
    member this.states: HashSet<'state> = let (AppStates set) = this in set

    /// <summary>toggles the specified <see cref="'state"/></summary>
    member this.toggleState state =
        if this.hasState state then this.removeState state
        else this.addState state
