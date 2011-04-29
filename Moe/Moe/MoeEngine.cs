using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SenthilKumarSelvaraj.Moe
{
    public class MoeEngine
    {
        private Test currentlyRunningTest = null;
        private int completedTests = 0;
        public State CurrentState { get; private set; }

        ObservableCollection<Test> tests = new ObservableCollection<Test>();
        public ObservableCollection<Test> Tests
        {
            get { return tests; }
        }

        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;
        public void ProcessLine(string line)
        {
            switch(CurrentState)
            {
                case State.None:
                    if (IsStartOfTestList(line))
                    {
                        tests.Clear();
                        completedTests = 0;
                        CurrentState = State.SettingUp;
                        RaiseProgressChanged(0);
                    }
                    break;
                case State.SettingUp:
                    HandleLineInSettingUpState(line);
                    break;
                case State.Running:
                    HandleLineInRunningState(line);
                    break;
                default:
                    break;

            }
        }

        private void RaiseProgressChanged(int progress)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, new ProgressChangedEventArgs(progress, null));
        }

        private string Unquote(string quoted)
        {
            return quoted.Trim('"');
        }

        private void HandleLineInRunningState(string line)
        {
            if (line.StartsWith(testRunEndPrefix))
            {
                CurrentState = State.None;
                RaiseProgressChanged(100);
            }
            else if (line.StartsWith(testStartPrefix))
            {
                var testName = Unquote(StripPrefixFromLine(line, testStartPrefix));
                currentlyRunningTest = tests.Where(t => t.Name == testName).Single();
            }
            else if (line.StartsWith(testAssertPrefix))
            {
                var testAssertData = StripPrefixFromLine(line, testAssertPrefix);
                ParseTestAssertData(testAssertData);
            }
            else if (line.StartsWith(testEndPrefix))
            {
                currentlyRunningTest = null;
                completedTests++;

                RaiseProgressChanged((int)Math.Round((double)completedTests / tests.Count));
            }
        }

        private void ParseTestAssertData(string testEndData)
        {
            var splitData = testEndData.Split();
            bool testPassed = splitData[0] == "1";
            var expected = splitData[1];
            var actual = splitData[2];

            currentlyRunningTest.Status = testPassed ? Status.Passed : Status.Failed;

            if (!testPassed)
            {
                currentlyRunningTest.ErrorMessage = string.Format("Expected {0}, Actual {1}", expected, actual);
            }
        }

        private void HandleLineInSettingUpState(string line)
        {
            if (IsEndOfTestList(line))
            {
                CurrentState = State.Running;
                MakeAllTestsPending();
            }
            else if (line.StartsWith(testNamePrefix))
            {
                tests.Add(new Test(Unquote(StripPrefixFromLine(line, testNamePrefix))));
            }
        }

        private void MakeAllTestsPending()
        {
            foreach (var test in tests)
            {
                test.Status = Status.Pending;
            }
        }

        private string StripPrefixFromLine(string line, string prefix)
        {
            return line.Substring(prefix.Length);
        }

        private bool IsEndOfTestList(string line)
        {
            return line == endOfTestList;
        }

        private bool IsStartOfTestList(string line)
        {
            return line == startOfTestList;
        }

        private const string startOfTestList = "MOE: [TESTLIST]";
        private const string endOfTestList = "MOE: [/TESTLIST]";
        private const string testNamePrefix = "MOE: TEST-NAME ";
        private const string testStartPrefix = "MOE: TEST-START ";
        private const string testAssertPrefix = "MOE: TEST-ASSERT ";
        private const string testEndPrefix = "MOE: TEST-END ";
        private const string testRunEndPrefix = "MOE: TEST-RUN-END";
    }

    public enum State
    {
        None,
        SettingUp,
        Running,
        Done,
    }
}
