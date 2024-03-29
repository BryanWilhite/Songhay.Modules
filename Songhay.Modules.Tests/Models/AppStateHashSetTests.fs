module Songhay.Modules.Models.Tests.AppStateHashSetTests

open Xunit
open FsUnit.Xunit

open Songhay.Modules.Models

type MyAppState = | StateOne | StateTwo | StateThree

[<Fact>]
let ``hasState test`` () =

    let actual = AppStateHashSet<MyAppState>.initialize.addStates(StateOne, StateThree)

    actual.hasState StateOne |> should be True
    actual.hasState StateTwo |> should be False
    actual.hasState StateThree |> should be True

[<Fact>]
let ``removeStates test`` () =

    let actual = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
                     .removeStates(StateTwo, StateThree)

    actual.hasState StateOne |> should be True
    actual.hasState StateTwo |> should be False
    actual.hasState StateThree |> should be False

[<Fact>]
let ``should be equal``() =
    let actual1 = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
    let actual2 = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateThree, StateTwo, StateOne)

    actual1 |> should equal actual2

[<Fact>]
let ``should not be equal``() =
    let actual1 = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
    let actual2 = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateThree, StateTwo, StateOne)
                     .removeState StateTwo

    actual1 |> should not' <| equal actual2

[<Fact>]
let ``toggleState test`` () =

    let actual = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
                     .toggleState(StateTwo)

    actual.hasState StateTwo |> should be False

    actual.states.Count |> should equal 2

    let actual = actual.toggleState(StateTwo)

    actual.hasState StateTwo |> should be True

    actual.states.Count |> should equal 3
