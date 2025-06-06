# Orleans Samples

> [!IMPORTANT]
> 📢 This collection of samples has been moved to the official [`dotnet/samples` repository](https://github.com/dotnet/samples/tree/main/orleans) and is part of the [Samples browser experience](https://learn.microsoft.com/en-us/samples/browse/?expanded=dotnet&products=dotnet-orleans).

- :octocat: [dotnet/samples](https://github.com/dotnet/samples/tree/main/orleans)
- :eyes: [Samples browser](https://learn.microsoft.com/samples/browse/?expanded=dotnet&products=dotnet-orleans)

## [Hello, World!](https://learn.microsoft.com/samples/dotnet/samples/orleans-hello-world-sample-app)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/HelloWorld/code.png" />
</p>

A *Hello, World!* application which demonstrates how to create and use your first grains.

### Demonstrates

- How to get started with Orleans
- How to define and implement grain interface
- How to get a reference to a grain and call a grain

## [Adventure](https://learn.microsoft.com/samples/dotnet/samples/orleans-text-adventure-game)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/Adventure/assets/BoxArt.jpg" />
</p>

Before there were graphical user interfaces, before the era of game consoles and massive-multiplayer games, there were VT100 terminals and there was [Colossal Cave Adventure](https://en.wikipedia.org/wiki/Colossal_Cave_Adventure), [Zork](https://en.wikipedia.org/wiki/Zork), and [Microsoft Adventure](https://en.wikipedia.org/wiki/Microsoft_Adventure).
Possibly lame by today's standards, back then it was a magical world of monsters, chirping birds, and things you could pick up.
It's the inspiration for this sample.

### Demonstrates

- How to structure an application (in this case, a game) using grains
- How to connect an external client to an Orleans cluster (`ClientBuilder`)

## [Chirper](https://learn.microsoft.com/samples/dotnet/samples/orleans-chirper-social-media-sample-app)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/Chirper/screenshot.png" />
</p>

A social network pub/sub system, with short text messages being sent between users.
Publishers send out short *"Chirp"* messages (not to be confused with *"Tweets"*, for a variety of legal reasons) to any other users that are following them.

### Demonstrates

- How to build a simplified social media / social network application using Orleans
- How to store state within a grain using grain persistence (`IPersistentState<T>`)
- Grains which implement multiple grain interfaces
- Reentrant grains, which allow for multiple grain calls to be executed concurrently, in a single-threaded, interleaving fashion
- Using a *grain observer* (`IGrainObserver`) to receive push notifications from grains

## [GPS Tracker](https://learn.microsoft.com/samples/dotnet/samples/orleans-gps-device-tracker-sample)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/GPSTracker/screenshot.jpeg" />
</p>

A service for tracking GPS-equipped [IoT](https://en.wikipedia.org/wiki/Internet_of_Things) devices on a map.
Device locations are updated in near-real-time using SignalR and hence this sample demonstrates one approach to integrating Orleans with SignalR.
The device updates originate from a *device gateway*, which is implemented using a separate process which connects to the main service and simulates a number of devices moving in a pseudorandom fashion around an area of San Francisco.

### Demonstrates

- How to use Orleans to build an [Internet of Things](https://en.wikipedia.org/wiki/Internet_of_Things) application
- How Orleans can be co-hosted and integrated with [ASP.NET Core SignalR](https://docs.microsoft.com/aspnet/core/signalr/introduction)
- How to broadcast real-time updates from a grain to a set of clients using Orleans and SignalR

## [HanBaoBao](https://github.com/ReubenBond/hanbaobao-web)

<p align="center">
    <img src="https://raw.githubusercontent.com/ReubenBond/hanbaobao-web/main/assets/demo-1.png" />
</p>

An English-Mandarin dictionary Web application demonstrating deployment to Kubernetes, fan-out grain calls, and request throttling.

### Demonstrates

- How to build a realistic application using Orleans
- How to deploy an Orleans-based application to Kubernetes
- How to integrate Orleans with ASP.NET Core and a [*Single-page Application*](https://en.wikipedia.org/wiki/Single-page_application) JavaScript framework ([Vue.js](https://vuejs.org/))
- How to implement leaky-bucket request throttling
- How to load and query data from a database
- How to cache results lazily and temporarily
- How to fan-out requests to many grains and collect the results

## [Presence Service](https://learn.microsoft.com/samples/dotnet/samples/orleans-gaming-presence-service-sample)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/Presence/screenshot.png" />
</p>

A gaming presence service, similar to one of the Orleans-based services built for [Halo](https://www.xbox.com/games/halo).
A presence service tracks players and game sessions in near-real-time.

### Demonstrates

- A simplified version of a real-world use of Orleans
- Using a *grain observer* (`IGrainObserver`) to receive push notifications from grains

## [Tic Tac Toe](https://learn.microsoft.com/samples/dotnet/samples/orleans-tictactoe-web-based-game)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/TicTacToe/logo.png"/>
</p>

A Web-based [Tic-tac-toe](https://en.wikipedia.org/wiki/Tic-tac-toe) game using ASP.NET MVC, JavaScript, and Orleans.

### Demonstrates

- How to build an online game using Orleans
- How to build a basic game lobby system
- How to access Orleans grains from an ASP.NET Core MVC application

## [Voting](https://learn.microsoft.com/samples/dotnet/samples/orleans-voting-sample-app-on-kubernetes)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/Voting/screenshot.png"/>
</p>

A Web application for voting on a set of choices. This sample demonstrates deployment to Kubernetes.
The application uses [.NET Generic Host](https://docs.microsoft.com/dotnet/core/extensions/generic-host) to co-host [ASP.NET Core](https://docs.microsoft.com/aspnet/core) and Orleans as well as the [Orleans Dashboard](https://github.com/OrleansContrib/OrleansDashboard) together in the same process.

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/Voting/dashboard.png"/>
</p>

### Demonstrates

- How to deploy an Orleans-based application to Kubernetes
- How to configure the [Orleans Dashboard](https://github.com/OrleansContrib/OrleansDashboard)

## [Chat Room](https://learn.microsoft.com/samples/dotnet/samples/orleans-chat-room-sample)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/ChatRoom/screenshot.png" />
</p>

A terminal-based chat application built using [Orleans Streams](https://docs.microsoft.com/dotnet/orleans/streaming).

### Demonstrates

- How to build a chat application using Orleans
- How to use [Orleans Streams](https://docs.microsoft.com/dotnet/orleans/streaming)

## [Bank Account](https://learn.microsoft.com/samples/dotnet/samples/orleans-bank-account-acid-transactions)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/BankAccount/assets/BankClient.png"/>
</p>

Simulates bank accounts, using ACID transactions to transfer random amounts between a set of accounts.

### Demonstrates

- How to use Orleans Transactions to safely perform operations involving multiple stateful grains with ACID guarantees and serializable isolation.

## [Blazor Server](https://learn.microsoft.com/samples/dotnet/samples/orleans-aspnet-core-blazor-server-sample) and [Blazor WebAssembly](https://learn.microsoft.com/samples/dotnet/samples/orleans-aspnet-core-blazor-wasm-sample)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/Blazor/BlazorServer/screenshot.jpeg"/>
</p>

These two Blazor samples are based on the [Blazor introductory tutorials](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/intro), adapted for use with Orleans.
The [Blazor WebAssembly](./Blazor/BlazorWasm/#readme) sample uses the [Blazor WebAssembly hosting model](https://docs.microsoft.com/aspnet/core/blazor/hosting-models#blazor-webassembly).
The [Blazor Server](./Blazor/BlazorServer/#readme) sample uses the [Blazor Server hosting model](https://docs.microsoft.com/aspnet/core/blazor/hosting-models#blazor-server).
They include an interactive counter, a TODO list, and a Weather service.

### Demonstrates

- How to integrate ASP.NET Core Blazor Server with Orleans
- How to integrate ASP.NET Core Blazor WebAssembly (WASM) with Orleans

## [Stocks](https://learn.microsoft.com/samples/dotnet/samples/orleans-stocks-sample-app)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/Stocks/screenshot.png" />
</p>

A stock price application which fetches prices from a remote service using an HTTP call and caches prices temporarily in a grain.
A [`BackgroundService`](https://docs.microsoft.com/aspnet/core/fundamentals/host/hosted-services#backgroundservice-base-class) periodically polls for updates stock prices from various `StockGrain` grains which correspond to a set of stock symbols.

### Demonstrates

- How to use Orleans from within a [`BackgroundService`](https://docs.microsoft.com/aspnet/core/fundamentals/host/hosted-services#backgroundservice-base-class).
- How to use timers within a grain
- How to make external service calls using .NET's `HttpClient` and cache the results within a grain.

## [Transport Layer Security](https://learn.microsoft.com/samples/dotnet/samples/orleans-transport-layer-security-tls)

<p align="center">
    <img src="https://raw.githubusercontent.com/dotnet/samples/main/orleans/TransportLayerSecurity/screenshot.png" />
</p>

A *Hello, World!* application configured to use mutual [*Transport Layer Security*](https://en.wikipedia.org/wiki/Transport_Layer_Security) to secure network communication between every server.

### Demonstrates

- How to configure mutual-TLS (mTLS) authentication for Orleans

## [General Examples - Road to Orleans](https://github.com/PiotrJustyna/road-to-orleans/)

A compiled list of examples varying in difficulty.

### Demonstrates

- How to develop Orleans-based applications

## [Visual Basic Hello World](https://github.com/dotnet/samples/tree/main/orleans/VBHelloWorld/README.md)

A *Hello, World!* application using Visual Basic.

### Demonstrates

- How to develop Orleans-based applications using Visual Basic

## [F# Hello World](https://github.com/dotnet/samples/tree/main/orleans/FSharpHelloWorld/README.md)

A *Hello, World!* application using F#.

### Demonstrates

- How to develop Orleans-based applications using F#

## [F# Hello World written in F# end to end](https://github.com/PiotrJustyna/road-to-orleans/tree/main/5a#readme)

In-memory clustering example where everything is written in F#:

- Clustered Silos
- Concurrent Clients
- Grains
- Interfaces

### Demonstrates

- How to develop Orleans-based applications using F# end to end

## [F# Reminder](https://github.com/PiotrJustyna/road-to-orleans/tree/main/1b#readme)

- How to use grain reminders in an F# grain

### Demonstrates

- How to develop a reminder grain in F#

## [F# Grain Service](https://github.com/PiotrJustyna/road-to-orleans/tree/main/1c#readme)

- How to use grain service from other grains in F#

### Demonstrates

- How to develop grain service and grain service client in F#

## [Streaming: Pub/Sub Streams over Azure Event Hubs](https://learn.microsoft.com/samples/dotnet/samples/orleans-streaming-pubsub-with-azure-event-hub)

An application using Orleans Streams with [Azure Event Hubs](https://azure.microsoft.com/services/event-hubs/) as the provider and implicit subscribers.

### Demonstrates

- How to use [Orleans Streams](https://docs.microsoft.com/dotnet/orleans/streaming)
- How to use the `[ImplicitStreamSubscription(namespace)]` attribute to implicitly subscribe a grain to the stream with the corresponding id
- How to configure Orleans Streams for use with [Azure Event Hubs](https://azure.microsoft.com/services/event-hubs/)

## [Streaming: Custom Data Adapter](https://learn.microsoft.com/samples/dotnet/samples/orleans-streaming-custom-data-adapter)

An application using Orleans Streams with a non-Orleans publisher pushing to a stream which a grain consumes via a *custom data adapter* which tells Orleans how to interpret stream messages.

### Demonstrates

- How to use [Orleans Streams](https://docs.microsoft.com/dotnet/orleans/streaming)
- How to use the `[ImplicitStreamSubscription(namespace)]` attribute to implicitly subscribe a grain to the stream with the corresponding id
- How to configure Orleans Streams for use with [Azure Event Hubs](https://azure.microsoft.com/services/event-hubs/)
- How to consume stream messages published by non-Orleans publishers by providing a custom `EventHubDataAdapter` implementation (a custom data adapter)
