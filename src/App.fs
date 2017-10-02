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
    selectedUrl : string option
    chosenSize : ThumbnailSize
    }

type Model = Album
    
type Msg = | SelectedUrl of string
           | RandomUrl
           | SetSize of ThumbnailSize
           | SelectByIndex of int
           | LoadPhotos of Photo List
           | FailureToLoad

let photoAlbum = [
         {url="1.jpeg"}
         {url= "2.jpeg"}
         {url = "3.jpeg"}
         ]

let album = {
    photos = []
    selectedUrl = None
    chosenSize = Medium
    }

let urlPrefix = "http://elm-in-action.com/"
let photoUrl = "http://elm-in-action.com/photos/list"

let rnd = System.Random()
let randomPhotoPicker (model: Album) =
    let x = model.photos.Length
    rnd.Next(x)

let getPhotoUrl model (x:int) =
    let photo = List.tryItem x model.photos
    photo |> Option.map (fun x -> x.url)

// FETCH TEST
let loadPhotos() =
    fetch photoUrl []
    |> Promise.bind (fun res -> res.text())
    |> Promise.map (fun x -> x.Split(','))
    |> (Array.map >> Promise.map) (fun x -> {url = x})
    |> Promise.map List.ofArray
  
let init() : Model * Cmd<Msg>  = 
    let photos = loadPhotos()
    let cmd = Cmd.ofPromise id photos LoadPhotos (fun errorResult -> FailureToLoad)
    album, cmd


// UPDATE
let update (msg:Msg) (model:Model): (Model * Cmd<Msg>) =
  match msg with 
  | (SelectedUrl x) -> {model with selectedUrl = Some x}, [] 
  | RandomUrl    -> model, SelectByIndex (randomPhotoPicker model) |> Cmd.ofMsg
  | (SetSize x) -> {model with chosenSize = x}, [] 
  | (SelectByIndex x) -> {model with selectedUrl = getPhotoUrl model x }, []
  | (LoadPhotos x) -> {model with photos = x}, []
  | FailureToLoad -> failwith "oops, couldn't load photos from url"
  //| _ -> model

// VIEW (rendered with React)
let view model dispatch =
 
  let viewThumbnail (selectedUrl: string option) thumbnail = 
        
        img [ Src (urlPrefix + thumbnail.url)
              classList  ["selected", selectedUrl = Some thumbnail.url] 
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
  let viewLarge (url: string option) =
    match url with 
    | Some url -> img [ClassName "large"
                       Src (urlPrefix + "large/" + url)]
    | None -> str ""

  div [] [
        br [] 
        h1 [ClassName "content"] [  str "Photo Groove"]
        button [ClassName "button"
                Id "button2"
                OnClick (fun _ -> dispatch  RandomUrl )] [str "Surprise Me!"]
        h3 [] [ str "Thumbnail Size:" ]
        div [Id "choose-size"] ([Small; Medium; Large] |> List.map viewSizeChooser) 
        div [Id "thumbnails"; ClassName (sizeToString model.chosenSize) ] (model.photos |> List.map (viewThumbnail model.selectedUrl)) 
        viewLarge model.selectedUrl    
        br []
        ]


// App
Program.mkProgram init update view
|> Program.withConsoleTrace
|> Program.withReact "elmish-app"
|> Program.run