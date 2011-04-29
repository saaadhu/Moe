using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SenthilKumarSelvaraj.Moe;

namespace MoeTests
{
    [TestClass]
    public class MoeEngineTests
    {
        private MoeEngine engine;

        [TestMethod]
        public void CurrentState_InitiallyNone()
        {
            Assert.AreEqual(State.None, engine.CurrentState);
        }

        [TestMethod]
        public void CurrentState_StartOfTestList_MovesToSettingup()
        {
            engine.ProcessLine("MOE: [TESTLIST]");
            Assert.AreEqual(State.SettingUp, engine.CurrentState);
        }

        [TestMethod]
        public void CurrentState_AfterProvidingNoTests_MovesToRunning()
        {
            string[] lines = {
                                 "MOE: [TESTLIST]",
                                 "MOE: [/TESTLIST]"
                             };
            ProcessLines(lines);
            Assert.AreEqual(State.Running, engine.CurrentState);
        }

        [TestMethod]
        public void Tests_ProvidedOneTest_ReturnsSingleTest()
        {
            string[] lines = {
                                 "MOE: [TESTLIST]",
                                 "MOE: TEST-NAME \"evaluate_int_literal\"",
                                 "MOE: [/TESTLIST]"
                             };
            ProcessLines(lines);
            CollectionAssert.AreEqual(new[] {"evaluate_int_literal"}, engine.Tests.Select(t => t.Name).ToArray());
        }

        [TestMethod]
        public void Tests_ProvidedOneTestAndTestPasses_TestStatusGetsUpdated()
        {
            string[] lines = {
                                 "MOE: [TESTLIST]",
                                 "MOE: TEST-NAME \"evaluate_int_literal\"",
                                 "MOE: [/TESTLIST]",
                                 "MOE: TEST-START \"evaluate_int_literal\"",
                                 "MOE: TEST-ASSERT 1 123 123",
                                 "MOE: TEST-END"
                             };
            ProcessLines(lines);
            var test = engine.Tests.Where(t => t.Name == "evaluate_int_literal").Single();
            Assert.AreEqual(Status.Passed, test.Status);
        }

        [TestMethod]
        public void Tests_ProvidedOneTestAndTestFails_TestStatusGetsUpdated()
        {
            string[] lines = {
                                 "MOE: [TESTLIST]",
                                 "MOE: TEST-NAME \"evaluate_int_literal\"",
                                 "MOE: [/TESTLIST]",
                                 "MOE: TEST-START \"evaluate_int_literal\"",
                                 "MOE: TEST-ASSERT 0 123 12",
                                 "MOE: TEST-END",
                             };
            ProcessLines(lines);
            var test = engine.Tests.Where(t => t.Name == "evaluate_int_literal").Single();
            Assert.AreEqual(Status.Failed, test.Status);
            Assert.AreEqual("Expected 123, Actual 12", test.ErrorMessage);
        }

        [TestMethod]
        public void CurrentState_TestRunCompletes_StateGoesBackToNone()
        {
            string[] lines = {
                                 "MOE: [TESTLIST]",
                                 "MOE: TEST-NAME \"evaluate_int_literal\"",
                                 "MOE: [/TESTLIST]",
                                 "MOE: TEST-START \"evaluate_int_literal\"",
                                 "MOE: TEST-ASSERT 0 123 12",
                                 "MOE: TEST-END",
                                 "MOE: TEST-RUN-END"
                             };
            ProcessLines(lines);

            Assert.AreEqual(State.None, engine.CurrentState);
        }

        private void  ProcessLines(string[] lines)
        {
            foreach (var line in lines)
            {
                engine.ProcessLine(line);
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            engine = new MoeEngine();
        }

    }
}
