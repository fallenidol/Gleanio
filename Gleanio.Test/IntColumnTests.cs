﻿namespace Gleanio.Test
{
    using System.Diagnostics;

    using Gleanio.Core.Columns;
    using Gleanio.Core.Extraction;
    using Gleanio.Core.Source;
    using Gleanio.Core.Target;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IntColumnTests
    {
        #region Methods

        [TestMethod]
        public void TestIntColumn()
        {
            var columns = new BaseColumn[] { new IntColumn() };
            var data = new[] { "1", "\t1", "\t1\t", "1-", "-1", "1.0-", "-1.0", " 1- ", " -1 ", "001", "1.0", " 1 ", "1 ", " 1", " 1.000", "1.000 ", int.MaxValue.ToString() };

            var source = new MemorySource("Data", data);
            var extraction = new LineExtraction<TraceOutputTarget>(columns, source, new TraceOutputTarget(), throwParseErrors: true)
            {
                SplitLineFunc = line => line.OriginalLine.Split(',')
            };

            extraction.DataParseError += DataParseError;
            extraction.ExtractToTarget();
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void TestIntColumnDecimal()
        {
            var columns = new BaseColumn[] { new IntColumn() };
            var data = new[] { ("1.1").ToString() };

            var source = new MemorySource("Data", data);
            var extraction = new LineExtraction<TraceOutputTarget>(columns, source, new TraceOutputTarget(), throwParseErrors: true)
            {
                SplitLineFunc = line => line.OriginalLine.Split(',')
            };

            extraction.DataParseError += DataParseError;
            extraction.ExtractToTarget();
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void TestIntColumnSpace()
        {
            var columns = new BaseColumn[] { new IntColumn() };
            var data = new[] { ("1 1").ToString() };

            var source = new MemorySource("Data", data);
            var extraction = new LineExtraction<TraceOutputTarget>(columns, source, new TraceOutputTarget(), throwParseErrors: true)
            {
                SplitLineFunc = line => line.OriginalLine.Split(',')
            };

            extraction.DataParseError += DataParseError;
            extraction.ExtractToTarget();
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void TestIntColumnString()
        {
            var columns = new BaseColumn[] { new IntColumn() };
            var data = new[] { ("One").ToString() };

            var source = new MemorySource("Data", data);
            var extraction = new LineExtraction<TraceOutputTarget>(columns, source, new TraceOutputTarget(), throwParseErrors: true)
            {
                SplitLineFunc = line => line.OriginalLine.Split(',')
            };

            extraction.DataParseError += DataParseError;
            extraction.ExtractToTarget();
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void TestIntColumnSymbol()
        {
            var columns = new BaseColumn[] { new IntColumn() };
            var data = new[] { ("-").ToString() };

            var source = new MemorySource("Data", data);
            var extraction = new LineExtraction<TraceOutputTarget>(columns, source, new TraceOutputTarget(), throwParseErrors: true)
            {
                SplitLineFunc = line => line.OriginalLine.Split(',')
            };

            extraction.DataParseError += DataParseError;
            extraction.ExtractToTarget();
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void TestIntColumnTooBig()
        {
            var columns = new BaseColumn[] { new IntColumn() };
            var data = new[] { ((long)int.MaxValue + 1L).ToString() };

            var source = new MemorySource("Data", data);
            var extraction = new LineExtraction<TraceOutputTarget>(columns, source, new TraceOutputTarget(), throwParseErrors: true)
            {
                SplitLineFunc = line => line.OriginalLine.Split(',')
            };

            extraction.DataParseError += DataParseError;
            extraction.ExtractToTarget();
        }

        private void DataParseError(object sender, Core.EventArgs.ParseErrorEventArgs e)
        {
            Trace.WriteLine(string.Format("PARSE ERROR: {0}, {1}", e.ValueBeingParsed ?? string.Empty, e.Message));
        }

        #endregion Methods
    }
}