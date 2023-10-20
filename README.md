# SimpleMediaLibrary
[![Build and Push Docker Image](https://github.com/CloudAvionics/SimpleMediaLibrary/actions/workflows/docker-build.yml/badge.svg)](https://github.com/CloudAvionics/SimpleMediaLibrary/actions/workflows/docker-build.yml)

## Table of Contents

- [SimpleMediaLibrary](#simplemedialibrary)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Features](#features)
  - [Technologies Used](#technologies-used)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
  - [Usage](#usage)
  - [Docker Deployment](#docker-deployment)
    - [SSL Certificate Requirement](#ssl-certificate-requirement)
    - [Creating a Self-Signed Certificate](#creating-a-self-signed-certificate)
    - [Docker Run Command](#docker-run-command)
  - [Contributing](#contributing)
  - [License](#license)

## Overview

SimpleMediaLibrary is a small, lightweight .NET Blazor (Server) application designed for the management and playback of audio recordings in MP3 format. Although it is primarily used for ATC recording files written to disk by `rtl_airband`, it is versatile enough to be used for any MP3 formatted files.

## Features

- Automatically adds new MP3 files to the library as they are written to disk
- Enables quick playback of audio recordings directly from the browser
- Allows individual files to be downloaded
- Hosts all files server-side

## Technologies Used

- .NET Blazor (Server)
- MP3 Audio Playback
- File System Monitoring

## Getting Started

### Prerequisites

- .NET SDK
- Docker (Optional for deployment)
- Any modern web browser (e.g., Chrome, Firefox)

### Installation

1. Clone the repository:

```bash
git clone https://github.com/yourusername/SimpleMediaLibrary.git
```

2. Navigate to the project directory:

```bash
cd SimpleMediaLibrary
```

3. Restore NuGet packages:

```bash
dotnet restore
```

4. Build the project:

```bash
dotnet build
```

5. Run the application:

```bash
dotnet run
```

## Usage

- Open your web browser and go to `http://localhost:5000`
- In the appliaion, click the "gear" icon on the app bar and configure the path where the MP3 files are stored. Also change the application name and click "Save"
- Back on the home page, select the date of the recording and click on any file in the table to initiate playback


## Docker Deployment

### SSL Certificate Requirement

When deploying with Docker, an SSL certificate in PFX format is required. The path to this certificate is specified in the Docker run command.

### Creating a Self-Signed Certificate

1. Open a terminal and navigate to your desired directory, e.g., `/home/pi/.simple-media/certs`.

2. Run the following command to generate a self-signed certificate:

    ```bash
    openssl req -x509 -newkey rsa:4096 -keyout key.pem -out cert.pem -days 365
    ```

3. When prompted, enter the passphrase for the private key and answer the certificate questions.

4. Combine the private key and the certificate into a single PFX file:

    ```bash
    openssl pkcs12 -export -out certificate.pfx -inkey key.pem -in cert.pem
    ```

5. You'll be prompted to enter a password for the PFX file. This password will need to be provided in the Docker environment later.

6. Move or ensure the `certificate.pfx` file is in the `/home/pi/.simple-media/certs` directory as specified in the Docker run command.

### Docker Run Command

Run the Docker command to start the container:

```bash
docker run -d --name mediaplayer \
  -v /home/pi/recordings:/app/Data/library \
  -v /home/pi/.simple-media/data:/app/Data \
  -v /home/pi/.simple-media/certs:/https/cert \
  -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert/certificate.pfx \
  -e ASPNETCORE_URLS="http://+:80;https://+:443" \
  -p 5443:443 \
  -p 8080:80 \
  --restart unless-stopped \
  cloudavionics/player:latest-arm64
  ```

## Contributing

We welcome contributions from the community. Please read our contributing guidelines before submitting any changes.

## License

This project is licensed under the MIT License - see the LICENSE.md file for details.
