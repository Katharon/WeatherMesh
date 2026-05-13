# WeatherMesh

**WeatherMesh** is a Blazor and ASP.NET Core based platform for connecting weather stations, collecting sensor data and visualizing distributed measurements.

The project was created as a study project to explore how weather stations can be registered, connected and used as distributed data sources inside a web application.

---

## Overview

WeatherMesh is more than a simple weather forecast application.  
The main idea is to provide a platform where weather stations can be brought online, connected to the application and used to send measurement data.

The project focuses on:

- weather station onboarding
- sensor data transmission
- distributed measurement sources
- web-based visualization
- API-based communication
- geocoding and weather-data integration

---

## Core Idea

A weather station acts as an external data source.  
After setup, it can connect to the WeatherMesh platform and provide measurements such as environmental or location-related weather data.

This makes the project interesting from a distributed-systems perspective, because the application does not only display static weather information. It deals with multiple external stations that can send data into the system.

---

## Features

- Blazor-based web frontend
- ASP.NET Core backend/API
- weather station integration concept
- sensor data handling
- geocoding integration
- weather data integration
- distributed measurement processing
- user-facing web interface

---

## Tech Stack

- **C#**
- **.NET**
- **ASP.NET Core**
- **Blazor**
- **REST APIs**
- **Geocoding**
- **Weather APIs**
- **Distributed Systems Concepts**

---

## Architecture

WeatherMesh is structured around the idea of multiple weather stations communicating with a central web platform.

```text
Weather Station(s)
        │
        │  sensor data / measurements
        ▼
ASP.NET Core Backend / API
        │
        │  processed data
        ▼
Blazor Web Frontend
        │
        │  visualization and interaction
        ▼
User
```

The application separates the user-facing web interface from the backend logic that receives, processes and provides weather-related data.

---

## Project Status

WeatherMesh is currently a prototype / study project.

The goal is to demonstrate the design of a web-based platform for connected weather stations, sensor data processing and distributed measurement workflows.

Some parts may still be experimental and are not production-ready.

---

## Getting Started

Clone the repository:

```bash
git clone https://github.com/Katharon/WeatherMesh.git
cd WeatherMesh
```

Restore dependencies:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build
```

Run the application:

```bash
dotnet run
```

---

## What I Learned

This project helped me deepen my understanding of:

- Blazor web development
- ASP.NET Core APIs
- connecting external data sources to a web platform
- weather and geocoding API integration
- processing distributed sensor data
- designing a platform around multiple external nodes/stations
- structuring a C# web application

---

## Repository Notes

This repository was originally created under the name **Blazor-Wetter-App-WASM**.  
The public project name is now **WeatherMesh**.

---

## License

No license has been specified yet.
