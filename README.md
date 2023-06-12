# Project
Make a reservation is a .NET Core API that allows you to Make a reservation dinner, see the available tables, see old reservations and cancel a reservation.

# Techonologies
.NET Core 6.0, EntityFramework 7.0 with Migrations e C#

# How to Run
- Make sure that you have installed the .NET Core 6.0
- Clone the repo
- Open your terminal
- On the root folder of the repo type `cd src/ReservationRestaurant`
- Clean the solution with `dotnet clean`
- Build the solution with `dotnet build`
- And run with `dotnet run`

For https the API will be listening on: `https://localhost:5001`<br>
For http the API will be listening on: `http://localhost:5000`

# More Information
The API considers that every reservation should have an interval of one hour and that all the available tables is defined by seats, date and time
