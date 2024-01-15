# Filio - Simple File Management API with .NET

Filio is an open-source project written in .NET that provides a RESTful API for uploading, retrieving, and deleting files. It integrates with Minio S3 for file storage and uses PostgreSQL for database management.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)

## Features

- Easily manage files through a RESTful API.
- Seamless integration with Minio S3 for scalable file storage.
- Utilizes PostgreSQL for robust database management.
- Secure and efficient file operations.

## Getting Started

Follow these steps to get Filio up and running on your system.

### Prerequisites

Make sure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) (>= 8.0)
- [PostgreSQL](https://www.postgresql.org/) (>=11.0)
- [Minio S3](https://min.io/) (or your preferred S3-compatible storage)

### Installation

1. Clone the Filio repository:

   ```bash
   git clone https://github.com/helius01/filio.git
   cd filio
   dotnet build
   ```
2. In `appsetting.json` put your configuration

3. Initialize the PostgreSQL database:
   ```bash
    dotnet ef database update
   ```
4. Start Filio
   ```bash
   cd Api && dotnet run
   ```

