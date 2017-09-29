module App

(**
 - title: Counter
 - tagline: The famous Increment/Decrement ported from Elm
*)

open Fable.Core
open Fable.Import
open Elmish
open System

open Fable.Core.JsInterop
open Fable.Helpers.React.Props

open Elmish.React

//module R = Fable.Helpers.React
open Fable.Helpers.React

// MODEL
type Photo = {
    url: string
    }

type Album = {
    photos : Photo list
    selectedUrl : string
    }

type Model = Album
    
type Msg = | SelectedUrl of string

let photoAlbum = [
         {url="1.jpeg"}
         {url= "2.jpeg"}
         {url = "3.jpeg"}
         ]

let album = {
    photos = photoAlbum
    selectedUrl = "1.jpeg"
    }

let urlPrefix = "http://elm-in-action.com/"

let init() : Model = album


// UPDATE
let update (msg:Msg) (model:Model) =
  match msg with 
  | (SelectedUrl x) -> {model with selectedUrl = x}    
  //| _ -> model

// VIEW (rendered with React)
let view model dispatch =
 
  let viewThumbnail selectedUrl thumbnail = 
        
        img [ Src (urlPrefix + thumbnail.url)
              classList  ["selected", selectedUrl = thumbnail.url] 
              OnClick (fun _ -> dispatch (SelectedUrl thumbnail.url))
        ]

  div [] [
        br [] 
        h1 [ClassName "content"] [  str "Photo Groove"]
        br []
        div [Id "thumbnails" ] (model.photos |> List.map (viewThumbnail model.selectedUrl)) 
        img [ClassName "large"
             Src (urlPrefix + "large/" + model.selectedUrl)]   
        br []
        ]


// App
Program.mkSimple init update view
|> Program.withConsoleTrace
|> Program.withReact "elmish-app"
|> Program.run