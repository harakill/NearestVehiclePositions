
using BenchmarkDotNet.Attributes;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace NearestVehiclePositions
{
    internal class FileReader : IEnumerable<Positions>
    {
        private readonly string _path;
        private readonly List<Positions> _list = new List<Positions>();

        public long ElapsedMilliseconds;

        public FileReader(string path)
        {
            _path = path;
        }

        private string GetPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), _path);
        }

        private byte[] ReadData()
        {
            return File.ReadAllBytes(GetPath());
        }

        private IEnumerator<Positions> ReadEnumerator()
        {
            var stopwatch = Stopwatch.StartNew();
            var data = ReadData();
            stopwatch.Stop();
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Binary data file read execution time : {ElapsedMilliseconds} ms");

            using (FileStream fs = File.OpenRead(GetPath()))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                var index = reader.BaseStream.Position;
                while (index < reader.BaseStream.Length)
                {
                    var position = new Positions();

                    position.PositionId = BitConverter.ToInt32(data, Convert.ToInt32(index));
                    index += 4;

                    var stringBuilder = new StringBuilder();
                    while (data[index] != 0)
                    {
                        stringBuilder.Append((char)data[index]);
                        index++;
                    }

                    position.VehicleRegistration = stringBuilder.ToString();

                    index++;

                    position.Latitude = BitConverter.ToSingle(data, Convert.ToInt32(index));

                    index += 4;

                    position.Longitude = BitConverter.ToSingle(data, Convert.ToInt32(index));

                    index += 4;

                    position.RecordedTimeUTC = BitConverter.ToUInt64(data, Convert.ToInt32(index));

                    index += 8;

                    _list.Add(position);
                }
            }
            return _list.GetEnumerator();
        }

        [Benchmark]
        public IEnumerator<Positions> GetEnumerator()
        {
            return ReadEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
