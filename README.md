
## Album API

#### Technologies and Tools
- .Net Core 3.1 and ASP.Net Core Web API
- Polly for Retry and Circuit Breaking https calls
- Swashbuckle.AspNetCore.* for Swagger (OpenApi) spec gen and documentation
- Serilog Log for logging to file
- AutoMapper

#### Run App
- Clone and open project in your favorite editor
- Confirm that .Net Core 3.1 is installed
- Restore Nuget packages
- Run the App
- Run the tests

#### Swagger {host:port}/swagger
- You can try out the api from the swagger ui or use Postman
- The /Albums may take a bit longer to retrieve all the albums.


#### Album Endpoints

- Get all albums with their photos {host:port}/albums

- Get single album with its photos {host:port}/albums/2

- Get photos of an album {host:port}/albums/2/photos


#### User Endpoints

- Get All Users, without albums {host:port}/users

- Get single User with their albums {host:port}/users/1

- Get Albums for a User {host:port}/users/1/albums



#### Remarks
- Write Integration tests for the Album Service. Time was against me, so i suspended this tests but integration tests and critical for quality api delivery
- Plenty of refactoring to do to clean the code




