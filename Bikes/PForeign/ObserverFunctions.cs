
using Plang.CSharpRuntime;
using Plang.CSharpRuntime.Values;
using System;

namespace PImplementation
{
    public static partial class GlobalFunctions
    {
        public static PrtString Decode(object o, PMachine machine)
        {
            PrtString decode =  tObserver.Decode(o);
            machine.Log(decode);

            return decode;
        }

        public static void AddMachine(PrtString name, PMachine machine)
        {
            tObserver.AddMachine(name, machine);
        }

        public static object GetInfo()
        {
            return tObserver.GetInfo();
        }
    }
}
