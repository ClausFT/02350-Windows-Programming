using System;
using System.Windows;
using Windows_Programming.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class RelationTest
    {
        static Shape fromShape = new Shape()
        {
            X = 131,
            Y = 257,
            Height = 124,
            Width = 145
        };

        static Shape toShape = new Shape()
        {
            X = 602,
            Y = 24,
            Height = 124,
            Width = 145
        };


        private Line line = new Line()
        {
            From = fromShape,
            To = toShape
        };

        [TestMethod]
        public void TestLineFromX()
        {
            line.SetShortestLine();
            double expected = 276;
            double actual = line.FromX;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestLineFromY()
        {
            line.SetShortestLine();
            double expected = 262;
            double actual = line.FromY;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestLineToX()
        {
            line.SetShortestLine();
            double expected = 602;
            double actual = line.ToX;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestLineToY()
        {
            line.SetShortestLine();
            double expected = 145;
            double actual = line.ToY;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestLineMitPointX()
        {
            line.SetShortestLine();
            double expected = 441;
            double actual = line.TextMargin.Left;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestLineMitPointY()
        {
            line.SetShortestLine();
            double expected = 205.5;
            double actual = line.TextMargin.Top;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestLineTextInitialValue()
        {
            string expected = null;
            string actual = line.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestLineTextSet()
        {
            string expected = "Inheritance";
            line.Text = expected;
            string actual = line.Text;

            Assert.AreEqual(expected, actual);
        }
    }
}
