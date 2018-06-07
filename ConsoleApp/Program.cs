using BizApplication.Core.Common.DI;
using System;

namespace SimpleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();
            container.Register<Test, Test>();
            container.Register<InnerTest1, InnerTest1>();
            container.Register<InnerParts1_1, InnerParts1_1>();
            container.Register<InnerParts1_2, InnerParts1_2>();
            container.Register<InnerTest2, InnerTest2>();
            container.Compile();
            var test = container.Resolve<Test>();
        }
    }

    public class Test
    {
        public Test()
        {
        }

        [Inject]
        public InnerTest1 InnerTest1 { get; set; }

        [Inject]
        private InnerTest2 innerTest2;

        public object Method()
        {
            return "test";
        }
    }

    
    public class InnerTest1
    {
        public InnerTest1(InnerParts1_1 parts1_1, InnerParts1_2 parts1_2)
        {
            this.parts1_1 = parts1_1;
            this.parts1_2 = parts1_2;
        }

        private InnerParts1_1 parts1_1;
        private InnerParts1_2 parts1_2;
    }

    public class InnerParts1_1
    {
        public InnerParts1_1()
        {
            value = 10;
        }

        private int value;

        public int Value { get { return value; } }
    }

    public class InnerParts1_2
    {
        public InnerParts1_2()
        {
            value = 10;
        }

        private int value;

        public int Value { get { return value; } }
    }

    public class InnerTest2
    {
        public InnerTest2()
        {
        }

        [Inject]
        public void TestMethod(InnerParts1_1 parts1)
        {
            Console.WriteLine(parts1.ToString());
        }
    }
}
