
using Config.Core;

using Config.Core.Conversion;

using System.Diagnostics;

namespace Core.Test
{
    [TestClass]
    public sealed class Test1
    {
       
        [TestMethod]
        public void EnumParserTest()
        {
            string enumString = "Beta";

            var enumVal = CEnumParserFast<SampleEnum>.Parse(enumString);

            Assert.AreEqual( SampleEnum.Beta, enumVal );
        }
        [TestMethod]
        public void ConvertExTest()
        {
            string intString = "123";
            int intVal = ConvertEx.From(intString,0);
            Assert.AreEqual( 123, intVal );

            string enumString = "Beta";
            var enumVal = ConvertEx.From(enumString, SampleEnum.None);
            Assert.AreEqual( SampleEnum.Beta, enumVal );

            string floatString = "1.1";
            var floatVal = ConvertEx.From(floatString, 0f);
            Assert.AreEqual( 1.1f, floatVal );
        }
        enum SampleEnum { None = 0, Alpha = 1, Beta = 2, Gamma = 3 }


        [TestMethod]
        public void ConversionBenchmarks()
        {
            Debug.WriteLine( "=== Conversion Demo ===\n" );

            RunSmokeTests();
            Debug.WriteLine("");

            RunBenchmarks( iterations: 5_000_000 );

            Debug.WriteLine( "" );
            Debug.WriteLine( "All Done" );            
                    }
        private static void RunSmokeTests()
        {
            Debug.WriteLine( "-- Smoke Tests" );

            int i = ConverterFast<int, object>.From("42 ");
            long l = ConverterFast<long, object>.From("9001 ");
            double d = ConverterFast<double, object>.From("3.14159");
            float f = ConverterFast<float, object>.From("2.5");
            bool b1 = ConverterFast<bool, object>.From("Yes");
            //bool b2 = "enabled".ToBool();            
            Guid g = ConverterFast<Guid, object>.From("b0c3f4f0-fb0e-4d2e-8c5e-0f3e6d4d9c9a");
            Uri u = ConverterFast<Uri, object>.From("https://example.com");
            SampleEnum eByName = ConverterFast<SampleEnum, object>.From("Beta");
            SampleEnum eByNumber = ConverterFast<SampleEnum, object>.From("3");

            Debug.WriteLine( "CConvertFast" );
            Debug.WriteLine( $"int: {i}" );
            Debug.WriteLine( $"long: {l}" );
            Debug.WriteLine( $"double: {d}" );
            Debug.WriteLine( $"float: {f}" );
            Debug.WriteLine( $"bool (Yes): {b1}" );
            //Console.WriteLine($"bool (enabled ext): {b2}");
            //Console.WriteLine($"bool (empty ext): {b3}");
            Debug.WriteLine( $"Guid: {g}" );
            Debug.WriteLine( $"Uri: {u}" );
            Debug.WriteLine( $"Enum by name: {eByName}" );
            Debug.WriteLine( $"Enum by number: {eByNumber}" );

            Debug.WriteLine( "ConvertEx" );
            Debug.WriteLine( $"int: {ConvertEx.From( " 42", 0 )}" );
            Debug.WriteLine( $"int: error default {ConvertEx.From( " ", 0 )}" );
            Debug.WriteLine( $"bool: {ConvertEx.From( "true", false )}" );
            Debug.WriteLine( $"bool: error default {ConvertEx.From( "", true )}" );
            //Console.WriteLine($"int: {ii}");

        }
        private static void RunBenchmarks(int iterations)
        {
            Debug.WriteLine( "-- Micro-benchmarks (Stopwatch)\n" );

            List<(string name,Stopwatch watch, Action test)> tests = new List<(string,Stopwatch,Action)> ()
            {
                ("int from \"+42\" (fast)",new Stopwatch(), () =>
                {
                    //var _ = ConvertEx.From("+42",0); 
                    var _ = ConverterFast<int, string>.From("+42");
                }),
                ("int from \"+42\" (fast)",new Stopwatch(),  () =>
                {
                    //var _ = ConvertEx.From("+42",0); 
                    var _ = ConverterFast<int, string>.From("+42");
                }),
                ("int from (object) \"+42\" (fast)",new Stopwatch(), () =>
                {
                    var _ = ConverterFast<int, object>.From("+42");
                }),
                ("int from \"+42\" (ChangeType)", new Stopwatch(), () =>
                {
                    var s = "+42".Trim(' ', '+');
                    var _ = (int)Convert.ChangeType(s, typeof(int));
                }),

                ("int from \"+42\" (int.Parse)", new Stopwatch(), () =>
                {
                    var s = "+42".Trim(' ', '+');
                    var _ = (int)int.Parse(s);
                }),

                ("enum by name (fast)", new Stopwatch(), () =>
                {
                    var _ = ConverterFast<SampleEnum, object>.From("Gamma");
                }),

                ("enum by name (Enum.Parse)", new Stopwatch(), () =>
                {
                    var _ = (SampleEnum)Enum.Parse(typeof(SampleEnum), "Gamma");
                }),
            };


            // Eliminate any compile time activites from the benchmark
            //ConverterFast<int, string>.Resolve();
            //ConverterFast<SampleEnum, string>.Resolve();

            // Why: normalize JIT/GC effects across runs.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();


            for (int i = 0; i < iterations; i++)
            {
                foreach (var item in tests)
                {
                    BenchmarkItem( item.name, i, item.test, item.watch );
                }
            }

            foreach (var item in tests)
            {
                Console.WriteLine( $"{item.name,-30} : {item.watch.ElapsedMilliseconds,8} ms for {iterations:n0} ops" );
            }
        }
        private static void BenchmarkItem(string name, int iteration, Action action, Stopwatch sw)
        {
            sw.Start();
            action();
            sw.Stop();
        }

    }
}
