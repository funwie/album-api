
## Album API

Album Enpoints

Get all albums with their photos
https://localhost:44340/api/albums

Get single album with its photos
https://localhost:44340/api/albums/2

Get photos of an album
https://localhost:44340/api/albums/2/photos


User Enpoints

All Users, without albums
https://localhost:44340/api/users

A User with albums
https://localhost:44340/api/users/1


Get Albums for a User
https://localhost:44340/api/users/1/albums

Technologies and Tools
- .Net Core 3.1 and ASP.Net Core Web API
- Polly for Retry and Circuit Breaking https calls
- Swashbuckle.AspNetCore.* for Swagger (OpenApi) spec gen and documentation
- Serilog Log for logging to file
- AutoMapper


