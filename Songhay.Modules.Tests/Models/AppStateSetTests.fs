module Songhay.Modules.Models.Tests.AppStateSetTests

open Xunit
open FsUnit.Xunit

open Songhay.Modules.Models

type MyAppState = | StateOne | StateTwo | StateThree | StateFour of int | StateFive of string

[<Fact>]
let ``chooseState test`` () =

    let set = AppStateSet<MyAppState>.initialize.addStates(StateOne, StateThree)

    //List.find with auto-generated `.Is*` properties and shorthand
    let find = set.states
            |> List.ofSeq
            |> List.find _.IsStateOne

    find |> should equal StateOne // will throw exception when item not found

    //List.choose with `if` and auto-generated `.Is*` properties
    let choose = set.states
                |> List.ofSeq
                |> List.choose (fun i -> if i.IsStateOne then Some StateOne else None)

    choose |> should equal [StateOne] // returns item(s) in a list

    //List.choose with `match`
    let choose = set.states
                |> List.ofSeq
                |> List.choose (fun i -> match i with | StateOne -> Some StateOne | _ ->  None)

    choose |> should equal [StateOne]

    //List.choose with `function` shorthand
    let choose = set.states
                |> List.ofSeq
                |> List.choose (function | StateOne -> Some StateOne | _ -> None)

    choose |> should equal [StateOne]

    let actual = set.chooseState (function | StateOne -> Some StateOne | _ -> None)

    actual |> should equal StateOne

[<Fact>]
let ``hasState test`` () =

    let actual = AppStateSet<MyAppState>.initialize.addStates(StateOne, StateThree)

    actual.hasState StateOne |> should be True
    actual.hasState StateTwo |> should be False
    actual.hasState StateThree |> should be True

[<Fact>]
let ``removeStates test`` () =

    let actual = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
                     .removeStates(StateTwo, StateThree)

    actual.hasState StateOne |> should be True
    actual.hasState StateTwo |> should be False
    actual.hasState StateThree |> should be False

[<Fact>]
let ``toggleState with addStates test`` () =

    let actual = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
                     .toggleState(StateTwo)

    actual.hasState StateTwo |> should be False

    actual.states.Count |> should equal 2

    let actual = actual.toggleState(StateTwo)

    actual.hasState StateTwo |> should be True

    actual.states.Count |> should equal 3

[<Fact>]
let ``toggleState with only initialize test`` () =

    let actual = AppStateSet<MyAppState>
                     .initialize
                     .toggleState(StateTwo)

    actual.hasState StateTwo |> should be True

[<Fact>]
let ``equality test`` () =

    let one = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateThree)

    let two = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateThree)

    one |> should equal two

[<Fact>]
let ``inequality test`` () =

    let one = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo)

    let two = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateThree)

    one |> should not' <| equal two

[<Fact>]
let ``primitive obsession equality test`` () =

    let one = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateFour 1, StateFive "one")

    let two = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateFour 1, StateFive "two")

    one.states |> should not' <| equal two.states
