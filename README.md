# **Super simplied template for Fable Elmish React**

This [FableReactApp repo](https://github.com/s952163/FableReactApp) demonstrates the Elm architecture from 
[Elm in Action](https://www.manning.com/books/elm-in-action) in F# with [Fable](http://fable.io/) [Elmish](https://fable-elmish.github.io/).  
More on the Elm architecture in the [Elm Guide](https://guide.elm-lang.org/architecture/).   

* Prerequisites:  
    * .NET Core SDK 2.0  
    * node.js  
    * npm or yarn    

To clone this repo: 

* ` git clone git@github.com:s952163/FableReactApp.git`
* `cd FableReactApp`
* `yarn install`
* `cd src`
* `dotnet restore`
* `dotnet fable yarn-start`

## To boostrap a Fable Elmish React App from scratch:  

* Install the fable-elmish-reach template for .net core: `dotnet new -i Fable.Template.Elmish.React`  
* Initalize  a new project: `dotnet new fable-elmish-react -n FableElmishReact`
* Install JS dependencies via yarn: `cd FableElmishReact && yarn install`
* Install .NET dependencies via dotnet restore: `dotnet restore && cd src && dotnet restore`
* Run the webpack development server: `dotnet fable yarn-start`  

This will create a sample app on `localhost:8080`.


## How to add react to fable-elmish to a bare-bones fable template

Updates the paket files with react dependencies.


* See below the Fable Simple Template  
* `paket restore`  
* `yarn global add react`  
* `yarn global add react-dom`  
* `yarn install`  
* `dotnet restore`  
* `cd src`  
* `dotnet fable yarn-start`  


The original `index.html` (index1.html) and `App.fs` (App1.fs) files are replaced with a React sample from the ReactSample folder.

Based on the famous (below is the original Readme):

# Fable Simple Template

This template can be used to generate a simple web app with [Fable](http://fable.io/).
You can find more templates by searching `Fable.Template` packages in [Nuget](https://www.nuget.org).

## Requirements

* [dotnet SDK](https://www.microsoft.com/net/download/core) 2.0 or higher
* [node.js](https://nodejs.org) 6.11 or higher
* A JS package manager: [yarn](https://yarnpkg.com) or [npm](http://npmjs.com/)

> npm comes bundled with node.js, but we recommend to use at least npm 5. If you have npm installed, you can upgrade it by running `npm install -g npm`.

Although is not a Fable requirement, on macOS and Linux you'll need [Mono](http://www.mono-project.com/) for other F# tooling like Paket or editor support.

## Editor

The project can be used by editors compatible with the new .fsproj format, like VS Code + [Ionide](http://ionide.io/), Emacs with [fsharp-mode](https://github.com/fsharp/emacs-fsharp-mode) or [Rider](https://www.jetbrains.com/rider/). **Visual Studio for Mac** is also compatible but in the current version the package auto-restore function conflicts with Paket so you need to disable it: `Preferences > Nuget > General`.

## Installing the template

In a terminal, run `dotnet new -i Fable.Template` to install or update the template to the latest version.

## Creating a new project with the template

In a terminal, run `dotnet new fable` to create a project in the current directory. Type `dotnet new fable -n MyApp` instead to create a subfolder named `MyApp` and put the new project there.

> The project will have the name of the directory. You may get some issues if the directory name contains some special characters like hyphens

## Building and running the app

> In the commands below, yarn is the tool of choice. If you want to use npm, just replace `yarn` by `npm` in the commands.

* Install JS dependencies: `yarn install`
* Move to `src` folder: `cd src`
* Install F# dependencies: `dotnet restore`
* Start Fable daemon and [Webpack](https://webpack.js.org/) dev server: `dotnet fable yarn-start`
* In your browser, open: http://localhost:8080/

> `dotnet fable yarn-start` (or `npm-start`) is used to start the Fable daemon and run a script in package.json concurrently. It's a shortcut of `yarn-run [SCRIPT_NAME]`, e.g. `dotnet fable yarn-run start`.

If you are using VS Code + [Ionide](http://ionide.io/), you can also use the key combination: Ctrl+Shift+B (Cmd+Shift+B on macOS) instead of typing the `dotnet fable yarn-start` command. This also has the advantage that Fable-specific errors will be highlighted in the editor along with other F# errors.

Any modification you do to the F# code will be reflected in the web page after saving. When you want to output the JS code to disk, run `dotnet fable yarn-build` and you'll get a minified JS bundle in the `public` folder.

## Project structure

### Paket

[Paket](https://fsprojects.github.io/Paket/) is the package manager used for F# dependencies. It doesn't need a global installation, the binary is included in the `.paket` folder. Other Paket related files are:

- **paket.dependencies**: contains all the dependencies in the repository.
- **paket.references**: there should be one such a file next to each `.fsproj` file.
- **paket.lock**: automatically generated, but should be committed to source control, [see why](https://fsprojects.github.io/Paket/faq.html#Why-should-I-commit-the-lock-file).
- **Nuget.Config**: prevents conflicts with Paket in machines with some Nuget configuration.

> Paket dependencies will be installed in the `packages` directory. See [Paket website](https://fsprojects.github.io/Paket/) for more info.

### yarn/npm

- **package.json**: contains the JS dependencies together with other info, like development scripts.
- **yarn.lock**: is the lock file created by yarn.
- **package-lock.json**: is the lock file understood by npm 5, if you use it instead of yarn.

> JS dependencies will be installed in `node_modules`. See [yarn](https://yarnpkg.com) and/or [npm](http://npmjs.com/) websites for more info.

### Webpack

[Webpack](https://webpack.js.org) is a bundler, which links different JS sources into a single file making deployment much easier. It also offers other features, like a static dev server that can automatically refresh the browser upon changes in your code or a minifier for production release. Fable interacts with Webpack through the `fable-loader`.

- **webpack.config.js**: is the configuration file for Webpack. It allows you to set many things: like the path of the bundle, the port for the development server or [Babel](https://babeljs.io/) options. See [Webpack website](https://webpack.js.org) for more info.

### F# source files

The template only contains two F# source files: the project (.fsproj) and a source file (.fs) in `src` folder.

## Where to go from here

Check more [Fable samples](https://github.com/fable-compiler/samples-browser), use another template like `Fable.Template.Elmish.React` or clone the [fable-suave-scaffold](https://github.com/fable-compiler/fable-suave-scaffold).