module Songhay.Modules.Models.Tests.AppStateHashSetTests

open Xunit

open Songhay.Modules.Models

type MyAppState = | StateOne | StateTwo | StateThree

[<Fact>]
let ``hasState test`` () =

    let actual = AppStateHashSet<MyAppState>.initialize.addStates(StateOne, StateThree)

    actual.hasState StateOne |> Assert.True
    actual.hasState StateTwo |> Assert.False
    actual.hasState StateThree |> Assert.True

[<Fact>]
let ``removeStates test`` () =

    let actual = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
                     .removeStates(StateTwo, StateThree)

    actual.hasState StateOne |> Assert.True
    actual.hasState StateTwo |> Assert.False
    actual.hasState StateThree |> Assert.False

[<Fact>]
let ``should be equal``() =
    let actual1 = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
    let actual2 = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateThree, StateTwo, StateOne)

    (actual1 = actual2) |> Assert.True

[<Fact>]
let ``should not be equal``() =
    let actual1 = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
    let actual2 = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateThree, StateTwo, StateOne)
                     .removeState StateTwo

    Assert.NotEqual(actual1, actual2)

[<Fact>]
let ``toggleState test`` () =

    let actual = AppStateHashSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
                     .toggleState(StateTwo)

    actual.hasState StateTwo |> Assert.False

    (actual.states.Count = 2) |> Assert.True

    let actual = actual.toggleState(StateTwo)

    actual.hasState StateTwo |> Assert.True

    (actual.states.Count = 3) |> Assert.True
