/// <summary> 
///     <para>
///         Test Class for Spreadsheet Class
///     </para>
/// </summary>
/// 
/// Name: Harrison Doppelt
/// Date: 09/27/2024

using CS3500.DependencyGraph;
using CS3500.Formula;
using CS3500.Spreadsheet;
using Newtonsoft.Json.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Linq;

namespace SpreadsheetTests
{

    [TestClass]
    public class SpreadsheetTests
    {

        // Personal PS5 Tests

        // --- Tests for GetNamesOfAllNonemptyCells ---

        [TestMethod]
        public void GetNamesOfAllNonemptyCells_NoCells_True()
        {
            Spreadsheet sheet = new Spreadsheet();

            var result = sheet.GetNamesOfAllNonemptyCells();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetNamesOfAllNonemptyCells_SingleCell_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "10");

            var result = sheet.GetNamesOfAllNonemptyCells();

            Assert.IsTrue(result.Contains("A1"));
        }

        [TestMethod]
        public void GetNamesOfAllNonemptyCells_MultipleCells_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "10");
            sheet.SetContentsOfCell("B2", "5");
            sheet.SetContentsOfCell("C3", "1");

            var result = sheet.GetNamesOfAllNonemptyCells();

            Assert.IsTrue(result.Contains("A1"));
            Assert.IsTrue(result.Contains("B2"));
            Assert.IsTrue(result.Contains("C3"));
        }

        // --- Tests for GetCellContents ---

        [TestMethod]
        public void GetCellContents_CellWithDoubleContents_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "10");

            var result = sheet.GetCellContents("A1");

            Assert.AreEqual(10.0, result);
        }

        [TestMethod]
        public void GetCellContents_CellWithStringContents_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "text");

            var result = sheet.GetCellContents("A1");

            Assert.AreEqual("text", result);
        }

        [TestMethod]
        public void GetCellContents_CellWithFormulaContents_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            string formula = "=1+1";
            sheet.SetContentsOfCell("A1", formula);

            var result = sheet.GetCellContents("A1");

            Formula f = new Formula("1+1");

            Assert.AreEqual(f, result);
        }

        [TestMethod]
        public void GetCellContents_CellDoesNotExist_True()
        {
            Spreadsheet sheet = new Spreadsheet();

            var result = sheet.GetCellContents("A1");

            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void GetCellContents_CellIsEmpty_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "");

            var result = sheet.GetCellContents("A1");

            Assert.AreEqual("", result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents_InvalidCellName_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.GetCellContents("1A");
        }

        // --- Tests for SetContentsOfCell Number ---

        [TestMethod]
        public void SetContentsOfCellNumber_AddCell_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");

            Assert.AreEqual(5.0, sheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void SetContentsOfCellNumber_UpdateCellNoDependency_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("A1", "10");

            Assert.AreEqual(10.0, sheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void SetContentsOfCellNumber_UpdateCellHasDependency_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            string formula = "=A1+5";
            sheet.SetContentsOfCell("B1", formula);

            IList<string> cellsToRecalculate = sheet.SetContentsOfCell("B1", "10");

            CollectionAssert.AreEqual(new List<string> { "B1" }, cellsToRecalculate.ToList());
        }

        [TestMethod]
        public void SetContentsOfCellNumber_ReturnCorrectOrder_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            string formulaB1 = "=A1*2";
            sheet.SetContentsOfCell("B1", formulaB1);
            string formulaC1 = "=B1+A1";
            sheet.SetContentsOfCell("C1", formulaC1);

            IList<string> cellsToRecalculate = sheet.SetContentsOfCell("A1", "10");

            CollectionAssert.AreEqual(new List<string> { "A1", "B1", "C1" }, cellsToRecalculate.ToList());
        }

        [TestMethod]
        public void SetContentsOfCellNumber_ReturnSingleCellName_True()
        {
            Spreadsheet sheet = new Spreadsheet();

            IList<string> result = sheet.SetContentsOfCell("A1", "5");

            CollectionAssert.AreEqual(new List<string> { "A1" }, result.ToList());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellNumber_InvalidCellName_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("1A", "42");
        }

        // --- Tests for SetContentsOfCell String ---

        [TestMethod]
        public void SetContentsOfCellText_AddCell_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "text");

            Assert.AreEqual("text", sheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void SetContentsOfCellText_UpdateCellNoDependency_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Old");
            sheet.SetContentsOfCell("A1", "New");

            Assert.AreEqual("New", sheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void SetContentsOfCellText_UpdateCellHasDependency_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            string formula = "=A1+5";
            sheet.SetContentsOfCell("B1", formula);

            IList<string> cellsToRecalculate = sheet.SetContentsOfCell("B1", "text");

            CollectionAssert.AreEqual(new List<string> { "B1" }, cellsToRecalculate.ToList());
        }

        [TestMethod]
        public void SetContentsOfCellText_ReturnCorrectOrder_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "old text");
            string formulaB1 = "=A1*2";
            sheet.SetContentsOfCell("B1", formulaB1);
            string formulaC1 = "=B1+A1";
            sheet.SetContentsOfCell("C1", formulaC1);

            IList<string> cellsToRecalculate = sheet.SetContentsOfCell("A1", "new text");

            CollectionAssert.AreEqual(new List<string> { "A1", "B1", "C1" }, cellsToRecalculate.ToList());
        }

        [TestMethod]
        public void SetContentsOfCellText_ReturnSingleCellName_True()
        {
            Spreadsheet sheet = new Spreadsheet();

            IList<string> result = sheet.SetContentsOfCell("A1", "text");

            CollectionAssert.AreEqual(new List<string> { "A1" }, result.ToList());
        }

        [TestMethod]
        public void SetContentsOfCellText_SettingExistingCellEmptyText_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            sheet.SetContentsOfCell("A1", "");

            var result = sheet.GetNamesOfAllNonemptyCells();

            Assert.IsFalse(result.Contains("A1"));
        }

        [TestMethod]
        public void SetContentsOfCellText_SettingNewCellEmptyText_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "");

            var result = sheet.GetNamesOfAllNonemptyCells();

            Assert.IsFalse(result.Contains("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellText_InvalidCellName_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("1A", "text");
        }

        // --- Tests for SetContentsOfCell Formula ---

        [TestMethod]
        public void SetContentsOfCellFormula_AddCell_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            string formula = "=1+1";
            sheet.SetContentsOfCell("A1", formula);

            Formula f = new Formula("1+1");
            Assert.AreEqual(f, sheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void SetContentsOfCellFormula_UpdateCellNoDependency_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            string formulaOld = "=1+1";
            sheet.SetContentsOfCell("A1", formulaOld);
            string formulaNew = "=2+2";
            sheet.SetContentsOfCell("A1", formulaNew);

            Formula f = new Formula("2+2");

            Assert.AreEqual(f, sheet.GetCellContents("A1"));
        }

        [TestMethod]
        public void SetContentsOfCellFormula_UpdateCellHasDependency_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5");
            string formulaOld = "=A1+5";
            sheet.SetContentsOfCell("B1", formulaOld);

            string formulaNew = "=5+5";
            IList<string> cellsToRecalculate = sheet.SetContentsOfCell("B1", formulaNew);

            CollectionAssert.AreEqual(new List<string> { "B1" }, cellsToRecalculate.ToList());
        }

        [TestMethod]
        public void SetContentsOfCellFormula_ReturnCorrectOrder_True()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "10");
            string formulaB1 = "=A1*2";
            sheet.SetContentsOfCell("B1", formulaB1);
            string formulaC1 = "=B1+A1";
            sheet.SetContentsOfCell("C1", formulaC1);

            string formulaA1 = "=5+5";
            IList<string> cellsToRecalculate = sheet.SetContentsOfCell("A1", formulaA1);

            CollectionAssert.AreEqual(new List<string> { "A1", "B1", "C1" }, cellsToRecalculate.ToList());
        }

        [TestMethod]
        public void SetContentsOfCellFormula_ReturnSingleCellName_True()
        {
            Spreadsheet sheet = new Spreadsheet();

            string formula = "=1+1";
            IList<string> result = sheet.SetContentsOfCell("A1", formula);

            CollectionAssert.AreEqual(new List<string> { "A1" }, result.ToList());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellFormula_InvalidCellName_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();
            string formula = "=1+1";

            sheet.SetContentsOfCell("1A", formula);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCellFormula_NewCellCircularDependencySelf_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();
            string formula = "=A1+1";

            sheet.SetContentsOfCell("A1", formula);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCellFormula_UpdateCellCircularDependencySelf_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "10");

            string formula = "=A1+1";

            sheet.SetContentsOfCell("A1", formula);
        }

        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCellFormula_NewCellsCircularDependencyMultiple_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();

            string formulaA1 = "=B1*2";
            string formulaB1 = "=C1*2";
            string formulaC1 = "=A1*2";

            sheet.SetContentsOfCell("A1", formulaA1);
            sheet.SetContentsOfCell("B1", formulaB1);
            sheet.SetContentsOfCell("C1", formulaC1);
        }





        // Teacher PS5 Tests

        // EMPTY SPREADSHEETS
        [TestMethod(), Timeout(2000)]
        [TestCategory("2")]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents_InvalidName_Throws()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1AA");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("3")]
        public void GetCellContents_EmptyCell_Works()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A2"));
        }

        // SETTING CELL TO A DOUBLE
        [TestMethod(), Timeout(2000)]
        [TestCategory("5")]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCell_InvalidNameDouble_Throws()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1A1A", "1.5");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("6")]
        public void SetAndGet_Double_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "1.5");
            Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
        }

        // SETTING CELL TO A STRING
        [TestMethod(), Timeout(2000)]
        [TestCategory("9")]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCell_InvalidNameString_Throws()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1AZ", "hello");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("10")]
        public void SetAndGet_String_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "hello");
            Assert.AreEqual("hello", s.GetCellContents("Z7"));
        }

        // SETTING CELL TO A FORMULA
        [TestMethod(), Timeout(2000)]
        [TestCategory("13")]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCell_InvalidNameFormula_Throws()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1AZ", "=2");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("14")]
        public void SetAndGet_Formula_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "=3");
            Formula f = (Formula)s.GetCellContents("Z7");
            Assert.AreEqual(new Formula("3"), f);
            Assert.AreNotEqual(new Formula("2"), f);
        }

        // CIRCULAR FORMULA DETECTION
        [TestMethod(), Timeout(2000)]
        [TestCategory("15")]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCell_Circular_Throws()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2");
            s.SetContentsOfCell("A2", "=A1");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("16")]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCell_IndirectCircular_Throws()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A3", "=A4+A5");
            s.SetContentsOfCell("A5", "=A6+A7");
            s.SetContentsOfCell("A7", "=A1+A1");
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("17")]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCell_Circular_UndoesCellChanges()
        {
            Spreadsheet s = new Spreadsheet();
            try
            {
                s.SetContentsOfCell("A1", "=A2+A3");
                s.SetContentsOfCell("A2", "15");
                s.SetContentsOfCell("A3", "30");
                s.SetContentsOfCell("A2", "=A3*A1");
            }
            catch (CircularException)
            {
                Assert.AreEqual(15, (double)s.GetCellContents("A2"), 1e-9);
                throw; // C# shortcut to rethrow the same exception that was caught
            }
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("17b")]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCell_Circular_UndoesGraphChanges()
        {
            Spreadsheet s = new Spreadsheet();
            try
            {
                s.SetContentsOfCell("A1", "=A2");
                s.SetContentsOfCell("A2", "=A1");
            }
            catch (CircularException)
            {
                Assert.AreEqual("", s.GetCellContents("A2"));
                Assert.IsTrue(new HashSet<string> { "A1" }.SetEquals(s.GetNamesOfAllNonemptyCells()));
                throw; // C# shortcut to rethrow the same exception that was caught
            }
        }

        // NONEMPTY CELLS
        [TestMethod(), Timeout(2000)]
        [TestCategory("18")]
        public void GetNames_Empty_Works()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("19")]
        public void GetNames_ExplicitlySetEmpty_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("20")]
        public void GetNames_NonemptyCellString_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("21")]
        public void GetNames_NonemptyCellDouble_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "52.25");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("22")]
        public void GetNames_NonemptyCellFormula_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "=3.5");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("23")]
        public void GetNames_NonemptyCells_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("C1", "hello");
            s.SetContentsOfCell("B1", "=3.5");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B1", "C1" }));
        }

        // RETURN VALUE OF SET CELL CONTENTS
        [TestMethod(), Timeout(2000)]
        [TestCategory("24")]
        public void SetContentsOfCell_Double_NoFalseDependencies()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            s.SetContentsOfCell("C1", "=5");
            Assert.IsTrue(s.SetContentsOfCell("A1", "17.2").SequenceEqual(new List<string>() { "A1" }));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("25")]
        public void SetContentsOfCell_String_NoFalseDependencies()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("C1", "=5");
            Assert.IsTrue(s.SetContentsOfCell("B1", "hello").SequenceEqual(new List<string>() { "B1" }));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("26")]
        public void SetContentsOfCell_Formula_NoFalseDependencies()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("B1", "hello");
            Assert.IsTrue(s.SetContentsOfCell("C1", "=5").SequenceEqual(new List<string>() { "C1" }));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("27")]
        public void SetContentsOfCell_ChainDependencies_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A2", "6");
            s.SetContentsOfCell("A3", "=A2+A4");
            s.SetContentsOfCell("A4", "=A2+A5");
            Assert.IsTrue(s.SetContentsOfCell("A5", "82.5").SequenceEqual(new List<string>() { "A5", "A4", "A3", "A1" }));
        }

        // CHANGING CELLS
        [TestMethod(), Timeout(2000)]
        [TestCategory("28")]
        public void SetContentsOfCell_FormulaToDouble_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A1", "2.5");
            Assert.AreEqual(2.5, (double)s.GetCellContents("A1"), 1e-9);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("29")]
        public void SetContentsOfCell_FormulaToString_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A1", "Hello");
            Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("30")]
        public void SetContentsOfCell_StringToFormula_Works()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Hello");
            s.SetContentsOfCell("A1", "=23");
            Assert.AreEqual(new Formula("23"), (Formula)s.GetCellContents("A1"));
            Assert.AreNotEqual(new Formula("24"), (Formula)s.GetCellContents("A1"));
        }

        // STRESS TESTS
        [TestMethod(), Timeout(2000)]
        [TestCategory("31")]
        public void TestStress1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1+B2");
            s.SetContentsOfCell("B1", "=C1-C2");
            s.SetContentsOfCell("B2", "=C3*C4");
            s.SetContentsOfCell("C1", "=D1*D2");
            s.SetContentsOfCell("C2", "=D3*D4");
            s.SetContentsOfCell("C3", "=D5*D6");
            s.SetContentsOfCell("C4", "=D7*D8");
            s.SetContentsOfCell("D1", "=E1");
            s.SetContentsOfCell("D2", "=E1");
            s.SetContentsOfCell("D3", "=E1");
            s.SetContentsOfCell("D4", "=E1");
            s.SetContentsOfCell("D5", "=E1");
            s.SetContentsOfCell("D6", "=E1");
            s.SetContentsOfCell("D7", "=E1");
            s.SetContentsOfCell("D8", "=E1");
            IList<String> cells = s.SetContentsOfCell("E1", "0");
            Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
        }

        // Repeated for extra weight
        [TestMethod(), Timeout(2000)]
        [TestCategory("32")]
        public void TestStress1a()
        {
            TestStress1();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("33")]
        public void TestStress1b()
        {
            TestStress1();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("34")]
        public void TestStress1c()
        {
            TestStress1();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("35")]
        public void TestStress2()
        {
            Spreadsheet s = new Spreadsheet();
            ISet<String> cells = new HashSet<string>();
            for (int i = 1; i < 200; i++)
            {
                cells.Add("A" + i);
                Assert.IsTrue(cells.SetEquals(s.SetContentsOfCell("A" + i, "=A" + (i + 1))));
            }
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("36")]
        public void TestStress2a()
        {
            TestStress2();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("37")]
        public void TestStress2b()
        {
            TestStress2();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("38")]
        public void TestStress2c()
        {
            TestStress2();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("39")]
        public void TestStress3()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 1; i < 200; i++)
            {
                s.SetContentsOfCell("A" + i, "=A" + (i + 1));
            }
            try
            {
                s.SetContentsOfCell("A150", "=A50");
                Assert.Fail();
            }
            catch (CircularException)
            {
            }
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("40")]
        public void TestStress3a()
        {
            TestStress3();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("41")]
        public void TestStress3b()
        {
            TestStress3();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("42")]
        public void TestStress3c()
        {
            TestStress3();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("43")]
        public void TestStress4()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 0; i < 500; i++)
            {
                s.SetContentsOfCell("A1" + i, "=A1" + (i + 1));
            }
            LinkedList<string> firstCells = new LinkedList<string>();
            LinkedList<string> lastCells = new LinkedList<string>();
            for (int i = 0; i < 250; i++)
            {
                firstCells.AddFirst("A1" + i);
                lastCells.AddFirst("A1" + (i + 250));
            }
            Assert.IsTrue(s.SetContentsOfCell("A1249", "25.0").SequenceEqual(firstCells));
            Assert.IsTrue(s.SetContentsOfCell("A1499", "0").SequenceEqual(lastCells));
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("44")]
        public void TestStress4a()
        {
            TestStress4();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("45")]
        public void TestStress4b()
        {
            TestStress4();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("46")]
        public void TestStress4c()
        {
            TestStress4();
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("47")]
        public void TestStress5()
        {
            RunRandomizedTest(47, 2519);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("48")]
        public void TestStress6()
        {
            RunRandomizedTest(48, 2521);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("49")]
        public void TestStress7()
        {
            RunRandomizedTest(49, 2526);
        }

        [TestMethod(), Timeout(2000)]
        [TestCategory("50")]
        public void TestStress8()
        {
            RunRandomizedTest(50, 2521);
        }

        /// <summary>
        /// Sets random contents for a random cell 10000 times
        /// </summary>
        /// <param name="seed">Random seed</param>
        /// <param name="size">The known resulting spreadsheet size, given the seed</param>
        public void RunRandomizedTest(int seed, int size)
        {
            Spreadsheet s = new Spreadsheet();
            Random rand = new Random(seed);
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    switch (rand.Next(3))
                    {
                        case 0:
                            s.SetContentsOfCell(randomName(rand), "3.14");
                            break;
                        case 1:
                            s.SetContentsOfCell(randomName(rand), "hello");
                            break;
                        case 2:
                            s.SetContentsOfCell(randomName(rand), randomFormula(rand));
                            break;
                    }
                }
                catch (CircularException)
                {
                }
            }
            ISet<string> set = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(size, set.Count);
        }

        /// <summary>
        /// Generates a random cell name with a capital letter and number between 1 - 99
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        private string randomName(Random rand)
        {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Substring(rand.Next(26), 1) + (rand.Next(99) + 1);
        }

        /// <summary>
        /// Generates a random Formula
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        private string randomFormula(Random rand)
        {
            string f = randomName(rand);
            for (int i = 0; i < 10; i++)
            {
                switch (rand.Next(4))
                {
                    case 0:
                        f += "+";
                        break;
                    case 1:
                        f += "-";
                        break;
                    case 2:
                        f += "*";
                        break;
                    case 3:
                        f += "/";
                        break;
                }
                switch (rand.Next(2))
                {
                    case 0:
                        f += 7.2;
                        break;
                    case 1:
                        f += randomName(rand);
                        break;
                }
            }
            return f;
        }










        // Personal PS5 Tests

        // --- Tests for SetContentsOfCell ---

        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SetContentsOfCell_InvalidFormula_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();

            string formula = "=A1A";

            sheet.SetContentsOfCell("A1", formula);
        }

        // --- Tests for Changed ---

        [TestMethod]
        public void Changed_InitialState_False()
        {
            Spreadsheet sheet = new Spreadsheet();

            bool isChanged = sheet.Changed;

            Assert.IsFalse(isChanged);
        }

        [TestMethod]
        public void Changed_SaveSpreadsheet_False()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "5");

            sheet.Save("testSave.json");

            bool isChanged = sheet.Changed;

            Assert.IsFalse(isChanged);
        }

        [TestMethod]
        public void Changed_LoadSpreadsheet_False()
        {
            string validJson = @"
            {
                ""Cells"": {
                    ""A1"": { ""StringForm"": ""Hello"" },
                    ""A2"": { ""StringForm"": ""5.5"" },
                    ""A3"": { ""StringForm"": ""=A2+10"" }
                }
            }";

            string filePath = "validSpreadsheet.json";

            File.WriteAllText(filePath, validJson);

            Spreadsheet sheet = new Spreadsheet(filePath);

            bool isChanged = sheet.Changed;

            Assert.IsFalse(isChanged);
        }

        [TestMethod]
        public void Changed_ChangeSinceCreation_True()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "5");

            bool isChanged = sheet.Changed;

            Assert.IsTrue(isChanged);
        }

        [TestMethod]
        public void Changed_ChangeSinceSave_True()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "5");

            sheet.Save("testSave.json");

            sheet.SetContentsOfCell("B1", "5");

            bool isChanged = sheet.Changed;

            Assert.IsTrue(isChanged);
        }

        // --- Tests for Indexer ---

        [TestMethod]
        public void GetCellValueIndexer_Double_Valid()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "5");

            object value = sheet["A1"];

            Assert.AreEqual(5.0, value);
        }

        [TestMethod]
        public void GetCellValueIndexer_String_Valid()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "hello");

            object value = sheet["A1"];

            Assert.AreEqual("hello", value);
        }

        [TestMethod]
        public void GetCellValueIndexer_Formula_Valid()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "=1+1");

            object value = sheet["A1"];

            Assert.AreEqual(2.0, value);
        }

        // --- Tests for SpreadsheetConstructor ---

        [TestMethod]
        public void SpreadsheetConstructor_ValidJson_Valid()
        {
            string validJson = @"
            {
                ""Cells"": {
                    ""A1"": { ""StringForm"": ""Hello"" },
                    ""A2"": { ""StringForm"": ""5.5"" },
                    ""A3"": { ""StringForm"": ""=A2+10"" }
                }
            }";

            string filePath = "validSpreadsheet.json";
            File.WriteAllText(filePath, validJson);

            Spreadsheet sheet = new Spreadsheet(filePath);

            Assert.AreEqual("Hello", sheet.GetCellContents("A1"));
            Assert.AreEqual(5.5, sheet.GetCellContents("A2"));
            Assert.AreEqual(new Formula("A2+10"), sheet.GetCellContents("A3"));
        }

        [TestMethod]
        public void SpreadsheetConstructor_EmptyJson_Valid()
        {
            string emptyJson = "{ \"Cells\": {} }";
            string filePath = "emptySpreadsheet.json";

            File.WriteAllText(filePath, emptyJson);

            Spreadsheet sheet = new Spreadsheet(filePath);

            Assert.AreEqual(0, sheet.GetNamesOfAllNonemptyCells().Count());
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetConstructor_InvalidCellName_ThrowsException()
        {
            string invalidCellJson = @"
            {
                ""Cells"": {
                    ""1A"": { ""StringForm"": ""5"" }
                }
            }";

            string filePath = "invalidCellName.json";
            File.WriteAllText(filePath, invalidCellJson);

            Spreadsheet sheet = new Spreadsheet(filePath);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetConstructor_InvalidFormula_ThrowsException()
        {
            string invalidFormulaJson = @"
            {
                ""Cells"": {
                    ""A1"": { ""StringForm"": ""=1++1"" }
                }
            }";

            string filePath = "invalidFormula.json";
            File.WriteAllText(filePath, invalidFormulaJson);

            Spreadsheet sheet = new Spreadsheet(filePath);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetConstructor_CircularDependency_ThrowsException()
        {
            string circularDependencyJson = @"
            {
                ""Cells"": {
                    ""A1"": { ""StringForm"": ""=B1"" },
                    ""B1"": { ""StringForm"": ""=A1"" }
                }
            }";

            string filePath = "circularDependency.json";

            File.WriteAllText(filePath, circularDependencyJson);

            Spreadsheet sheet = new Spreadsheet(filePath);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetConstructor_FileNotFound_ThrowsException()
        {
            string filePath = "nonExistentFile.json";

            Spreadsheet sheet = new Spreadsheet(filePath);
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SpreadsheetConstructor_ErrorReadingFile_ThrowsException()
        {
            string filePath = "corruptedFile.json";

            File.WriteAllText(filePath, "INVALID_JSON_DATA");

            Spreadsheet sheet = new Spreadsheet(filePath);
        }

        // --- Tests for Save ---

        [TestMethod]
        public void Save_SpreadsheetWithData_Valid()
        {

            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "Hello");
            sheet.SetContentsOfCell("B1", "5.0");
            sheet.SetContentsOfCell("C1", "=A1+5");

            string filePath = "spreadsheetWithData.json";

            sheet.Save(filePath);

            string savedJson = File.ReadAllText(filePath);

            string expectedJson = @"
            {
                ""Cells"": {
                    ""A1"": {
                        ""StringForm"": ""Hello""
                    },
                    ""B1"": {
                        ""StringForm"": ""5""
                    },
                    ""C1"": {
                        ""StringForm"": ""=A1+5""
                    }
                }
            }".Replace("\r\n", "").Replace(" ", "");

            Assert.AreEqual(expectedJson, savedJson.Replace("\r\n", "").Replace(" ", ""));
        }

        [TestMethod]
        public void Save_SpreadsheetEmpty_Valid()
        {

            Spreadsheet sheet = new Spreadsheet();

            string filePath = "emptySpreadsheet.json";

            sheet.Save(filePath);

            string savedJson = File.ReadAllText(filePath);

            string expectedJson = @"
            {
                ""Cells"": {}
            }".Replace("\r\n", "").Replace(" ", "");

            Assert.AreEqual(expectedJson, savedJson.Replace("\r\n", "").Replace(" ", ""));
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Save_InvalidPath_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "Test");

            string invalidFilePath = "/missing/save.json";

            sheet.Save(invalidFilePath);
        }

        // --- Tests for GetCellValue ---

        [TestMethod]
        public void GetCellValue_CellDNE_Valid()
        {
            Spreadsheet sheet = new Spreadsheet();

            object result = sheet.GetCellValue("A1");

            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void GetCellValue_Double_Valid()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "5");

            object result = sheet.GetCellValue("A1");

            Assert.AreEqual(5.0, result);
        }

        [TestMethod]
        public void GetCellValue_String_Valid()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "hello");

            object result = sheet.GetCellValue("A1");

            Assert.AreEqual("hello", result);
        }

        [TestMethod]
        public void GetCellValue_FormulaNoVariables_Valid()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "=1+1");

            object result = sheet.GetCellValue("A1");

            Assert.AreEqual(2.0, result);
        }

        [TestMethod]
        public void GetCellValue_FormulaWithVariables_Valid()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("B1", "3.0");
            sheet.SetContentsOfCell("C1", "=A1+B1");

            object result = sheet.GetCellValue("C1");

            Assert.AreEqual(8.0, result);
        }

        [TestMethod]
        public void GetCellValue_StringInFormula_ReturnsFormulaError()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("B1", "hello");
            sheet.SetContentsOfCell("C1", "=A1 + B1");

            object result = sheet.GetCellValue("C1");

            Assert.IsInstanceOfType(result, typeof(FormulaError));
        }





        // Teacher PS6 Tests

        /// <summary>
        /// Helper method to verify an arbitrary spreadsheet's values
        /// Cell names and eexpeced values are given in an array in alternating pairs
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="constraints"></param>
        public void VerifyValues(Spreadsheet sheet, params object[] constraints)
        {
            for (int i = 0; i < constraints.Length; i += 2)
            {
                if (constraints[i + 1] is double)
                {
                    Assert.AreEqual((double)constraints[i + 1], (double)sheet.GetCellValue((string)constraints[i]), 1e-9);
                }
                else
                {
                    Assert.AreEqual(constraints[i + 1], sheet.GetCellValue((string)constraints[i]));
                }
            }
        }


        /// <summary>
        /// Helper method to set the contents of a given cell for a given spreadsheet
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public IEnumerable<string> Set(Spreadsheet sheet, string name, string contents)
        {
            List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
            return result;
        }

        // Tests IsValid
        [TestMethod, Timeout(2000)]
        [TestCategory("1")]
        public void SetContentsOfCell_SetString_IsValid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "x");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("2")]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCell_InvalidName_Throws()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("1a", "x");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("3")]
        public void SetContentsOfCell_SetFormula_IsValid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "= A1 + C1");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("4")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SetContentsOfCell_SetInvalidFormula_Throws() // try construct an invalid formula
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("B1", "= A1 + 1C");
        }

        // Tests Normalize
        [TestMethod, Timeout(2000)]
        [TestCategory("5")]
        public void GetCellContents_LowerCaseName_IsValid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("b1"));
        }

        /// <summary>
        /// Increase the weight by repeating the previous test
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("6")]
        public void GetCellContents_LowerCaseName_IsValid2()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("B1", "hello");
            Assert.AreEqual("hello", ss.GetCellContents("b1"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("7")]
        public void GetCellValue_CaseSensitivity_IsValid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("B1", "= A1");
            Assert.AreEqual(5.0, (double)s.GetCellValue("B1"), 1e-9);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("8")]
        public void GetCellValue_CaseSensitivity_IsValid2()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "5");
            ss.SetContentsOfCell("B1", "= a1");
            Assert.AreEqual(5.0, (double)ss.GetCellValue("B1"), 1e-9);
        }

        // Simple tests
        [TestMethod, Timeout(2000)]
        [TestCategory("9")]
        public void Constructor_Empty_CorrectValue()
        {
            Spreadsheet ss = new Spreadsheet();
            VerifyValues(ss, "A1", "");
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("10")]
        public void GetCellValue_GetString_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            OneString(ss);
        }

        /// <summary>
        /// Helper method that sets one string in one cell and verifies the value
        /// </summary>
        /// <param name="ss"></param>
        public void OneString(Spreadsheet ss)
        {
            Set(ss, "B1", "hello");
            VerifyValues(ss, "B1", "hello");
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("11")]
        public void GetCellValue_GetNumber_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            OneNumber(ss);
        }

        /// <summary>
        /// Helper method that sets one number in one cell and verifies the value
        /// </summary>
        /// <param name="ss"></param>
        public void OneNumber(Spreadsheet ss)
        {
            Set(ss, "C1", "17.5");
            VerifyValues(ss, "C1", 17.5);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("12")]
        public void GetCellValue_GetFormula_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            OneFormula(ss);
        }

        /// <summary>
        /// Helper method that sets one formula in one cell and verifies the value
        /// </summary>
        /// <param name="ss"></param>
        public void OneFormula(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "5.2");
            Set(ss, "C1", "= A1+B1");
            VerifyValues(ss, "A1", 4.1, "B1", 5.2, "C1", 9.3);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("13")]
        public void Changed_AfterModify_IsTrue()
        {
            Spreadsheet ss = new Spreadsheet();
            Assert.IsFalse(ss.Changed);
            Set(ss, "C1", "17.5");
            Assert.IsTrue(ss.Changed);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("13b")]
        public void Changed_AfterSave_IsFalse()
        {
            Spreadsheet ss = new Spreadsheet();
            Set(ss, "C1", "17.5");
            ss.Save("changed.txt");
            Assert.IsFalse(ss.Changed);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("14")]
        public void GetCellValue_DivideByZero_ReturnsError()
        {
            Spreadsheet ss = new Spreadsheet();
            DivisionByZero1(ss);
        }

        /// <summary>
        /// Helper method to test a formula that indirectly divides by zero
        /// </summary>
        /// <param name="ss"></param>
        public void DivisionByZero1(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "0.0");
            Set(ss, "C1", "= A1 / B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("15")]
        public void GetCellValue_DivideByZero_ReturnsError2()
        {
            Spreadsheet ss = new Spreadsheet();
            DivisionByZero2(ss);
        }

        /// <summary>
        /// Helper method that directly divides by zero
        /// </summary>
        /// <param name="ss"></param>
        public void DivisionByZero2(Spreadsheet ss)
        {
            Set(ss, "A1", "5.0");
            Set(ss, "A3", "= A1 / 0.0");
            Assert.IsInstanceOfType(ss.GetCellValue("A3"), typeof(FormulaError));
        }



        [TestMethod, Timeout(2000)]
        [TestCategory("16")]
        public void GetCellValue_FormulaBadVariable_ReturnsError()
        {
            Spreadsheet ss = new Spreadsheet();
            EmptyArgument(ss);
        }

        /// <summary>
        /// Helper method that tests a formula that references an empty cell
        /// </summary>
        /// <param name="ss"></param>
        public void EmptyArgument(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "C1", "= A1 + B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("17")]
        public void GetCellValue_FormulaBadVariable_ReturnsError2()
        {
            Spreadsheet ss = new Spreadsheet();
            StringArgument(ss);
        }

        /// <summary>
        /// Helper method that tests a formula that references a non-empty string cell
        /// </summary>
        /// <param name="ss"></param>
        public void StringArgument(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "hello");
            Set(ss, "C1", "= A1 + B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("18")]
        public void GetCellValue_FormulaIndirectBadVariable_ReturnsError()
        {
            Spreadsheet ss = new Spreadsheet();
            ErrorArgument(ss);
        }

        /// <summary>
        /// Helper method that creates a formula that indirectly references an empty cell
        /// </summary>
        /// <param name="ss"></param>
        public void ErrorArgument(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "");
            Set(ss, "C1", "= A1 + B1");
            Set(ss, "D1", "= C1");
            Assert.IsInstanceOfType(ss.GetCellValue("D1"), typeof(FormulaError));
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("19")]
        public void GetCellValue_FormulaWithVariable_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            NumberFormula1(ss);
        }

        /// <summary>
        /// Helper method that creates a simple formula with a variable reference
        /// </summary>
        /// <param name="ss"></param>
        public void NumberFormula1(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "C1", "= A1 + 4.2");
            VerifyValues(ss, "C1", 8.3);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("20")]
        public void GetCellValue_FormulaWithNumber_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            NumberFormula2(ss);
        }

        /// <summary>
        /// Helper method that creates a simple formula that's just a number
        /// </summary>
        /// <param name="ss"></param>
        public void NumberFormula2(Spreadsheet ss)
        {
            Set(ss, "A1", "= 4.6");
            VerifyValues(ss, "A1", 4.6);
        }


        // Repeats the simple tests all together
        [TestMethod, Timeout(2000)]
        [TestCategory("21")]
        public void StressTestVariety()
        {
            Spreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "17.32");
            Set(ss, "B1", "This is a test");
            Set(ss, "C1", "= A1+B1");
            OneString(ss);
            OneNumber(ss);
            OneFormula(ss);
            DivisionByZero1(ss);
            DivisionByZero2(ss);
            StringArgument(ss);
            ErrorArgument(ss);
            NumberFormula1(ss);
            NumberFormula2(ss);
        }

        // Four kinds of formulas
        [TestMethod, Timeout(2000)]
        [TestCategory("22")]
        public void StressTestFormulas()
        {
            Spreadsheet ss = new Spreadsheet();
            Formulas(ss);
        }

        public void Formulas(Spreadsheet ss)
        {
            Set(ss, "A1", "4.4");
            Set(ss, "B1", "2.2");
            Set(ss, "C1", "= A1 + B1");
            Set(ss, "D1", "= A1 - B1");
            Set(ss, "E1", "= A1 * B1");
            Set(ss, "F1", "= A1 / B1");
            VerifyValues(ss, "C1", 6.6, "D1", 2.2, "E1", 4.4 * 2.2, "F1", 2.0);
        }

        /// <summary>
        /// Repeated for increased weight
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("23")]
        public void StressTestFormulas2()
        {
            StressTestFormulas();
        }

        /// <summary>
        /// Repeated for increased weight
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("24")]
        public void StressTestFormulas3()
        {
            StressTestFormulas();
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("25")]
        public void Constructor_MultipleSpreadsheets_DontIntefere()
        {
            Spreadsheet s1 = new Spreadsheet();
            Spreadsheet s2 = new Spreadsheet();
            Set(s1, "X1", "hello");
            Set(s2, "X1", "goodbye");
            VerifyValues(s1, "X1", "hello");
            VerifyValues(s2, "X1", "goodbye");
        }

        /// <summary>
        /// Repeated for increased weight
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("26")]
        public void Constructor_MultipleSpreadsheets_DontIntefere2()
        {
            Constructor_MultipleSpreadsheets_DontIntefere();
        }

        /// <summary>
        /// Repeated for increased weight
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("27")]
        public void Constructor_MultipleSpreadsheets_DontIntefere3()
        {
            Constructor_MultipleSpreadsheets_DontIntefere();
        }

        /// <summary>
        /// Repeated for increased weight
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("28")]
        public void Constructor_MultipleSpreadsheets_DontIntefere4()
        {
            Constructor_MultipleSpreadsheets_DontIntefere();
        }

        // Reading/writing spreadsheets
        [TestMethod, Timeout(2000)]
        [TestCategory("29")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Save_InvalidPath_Throws()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.Save(Path.GetFullPath("/missing/save.txt"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("30")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Load_InvalidPath_Throws()
        {
            Spreadsheet ss = new Spreadsheet(Path.GetFullPath("/missing/save.txt"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("31")]
        public void SaveLoad_SimpleSheet_IsValid()
        {
            Spreadsheet s1 = new Spreadsheet();
            Set(s1, "A1", "hello");
            s1.Save("save1.txt");
            s1 = new Spreadsheet("save1.txt");
            Assert.AreEqual("hello", s1.GetCellContents("A1"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("32")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Load_InvalidJson_Throws()
        {
            using (StreamWriter writer = new StreamWriter("save2.txt"))
            {
                writer.WriteLine("This");
                writer.WriteLine("is");
                writer.WriteLine("a");
                writer.WriteLine("test!");
            }
            Spreadsheet ss = new Spreadsheet("save2.txt");
        }



        [TestMethod, Timeout(2000)]
        [TestCategory("35")]
        public void Load_FromManualJson_IsValid()
        {
            var sheet = new
            {
                Cells = new
                {
                    A1 = new { StringForm = "hello" },
                    A2 = new { StringForm = "5.0" },
                    A3 = new { StringForm = "4.0" },
                    A4 = new { StringForm = "= A2 + A3" }
                },
            };

            File.WriteAllText("save5.txt", JsonSerializer.Serialize(sheet));


            Spreadsheet ss = new Spreadsheet("save5.txt");
            VerifyValues(ss, "A1", "hello", "A2", 5.0, "A3", 4.0, "A4", 9.0);
        }

        /// <summary>
        /// This test saves your spreadsheet and then loads it into 
        /// a general (dynamic) object, not using your spreadsheet's load constructor
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("36")]
        public void Save_ToGeneralObject_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "hello");
            Set(ss, "A2", "5.0");
            Set(ss, "A3", "4.0");
            Set(ss, "A4", "= A2 + A3");
            ss.Save("save6.txt");

            string fileContents = File.ReadAllText("save6.txt");

            dynamic? o = JObject.Parse(fileContents);

            Assert.IsNotNull(o);

            Assert.AreEqual("hello", o?.Cells.A1.StringForm.ToString());
            Assert.AreEqual(5.0, double.Parse(o?.Cells.A2.StringForm.ToString()), 1e-9);
            Assert.AreEqual(4.0, double.Parse(o?.Cells.A3.StringForm.ToString()), 1e-9);
            Assert.AreEqual("=A2+A3", o?.Cells.A4.StringForm.ToString().Replace(" ", ""));
        }


        // Fun with formulas
        [TestMethod, Timeout(2000)]
        [TestCategory("37")]
        public void FormulaStress1()
        {
            Formula1(new Spreadsheet());
        }

        /// <summary>
        /// Helper method for formula stress tests
        /// </summary>
        /// <param name="ss"></param>
        public void Formula1(Spreadsheet ss)
        {
            Set(ss, "a1", "= a2 + a3");
            Set(ss, "a2", "= b1 + b2");
            Assert.IsInstanceOfType(ss.GetCellValue("a1"), typeof(FormulaError));
            Assert.IsInstanceOfType(ss.GetCellValue("a2"), typeof(FormulaError));
            Set(ss, "a3", "5.0");
            Set(ss, "b1", "2.0");
            Set(ss, "b2", "3.0");
            VerifyValues(ss, "a1", 10.0, "a2", 5.0);
            Set(ss, "b2", "4.0");
            VerifyValues(ss, "a1", 11.0, "a2", 6.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("38")]
        public void FormulaStress2()
        {
            Formula2(new Spreadsheet());
        }

        /// <summary>
        /// Helper method for formula stress tests
        /// </summary>
        /// <param name="ss"></param>
        public void Formula2(Spreadsheet ss)
        {
            Set(ss, "a1", "= a2 + a3");
            Set(ss, "a2", "= a3");
            Set(ss, "a3", "6.0");
            VerifyValues(ss, "a1", 12.0, "a2", 6.0, "a3", 6.0);
            Set(ss, "a3", "5.0");
            VerifyValues(ss, "a1", 10.0, "a2", 5.0, "a3", 5.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("39")]
        public void FormulaStress3()
        {
            Formula3(new Spreadsheet());
        }

        /// <summary>
        /// Helper method for formula stress tests
        /// </summary>
        /// <param name="ss"></param>
        public void Formula3(Spreadsheet ss)
        {
            Set(ss, "a1", "= a3 + a5");
            Set(ss, "a2", "= a5 + a4");
            Set(ss, "a3", "= a5");
            Set(ss, "a4", "= a5");
            Set(ss, "a5", "9.0");
            VerifyValues(ss, "a1", 18.0);
            VerifyValues(ss, "a2", 18.0);
            Set(ss, "a5", "8.0");
            VerifyValues(ss, "a1", 16.0);
            VerifyValues(ss, "a2", 16.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("40")]
        public void FormulaStress4()
        {
            Spreadsheet ss = new Spreadsheet();
            Formula1(ss);
            Formula2(ss);
            Formula3(ss);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("41")]
        public void FormulaStress5()
        {
            FormulaStress4();
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("42")]
        public void MediumStress()
        {
            Spreadsheet ss = new Spreadsheet();
            MediumSheet(ss);
        }

        /// <summary>
        /// Helper method for formula stress tests
        /// </summary>
        /// <param name="ss"></param>
        public void MediumSheet(Spreadsheet ss)
        {
            Set(ss, "A1", "1.0");
            Set(ss, "A2", "2.0");
            Set(ss, "A3", "3.0");
            Set(ss, "A4", "4.0");
            Set(ss, "B1", "= A1 + A2");
            Set(ss, "B2", "= A3 * A4");
            Set(ss, "C1", "= B1 + B2");
            VerifyValues(ss, "A1", 1.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 3.0, "B2", 12.0, "C1", 15.0);
            Set(ss, "A1", "2.0");
            VerifyValues(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 4.0, "B2", 12.0, "C1", 16.0);
            Set(ss, "B1", "= A1 / A2");
            VerifyValues(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("43")]
        public void MediumStress2()
        {
            MediumStress();
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("44")]
        public void MediumStressSave()
        {
            Spreadsheet ss = new Spreadsheet();
            MediumSheet(ss);
            ss.Save("save7.txt");
            ss = new Spreadsheet("save7.txt");
            VerifyValues(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("45")]
        public void MediumStressSave2()
        {
            MediumStressSave();
        }


        // A long chained formula. Solutions that re-evaluate 
        // cells on every request, rather than after a cell changes,
        // will timeout on this test.
        // This test is repeated to increase its scoring weight
        [TestMethod, Timeout(6000)]
        [TestCategory("46")]
        public void StressLongFormulaChain()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("47")]
        public void StressLongFormulaChain2()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("48")]
        public void StressLongFormulaChain3()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("49")]
        public void StressLongFormulaChain4()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("50")]
        public void StressLongFormulaChain5()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        /// <summary>
        /// Helper method for long formula stress tests
        /// </summary>
        /// <param name="result"></param>
        public void LongFormulaHelper(out object result)
        {
            try
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("sum1", "= a1 + a2");
                int i;
                int depth = 100;
                for (i = 1; i <= depth * 2; i += 2)
                {
                    s.SetContentsOfCell("a" + i, "= a" + (i + 2) + " + a" + (i + 3));
                    s.SetContentsOfCell("a" + (i + 1), "= a" + (i + 2) + "+ a" + (i + 3));
                }
                s.SetContentsOfCell("a" + i, "1");
                s.SetContentsOfCell("a" + (i + 1), "1");
                Assert.AreEqual(Math.Pow(2, depth + 1), (double)s.GetCellValue("sum1"), 1.0);
                s.SetContentsOfCell("a" + i, "0");
                Assert.AreEqual(Math.Pow(2, depth), (double)s.GetCellValue("sum1"), 1.0);
                s.SetContentsOfCell("a" + (i + 1), "0");
                Assert.AreEqual(0.0, (double)s.GetCellValue("sum1"), 0.1);
                result = "ok";
            }
            catch (Exception e)
            {
                result = e;
            }
        }

    }



}