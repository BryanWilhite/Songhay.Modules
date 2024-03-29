module Songhay.Modules.Models.Tests.AppStateSetTests

open Xunit
open FsUnit.Xunit

open Songhay.Modules.Models

type MyAppState = | StateOne | StateTwo | StateThree | StateFour of int | StateFive of string

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
