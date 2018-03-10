namespace Ionide.VSCode.FSharp

open Ionide.VSCode.Helpers
open System
open Fable.Import.vscode

module ILCodeViewer =

    let exampleCommand () =
        "IL Viewer Command Works!"
        |> window.showInformationMessage
        |> ignore

    let activate (context: ExtensionContext) =
        commands.registerCommand("fsharp.ILViewer", unbox<Func<obj, obj>> exampleCommand)
        |> context.subscriptions.Add