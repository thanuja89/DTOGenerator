using Core;
using System;
using System.Threading.Tasks;

namespace DTGen
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var source = @"
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }";

            var gen = new Generator(new GenOptions()
            {
                IsCamelCaseEnabled = true,
                Language = Language.TypeScript
            });

            Console.WriteLine(await gen.GenerateAsync(source));
        }
    }
}
