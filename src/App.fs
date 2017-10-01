module App


open Fable.Core
open Fable.Import
open Elmish
open System

open Fable.Core.JsInterop
open Fable.Helpers.React.Props
open Fable.PowerPack
open Fable.PowerPack.Fetch
open Elmish.React

//module R = Fable.Helpers.React
open Fable.Helpers.React

// MODEL
type Photo = {
    url: string
    }

type ThumbnailSize =
    | Small
    | Medium
    | Large

type Album = {
    photos : Photo list
    selectedUrl : string
    chosenSize : ThumbnailSize
    }

type Model = Album
    
type Msg = | SelectedUrl of string
           | RandomUrl
           | SetSize of ThumbnailSize
           | SelectByIndex of int

let photoAlbum = [
         {url="1.jpeg"}
         {url= "2.jpeg"}
         {url = "3.jpeg"}
         ]

let album = {
    photos = photoAlbum
    selectedUrl = "1.jpeg"
    chosenSize = Medium
    }

let urlPrefix = "http://elm-in-action.com/"
let photoUrl = "http://elm-in-action.com/photos/list"

let rnd = System.Random()
let randomPhotoPicker() =
    let x = photoAlbum.Length
    rnd.Next(x)

let getPhotoUrl (x:int) =
    let photo = List.tryItem x photoAlbum
    match photo with 
    | Some x -> x.url
    | None -> ""

let init() : Model * Cmd<Msg>  = album, []

// FETCH TEST
fetch photoUrl []
|> Promise.bind (fun res -> res.text())
|> Promise.map (fun txt ->
    Browser.console.log txt
) |> ignore

// UPDATE
let update (msg:Msg) (model:Model): (Model * Cmd<Msg>) =
  match msg with 
  | (SelectedUrl x) -> {model with selectedUrl = x}, [] 
  | RandomUrl    -> model, SelectByIndex (randomPhotoPicker()) |> Cmd.ofMsg
  | (SetSize x) -> {model with chosenSize = x}, [] 
  | (SelectByIndex x) -> {model with selectedUrl = getPhotoUrl x }, []
  //| _ -> model

// VIEW (rendered with React)
let view model dispatch =
 
        
  let viewThumbnail selectedUrl thumbnail = 
        
        img [ Src (urlPrefix + thumbnail.url)
              classList  ["selected", selectedUrl = thumbnail.url] 
              OnClick (fun _ -> dispatch (SelectedUrl thumbnail.url))
        ]


  let sizeToString (size: ThumbnailSize) =
      match size with
      | Small -> "small"
      | Medium -> "med"
      | Large -> "large"

  let viewSizeChooser (size : ThumbnailSize) =
      label [OnClick (fun _ -> dispatch (SetSize size))] 
            [ input [Type "radio"; Name "size"] 
              str (sizeToString size)
                  ]

  div [] [
        br [] 
        h1 [ClassName "content"] [  str "Photo Groove"]
        button [ClassName "button"
                Id "button2"
                OnClick (fun _ -> dispatch  RandomUrl )] [str "Surprise Me!"]
        h3 [] [ str "Thumbnail Size:" ]
        div [Id "choose-size"] ([Small; Medium; Large] |> List.map viewSizeChooser) 
        div [Id "thumbnails"; ClassName (sizeToString model.chosenSize) ] (model.photos |> List.map (viewThumbnail model.selectedUrl)) 
        img [ClassName "large"
             Src (urlPrefix + "large/" + model.selectedUrl)]   
        br []
        ]


// App
Program.mkProgram init update view
|> Program.withConsoleTrace
|> Program.withReact "elmish-app"
|> Program.run