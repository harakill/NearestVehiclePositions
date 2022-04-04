// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Geolocation;
using NearestVehiclePositions;
using System.Diagnostics;

var fileReader = new FileReader("VehiclePositions.dat");
var readers = fileReader.ToList();

var stopwatch = Stopwatch.StartNew();

foreach (var vehicle in VehiclesList())
{
    double distance;
    var vcl = new Positions();
    Stack<double> stack = new Stack<double>();

    foreach (var reader in readers)
    {
        distance = GeoCalculator.GetDistance(vehicle.Latitude, vehicle.Longitude, reader.Latitude, reader.Longitude, 15, DistanceUnit.Kilometers);
        
        if(stack.Count == 0)
            stack.Push(distance);
        else
        {
            if (stack.Peek() > distance)
            {
                stack.Push(distance); 
                vcl = reader;
            }
        }
    }
    
    Console.WriteLine($"{vehicle.Latitude} : {vehicle.Longitude } distance in KM { stack.Peek() } With {vcl.Latitude} : {vcl.Longitude}, Id: {vcl.PositionId}");
}

stopwatch.Stop();
var elapsedMilliseconds = fileReader.ElapsedMilliseconds;
//Console.WriteLine($"Data file read execution time : {elapsedMilliseconds} ms");
Console.WriteLine($"Closest position calculation execution time : {stopwatch.ElapsedMilliseconds} ms");
Console.WriteLine($"Total execution time : {elapsedMilliseconds + stopwatch.ElapsedMilliseconds} ms");
Console.ReadKey();

//[Benchmark]
List<Coordinate> VehiclesList()
{
    var coordinateList = new List<Coordinate>();

    coordinateList.Add(new Coordinate(34.544909, -102.100843));
    coordinateList.Add(new Coordinate(32.345544, -99.123124));
    coordinateList.Add(new Coordinate(33.234235, -100.214124));
    coordinateList.Add(new Coordinate(35.195739, -95.348899));
    coordinateList.Add(new Coordinate(31.895839, -97.789573));
    coordinateList.Add(new Coordinate(32.895839, -101.789573));
    coordinateList.Add(new Coordinate(34.115839, -100.225732));
    coordinateList.Add(new Coordinate(32.335839, -99.992232));
    coordinateList.Add(new Coordinate(33.535339, -94.792232));
    coordinateList.Add(new Coordinate(32.234235, -100.222222));

    return coordinateList;
}