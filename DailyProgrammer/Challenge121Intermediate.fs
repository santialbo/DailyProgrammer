
module DailyProgrammer.Challenge121Intermediate
open System
open System.Collections.Generic
open System.IO
open System.Net
open System.Text.RegularExpressions

// [03/27/13] Challenge #121 [Intermediate] Path to Philosophy
// http://www.reddit.com/r/dailyprogrammer/comments/1b3ka1/032713_challenge_121_intermediate_path_to/

let Download (url: string) =
    async {
        try
            let req = WebRequest.Create(url)
            let! resp = req.AsyncGetResponse()
            let stream = resp.GetResponseStream()
            use reader = new StreamReader(stream)
            return reader.ReadToEnd()
        with ex ->
            printfn "%s" ex.Message
            return ""
    }
let GetWikipediaLinks article =
    let url = sprintf "http://en.wikipedia.org/w/api.php?format=json&action=query&titles=%s&prop=revisions&rvprop=content" article
    Download url
    |> Async.RunSynchronously
    |> (new Regex @"""\*"":""(.*)[^\\]""").Match                     // get content
    |> fun m -> if m.Success then m.Groups.[1].Value else ""
    |> (new Regex @"\[\[([^:\[\]|]+)(?:\|[^:\[\]]+)?\]\]").Matches   // get links
    |> Seq.cast
    |> Seq.map (fun (m:Match) -> m.Groups.[1].Value)

let PathTo origin destination =
    let visited = Dictionary<_, _>()
    let rec aux origin history =
        printfn "Looking at %A" origin;
        if String.Compare(origin, destination, true) = 0 then
            Some(List.rev history)
        else if visited.ContainsKey origin then
            None
        else
            visited.Add(origin, history);
            GetWikipediaLinks origin
            |> Seq.tryPick (fun w -> aux w (w::history))
    aux origin [origin]
