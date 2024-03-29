﻿using Core;
using System;
using System.Threading.Tasks;

namespace DTGen
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var source = @"
    public class Model
    {
        public List<int> Id { get; set; }
        public Dictionary<int, string> Roles { get; set; }
        public string Name { get; set; }
        public string Field1, field2;
    }

    public class Model1
    {
        public List<int> Id { get; set; }
        public Dictionary<int, string> Roles { get; set; }
        public string Name { get; set; }
        public string Field1, field2;
    }";

                var gen = new Generator(new GenOptions()
                {
                    IsCamelCaseEnabled = true,
                    Language = Language.TypeScript
                });

                Console.WriteLine(await gen.GenerateAsync(source));

            }
            catch (Exception ex)
            {
                throw;
            }
            Console.ReadKey();
        }
    }
}
