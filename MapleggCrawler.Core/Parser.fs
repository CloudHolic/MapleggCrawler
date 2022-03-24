module MapleggCrawler.Parser

open System.Net.Http
open System.Web

[<StructuredFormatDisplay("{AsString}")>]
type UserInfo = {
    Nickname: string
    Level: int
    Mulung: int } with
    
    override this.ToString() = this.Nickname + " | Lv." + string this.Level + " | " + string this.Mulung + "층"
    member this.AsString = this.ToString()

let getUserInfo (nickname: string) =
    let getAsync (client: HttpClient) (url: string) =
        async {
            let! response = client.GetAsync(url) |> Async.AwaitTask
            response.EnsureSuccessStatusCode () |> ignore
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return content
        }

    let loadHtml nickname = 
        async {
            use httpClient = new HttpClient()
            let! htmlDoc = "https://maple.gg/u/" + nickname |> getAsync httpClient
            return htmlDoc
        }
        |> Async.RunSynchronously

    let parseHtml (htmlDoc: string) =
        let levelIdx = "user-summary-item" |> htmlDoc.IndexOf
        let mulungIdx = "최고무릉:" |> htmlDoc.IndexOf
        let level = htmlDoc[levelIdx + 22 .. levelIdx + 24] |> int
        let mulung = htmlDoc[mulungIdx + 5 .. mulungIdx + 6] |> int
        (level, mulung)

    nickname
    |> HttpUtility.UrlEncode
    |> loadHtml
    |> parseHtml
    |> (fun data -> {Nickname = nickname; Level = fst data; Mulung = snd data})