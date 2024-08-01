
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Collections.Generic;
using Plang.CSharpRuntime.Values;
using System.Text.Json;
using Plang.CSharpRuntime;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace PImplementation
{
    public sealed class tObserver : IPrtValue
    {
        private static tObserver _instance;
        private readonly ConcurrentDictionary<string, Dictionary<int, PMachine>> _info = new ConcurrentDictionary<string, Dictionary<int, PMachine>>();
        private readonly ConcurrentDictionary<string, int> _counter = new ConcurrentDictionary<string, int>();

        // Singleton pattern
        private tObserver()
        {
        }

        private static tObserver GetInstance()
        {
            if (_instance == null)
            {
                _instance = new tObserver();
            }

            return _instance;
        }

        public static void AddMachine(PrtString name, PMachine machine)
        {
            // Get information from instance
            var info = GetInstance()._info;
            var counter = GetInstance()._counter;
            var identifier = counter.GetValueOrDefault(name, 0);
            Dictionary<int, PMachine> machines;

            // We identify the machine to give it its identifier
            if (info.ContainsKey(name))
            {
                // Already exists machines with that name
                machines = info.GetOrAdd(name, new Dictionary<int, PMachine>());
                // Add machine with that counter
                machines?.Add(identifier, machine);
            }
            else
            {
                // Is the first machine with that name
                machines = new Dictionary<int, PMachine> { { identifier, machine } };
                info.TryAdd(name, machines);
            }
            
            // Update counter
            counter[name] = identifier + 1;
        }

        public static ConcurrentDictionary<string, Dictionary<int, PMachine>> GetInfo()
        {
            return GetInstance()._info;
        }

        public static string Decode(object o)
        {
            // Add all P variables converters
            var options = new JsonSerializerOptions();
            options.Converters.Add(new PrtIntConverter());
            options.Converters.Add(new PrtStringConverter());
            // Serialize object with it type
            var serialize = JsonSerializer.Serialize(o, o.GetType(), options);
            // Remove numeric on variables p transcription
            return Regex.Replace(serialize, @"w*(_\d*)", "");
        }

        public bool Equals(IPrtValue other)
        {
            return other is tObserver;
        }

        public IPrtValue Clone()
        {
            var cloned = new tObserver();
            return cloned;
        }

        public string ToEscapedString()
        {
            return "Observer";
        }
    }
}

