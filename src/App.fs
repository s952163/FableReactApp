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

open Fulma
open Fulma.Elmish
open Fulma.Extensions

//                                         <==MODEL==>
[<CLIMutable>]
type Photo = {
    url: string
    size: int
    title: string
    }

type ThumbnailSize =
    | Small
    | Medium
    | Large

type Album = {
    photos : Photo list
    selectedUrl : string option
    chosenSize : ThumbnailSize
    loadingError: string option
    value: int
    }

type Model = Album
    
type Msg = | SelectedUrl of string
           | RandomUrl
           | SetSize of ThumbnailSize
           | SelectByIndex of int
           | LoadPhotos of Photo List
           | FailureToLoad
           | Change of int



//                                      <==UPDATE (aka STATE)==>
let album = {
    photos = []
    selectedUrl = None
    chosenSize = Medium
    loadingError = None
    value = 50
    }

let urlPrefix = "http://elm-in-action.com/"
let photoUrl = "http://elm-in-action.com/photos/list"
let photoUrlJSON = "http://elm-in-action.com/photos/list.json"

let rnd = System.Random()
let randomPhotoPicker (model: Album) =
    let x = model.photos.Length
    rnd.Next(x)

let getPhotoUrl model (x:int) =
    List.tryItem x model.photos
    |> Option.map (fun x -> x.url)


let loadPhotosJSON() =
    promise {
        let! response = fetchAs<Photo list> @"http://elm-in-action.com/photos/list.json" []
        return response    
    } 


let init() : Model * Cmd<Msg>  = 
    let photos = loadPhotosJSON()
    let cmd = Cmd.ofPromise id photos LoadPhotos (fun errorResult -> FailureToLoad)
    album, cmd

let update (msg:Msg) (model:Model): (Model * Cmd<Msg>) =
  match msg with 
  | (SelectedUrl x) -> {model with selectedUrl = Some x}, [] 
  | RandomUrl    -> model, SelectByIndex (randomPhotoPicker model) |> Cmd.ofMsg
  | (SetSize x) -> {model with chosenSize = x}, [] 
  | (SelectByIndex x) -> {model with selectedUrl = getPhotoUrl model x }, []
  | (LoadPhotos x) -> {model with photos = x; selectedUrl = Some (List.head(x)).url}, []
  | FailureToLoad -> {model with loadingError = Some "Error. Try turning it off and on."}, []
  | Change x -> { model with value = x}, []

//                             <==VIEW (rendered with React)==>
let view model dispatch =
 
  let viewThumbnail (selectedUrl: string option) thumbnail = 
        img [ Src (urlPrefix + thumbnail.url)
              Title (thumbnail.title + " [" + (string thumbnail.size) + " KB]")
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
        div [ ClassName "filters"] [
               Slider.slider [Slider.defaultValue(50.) 
                              Slider.onChange (fun x -> dispatch <| Change !!x.currentTarget?value)  
                              ] [] 
               div [] [str (string model.value)] 
        ]       
        h3 [] [ str "Thumbnail Size:" ]
        div [Id "choose-size"] ([Small; Medium; Large] |> List.map viewSizeChooser) 
        div [Id "thumbnails"; ClassName (sizeToString model.chosenSize) ] (model.photos |> List.map (viewThumbnail model.selectedUrl)) 
        viewLarge model.selectedUrl    
        br []
        ]
  


let viewOrError (model:Model) dispatch =
    match model.loadingError with
    | Some x -> 
        div [ClassName "error-message"]
            [ h1 [] [str "Photo Groove"]
              p [] [str x]           
            ]
    | None ->
        view model dispatch

//                                  <== App ==>
Program.mkProgram init update viewOrError
|> Program.withConsoleTrace
|> Program.withReact "elmish-app"
|> Program.run