# API Architecture Diagram

This document provides a hierarchical overview of the API endpoints defined in `Program.cs`, illustrating the routing configuration and the different categories of services.

```mermaid
graph TD
    AppRouting[Application Routing]

    AppRouting -- routes to --> CustomersAPI
    AppRouting -- routes to --> ParkingAPI
    AppRouting -- routes to --> GeneralAPI

    subgraph Customers API
        GetCustomerById("GET /customers/id/{cid}<br/>(GetCustomerById)") --> GetCustomerByIdResult
        AddCustomer("POST /customers/add<br/>(AddCustomer)") --> AddCustomerResult
    end

    subgraph Parking API
        subgraph Enter/Exit Parking
            CreateParkingRecord("POST /api/enter-parking/vehicle<br/>(CreateParkingRecord)") --> CreateParkingRecordResult
            EnterParkingViaQuery("POST /api/enter-parking/car<br/>(EnterParkingViaQuery)") --> EnterParkingViaQueryResult
            ExitParking("DELETE /api/exit-parking/vehicle<br/>(ExitParking)") --> ExitParkingResult
        end
        subgraph Parking Records Management
            UpdateParkingSpot("PUT /api/parking-records/update<br/>(UpdateParkingSpot)") --> UpdateParkingSpotResult
            RequestParkingSpot("POST /api/parking-records/request-parking-spot<br/>(RequestParkingSpot)") --> RequestParkingSpotResult
            UpdateParkingFee("PATCH /api/parking-records/update-fee<br/>(UpdateParkingFee)") --> UpdateParkingFeeResult
            GetParkingData("GET /api/parking-data/vehicle<br/>(GetParkingData)") --> GetParkingDataResult
        end
        subgraph Car Request
            Assigned("POST /api/car/request-parking-spot<br/>(assigned)") --> AssignedResult
        end
    end

    subgraph General API
        HelloWorld["GET /helloworld<br/>(helloworld)"] --> HelloWorldResult
        WeatherForecast["GET /weatherforcast<br/>(WeatherForecast)"] --> WeatherForecastResult
    end

    subgraph Results
        GetCustomerByIdResult["200 OK (Customer) / 404 Not Found (Message)"]
        CreateParkingRecordResult["200 OK (string)"]
        ExitParkingResult["200 OK (string)"]
        HelloWorldResult["200 OK (object {code, message})"]
        AssignedResult["200 OK (string)"]
        UpdateParkingSpotResult["200 OK (string)"]
        RequestParkingSpotResult["200 OK (object {message})"]
        UpdateParkingFeeResult["200 OK (string)"]
        GetParkingDataResult["200 OK (string)"]
        AddCustomerResult["200 OK (Message) / 400 Bad Request (Message)"]
        WeatherForecastResult["200 OK (WeatherForecast[])"]
        EnterParkingViaQueryResult["200 OK (object {message, spotId, entryTime, exitTime, fee})"]
    end
``` 