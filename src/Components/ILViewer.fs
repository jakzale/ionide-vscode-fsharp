namespace Ionide.VSCode.FSharp

open Ionide.VSCode.Helpers

open System
open System.Text
 
open Fable.Import.vscode

open Fable.Core.JsInterop
open Fable.Import.Node
// open Fable.Import.Node.ChildProcess

module ILCodeViewer =
    let private logger = ConsoleAndOutputChannelLogger(Some "ILViewer", Level.DEBUG, None, Some Level.DEBUG)

    let exampleCommand () =
        // Start by running ILViewer on predefined path
        let monodis = "/Library/Frameworks/Mono.framework/Versions/Current/Commands/monodis"
        let assembly = "/Users/jakub/Developer/FSharp/DisassemblerSamples/src/SampleLibrary/bin/Debug/netstandard2.0/SampleLibrary.dll"
        // let command = sprintf "%s %s" monodis assembly
        let options = createObj []

        let arguments = ResizeArray([ assembly ])

        let sb = ResizeArray()

        ChildProcess.spawn(monodis, arguments, options)
        |> Process.onOutput(fun buffer ->
            sb.Add(buffer.toString()) 
        )
        |> Process.toPromise

        // Process.spawn "/Users/jakub" command ""
        // |> Process.toPromise
        |> Promise.bind (fun _ ->
            let content = String.Join("", sb)
            logger.Debug(content)

            // TODO: THis bit does not work
            (*
                Plan:
                1. Write out the IL to a temporary file
                2. Open the temporary file
            *)
            createObj ["content" ==> content]
            |> workspace.openTextDocument
            |> Promise.bind (fun _ ->
                "Successfully Executed IL Viewer!"
                |> window.showInformationMessage
            )
            // let options = createObj [ "content" ==> content]
        )

    let activate (context: ExtensionContext) =
        commands.registerCommand("fsharp.ILViewer", unbox<Func<obj, obj>> exampleCommand)
        |> context.subscriptions.Add