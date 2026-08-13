module Songhay.Modules.Tests.Models.AppStateSetTests

open Xunit

open Songhay.Modules.Models

type MyAppState = | StateOne | StateTwo | StateThree | StateFour of int | StateFive of string

[<Fact>]
let ``chooseState test`` () =

    let set = AppStateSet<MyAppState>.initialize.addStates(StateOne, StateThree)

    //List.find with auto-generated `.Is*` properties and shorthand
    let find = set.states
            |> List.ofSeq
            |> List.find _.IsStateOne

    Assert.Equal(find, StateOne) // will throw exception when item not found

    //List.choose with `if` and auto-generated `.Is*` properties
    let choose = set.states
                |> List.ofSeq
                |> List.choose (fun i -> if i.IsStateOne then Some StateOne else None)

    Assert.True(choose.Equals [StateOne]) // returns item(s) in a list

    //List.choose with `match`
    let choose = set.states
                |> List.ofSeq
                |> List.choose (fun i -> match i with | StateOne -> Some StateOne | _ ->  None)

    Assert.True(choose.Equals [StateOne])

    //List.choose with `function` shorthand
    let choose = set.states
                |> List.ofSeq
                |> List.choose (function | StateOne -> Some StateOne | _ -> None)

    Assert.True(choose.Equals [StateOne])

    let actual = set.chooseState (function | StateOne -> Some StateOne | _ -> None)

    Assert.Equal(actual, StateOne)

[<Fact>]
let ``hasState test`` () =

    let actual = AppStateSet<MyAppState>.initialize.addStates(StateOne, StateThree)

    actual.hasState StateOne |> Assert.True
    actual.hasState StateTwo |> Assert.False
    actual.hasState StateThree |> Assert.True

[<Fact>]
let ``removeStates test`` () =

    let actual = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
                     .removeStates(StateTwo, StateThree)

    actual.hasState StateOne |> Assert.True
    actual.hasState StateTwo |> Assert.False
    actual.hasState StateThree |> Assert.False

[<Fact>]
let ``toggleState with addStates test`` () =

    let actual = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo, StateThree)
                     .toggleState(StateTwo)

    actual.hasState StateTwo |> Assert.False

    Assert.Equal(actual.states.Count, 2)

    let actual = actual.toggleState(StateTwo)

    actual.hasState StateTwo |> Assert.True

    Assert.Equal(actual.states.Count, 3)

[<Fact>]
let ``toggleState with only initialize test`` () =

    let actual = AppStateSet<MyAppState>
                     .initialize
                     .toggleState(StateTwo)

    actual.hasState StateTwo |> Assert.True

[<Fact>]
let ``equality test`` () =

    let one = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateThree)

    let two = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateThree)

    Assert.Equal(one, two)

[<Fact>]
let ``inequality test`` () =

    let one = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateTwo)

    let two = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateOne, StateThree)

    Assert.False(one.Equals two)

[<Fact>]
let ``primitive obsession equality test`` () =

    let one = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateFour 1, StateFive "one")

    let two = AppStateSet<MyAppState>
                     .initialize
                     .addStates(StateFour 1, StateFive "two")

    Assert.False(one.states.Equals two.states)
