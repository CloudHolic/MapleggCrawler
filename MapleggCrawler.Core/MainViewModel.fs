module MapleggCrawler.MainViewModel

open Elmish.WPF
open MapleggCrawler.Parser

type Model = {
    Info: UserInfo
    Nick: string
}

type Msg =
    | Search
    | SetNickname of string

let init = { 
    Info = {Nickname = ""; Level = 0; Mulung = 0}
    Nick = ""
}

let update msg m =
    match msg with
    | Search -> { m with Info = getUserInfo m.Nick }
    | SetNickname x -> { m with Nick = x}

let bindings () : Binding<Model, Msg> list = [
    "SearchNick" |> Binding.oneWayToSource SetNickname
    "Search" |> Binding.cmd Search
    "Nickname" |> Binding.oneWay (fun m -> m.Info.Nickname)
    "Level" |> Binding.oneWay (fun m -> m.Info.Level)
    "Mulung" |> Binding.oneWay (fun m -> m.Info.Mulung)
]

let designVm = ViewModel.designInstance init (bindings ())

let main window =
    WpfProgram.mkSimple (fun () -> init) update bindings
    |> WpfProgram.startElmishLoop window