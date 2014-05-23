namespace Quiche.Tests
{
    using System;
    using System.Diagnostics;
    using Samples;
    using Settings;
    using Shouldly;

    public class PerformanceTests
    {
        private static readonly Tuple<string, object>[] Objects =
        {
            new Tuple<string, object>("?itemNames=one&itemNames=two&textMessage=programming+is+fun", new MixedObject { ItemNames = new[] { "one", "two" }, TextMessage = "programming is fun" }),
            new Tuple<string, object>("?child[id]=12&child[name]=Fred+Jr&name=Fred", new ObjectWithChildObject { Child = new FlatObject { Id = 12, Name = "Fred Jr" }, Name = "Fred" }),
            new Tuple<string, object>("?atmosphere[crust][upperMantle][transitionZone][lowerMantle][outerCore][innerCore]=hot", new { Atmosphere = new { Crust = new { UpperMantle = new { TransitionZone = new { LowerMantle = new { OuterCore = new { InnerCore = "hot", } } } } } } }),
            new Tuple<string, object>("?a[b]=1&a[c]=2&d[]=3&d[]=4&d[2][e]=5", new { a = new { b = 1, c = 2 }, d = new object[] { 3, 4, new { e = 5 } } }),
        };

        public void Building_objects_should_be_fast()
        {
            var builder = new Builder(x => { x.FieldCasing = FieldCasing.CamelCase; });

            var buildCount = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < 1000)
            {
                var obj = GetRandomObject();
                var result = builder.ToQueryString(obj.Item2);
                obj.Item1.ShouldBe(result);
                buildCount++;
            }
            stopwatch.Stop();
            Console.WriteLine("Built {0} objects in 1 second.", buildCount);
            buildCount.ShouldBeGreaterThan(50000);
        }

        private static Tuple<string, object> GetRandomObject()
        {
            var index = new Random().Next(0, Objects.Length - 1);
            return Objects[index];
        }
    }
}