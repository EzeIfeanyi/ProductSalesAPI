﻿# ProductSalesAPI
This is a simple REST API application. Users with accounts can create and retrieve products and sales orders, and also get a simple analysis for products ordered.

### Prerequisite
- .NET SDK 8.0 installed on your machine.
- Postgresql installed and running on your machine.
- Visual Studio or any other preferred C# IDE.
- Docker (Optional)

## Features

- **SalesOrder Endpoints**:
  - Display a list of sales orders.
  - Create a new sales order.
  - Delete an existing sales order.

- **Products Endpoints**:
  - Display a list of products.
  - Create a new product.

- **Dashboard Endpoints**:
  - Display products with the highest quantity sold.
  - Display products with the highest price.

- **Auth Endpoints**:
  - Register a new user.
  - Login and retrieve a JWT token.

- **Authentication**:
  - User registration and login are generated using JWT tokens.

- **Real-Time Updates**:
  - Live sales updates using SignalR. All users get notified of newly added products and sales orders.

- **Docker Integration**:
  - Dockerfile for containerization.
  - Instructions to build and run the application using Docker.

- **Logging**:
  - Captures important events, requests, and errors.

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/EzeIfeanyi/ProductSalesAPI.git
cd ProductSalesAPI
```

### Configuration
Create a Postgres database and update the connection string in the database configuration, and also the JWT settings in the appSettings.json:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "Logs/log-.txt",
          "rollingInterval": "Day",
          "restrictedToMinimumLevel": "Information",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=salesorder;User Id=postgres;Password=root;"
    //"DefaultConnection": "Host=localhost;Port=5433;Database=<Your-db-name>;Username=postgres;Password=<Your-password>;Pooling=false;"
  },
  "JwtSettings": {
    "Secret": "<*****************Your secret***************>",
    "Issuer": "<*****Update this*****>",
    "Audience": "******Update this*****>",
    "ExpiryDays": 1
  }
}
```

### Build and Run
Restore dependencies and build the project:

```bash
dotnet restore
dotnet build
```

Run the application:

```bash
dotnet run
```

### API Endpoints

#### SalesOrder Endpoints
 GET `/api/Orders`
* Display a list of sales orders.

 POST `/api/Orders`
* Create a new sales order.

 DELETE `/api/Orders/{id}`
* Delete an existing sales order.

#### Products Endpoints

 GET `/api/products`
* Display a list of products.

 POST `/api/products`
* Create a new product.

#### Dashboard Endpoints

 GET `/api/Dashboard/highest-quantity-sold`
* Display products with the highest quantity sold.

 GET `/api/Dashboard/highest-total-price-sold`
* Display products with the highest price.

#### Auth Endpoints

 POST `/api/Auth/register`
* Register a new user.

 POST `/api/Auth/login`
* Login and retrieve a JWT token.


### SingalR Hub

Endpoint: `/notificationHub`

## Authentication

The API uses JWT (JSON Web Tokens) for authentication. After registering or logging in, you will receive a token which must be included in the Authorization header of subsequent requests.

### Register a User

Endpoint: POST /api/Auth/register

Request body:

```json
{
  "username": "yourusername", // must be 3 or more character
  "password": "yourpassword"  // must not be less than 8 characters
}
```

### Login

Endpoint: POST `/api/Auth/login`

Request body:

```json
{
  "username": "yourusername",
  "password": "yourpassword"
}
```

Response

```json
{
  "success": true,
  "message": "Login successful.",
  "data": "<JWT token>"
}
```

Include this token in the Authorization header of your requests:

```makefile
Authorization: Bearer token
```

## Docker Integration

### Dockerfile

A Dockerfile is included to build the application into a Docker image.

### Build and Run with Docker

Build the Docker image:

```bash
docker build -t product-sales-api .
```

Run the Docker container:

```bash
docker run -d -p 8000:80 --name product-sales-api product-sales-api
```

The API will be available at http://localhost:8000.

### Using Docker Desktop GUI
1. Open Docker Desktop: Launch Docker Desktop from your system tray or menu bar.

2. Build the Image Using Docker Desktop:

* Click on the + Add button in the Images section.
* Select Build image from Dockerfile.
* Choose the directory where your Dockerfile is located (the root directory of your cloned repository).
* Follow the prompts to build the image. Tag it as product-sales-api.

3.Run the Container Using Docker Desktop:

* Go to the Images tab.
* Find the product-sales-api image you just built.
* Click on the Run button next to the image.
* Configure the container settings:
  Set the port mapping (e.g., map container port 80 to host port 8000).
  Name the container product-sales-api.
* Click Run to start the container.

4. Access the API:
* Open your web browser and navigate to http://localhost:8000. The API should be running.

## Logging
The application uses Serilog for logging. Logs are configured in the appsettings.json file.

## Real-Time Updates with SignalR

SignalR is used for real-time updates. The hub is available at `/notificationHub`. Clients can connect to receive real-time sales updates.

### Example Client Connection

Using Postman:
- Select the websocket option.
- Type in the Url to the hub ("wss://localhost:7212/notificationHub") and click on connect.
- Paste the value with the unicode below in the message tab and click on send to establish a connection.

```json
{"protocol":"json","version":1}
```

Now, The Postman client would listen for any event from the server. Create a new product and confirm the client is receiving the data.
