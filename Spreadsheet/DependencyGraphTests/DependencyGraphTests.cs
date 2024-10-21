/// <summary> 
///     <para>
///         Test Class for DependencyGraph Class
///     </para>
/// </summary>
/// 
/// Name: Harrison Doppelt
/// Date: 09/27/2024

namespace CS3500.DevelopmentTests;

using CS3500.DependencyGraph;

/// <summary>
///   This is a test class for DependencyGraphTest and is intended
///   to contain all DependencyGraphTest Unit Tests
/// </summary>
[TestClass]
public class DependencyGraphTests
{

    // --- Provided Test Method ---

    /// <summary>
    ///     Stress test for the DependencyGraph class.
    /// 
    ///     This test adds, removes, and re-adds dependencies between a large number of nodes (strings),
    ///     and verifies that the DependencyGraph class correctly maintains the relationships between dependents and dependees
    ///     throughout multiple complex operations.
    /// 
    ///     Specifically, it performs the following steps:
    ///     - Adds a series of dependencies between nodes.
    ///     - Removes certain dependencies.
    ///     - Re-adds some dependencies in a controlled manner.
    ///     - Removes additional dependencies.
    ///     - Verifies that the DependencyGraph's internal state matches the expected set of dependents and dependees.
    /// </summary>
    /// <remarks>
    ///     This test is designed to push the DependencyGraph class to handle a large number of operations
    ///     (adding and removing dependencies) in a relatively short time. The test verifies that the graph 
    ///     maintains consistent relationships after complex modifications.
    /// 
    ///     The test uses a constant SIZE (200) to generate 200 unique strings (nodes) and performs operations 
    ///     involving dependencies between these nodes in a variety of patterns to stress the internal data structures
    ///     of the DependencyGraph.
    /// 
    ///     The timeout of 2000 milliseconds ensures that the test runs within a 2-second time limit, further testing
    ///     the efficiency of the DependencyGraph class.
    /// </remarks>
    /// <param name="SIZE">
    ///     The constant SIZE defines the number of nodes (strings) being used in the test. This value is set to 200.
    /// </param>
    /// <param name="letters">
    ///     This array holds the string representations of each node in the graph, generated based on their index in the alphabet.
    ///     For example, "a", "b", "c", etc., are generated dynamically as the node names.
    /// </param>
    /// <param name="dependents">
    ///     This array stores the expected dependents for each node after operations are performed.
    /// </param>
    /// <param name="dependees">
    ///     This array stores the expected dependees for each node after operations are performed.
    /// </param>
    /// <param name="dg">
    ///     The DependencyGraph object being tested. Operations such as AddDependency and RemoveDependency
    ///     are performed on this object.
    /// </param>
    /// <param name="Assert">
    ///     The Assert statements at the end of the test verify that the actual dependents and dependees in the graph
    ///     match the expected ones, using the SetEquals method for comparison.
    /// </param>
    [TestMethod]
    [Timeout(2000)]  // 2 second run time limit
    public void StressTest()
    {
        DependencyGraph dg = new();

        // A bunch of strings to use
        const int SIZE = 200;
        string[] letters = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            letters[i] = string.Empty + ((char)('a' + i));
        }

        // The correct answers
        HashSet<string>[] dependents = new HashSet<string>[SIZE];
        HashSet<string>[] dependees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dependents[i] = [];
            dependees[i] = [];
        }

        // Add a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // Remove a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 4; j < SIZE; j += 4)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Add some back
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j += 2)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // Remove some more
        for (int i = 0; i < SIZE; i += 2)
        {
            for (int j = i + 3; j < SIZE; j += 3)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Make sure everything is right
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dependents[i].SetEquals(new HashSet<string>(dg.GetDependents(letters[i]))));
            Assert.IsTrue(dependees[i].SetEquals(new HashSet<string>(dg.GetDependees(letters[i]))));
        }

    }










    // --- Personal PS3 Tests ---

    // --- Tests for AddDependency Method ---

    [TestMethod]
    public void AddDependency_TestDependentAddedA_ReturnsTrue()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");

        Assert.IsTrue(graph.HasDependents("A"));
    }

    [TestMethod]
    public void AddDependency_TestDependeeAddedB_ReturnsTrue()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");

        Assert.IsTrue(graph.HasDependees("B"));
    }

    // --- Tests for HasDependents Method ---

    [TestMethod]
    public void HasDependents_TestBDependents_ReturnsFalse()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");

        Assert.IsFalse(graph.HasDependents("B"));
    }

    [TestMethod]
    public void HasDependents_TestEmptyGraph_ReturnsFalse()
    {
        var graph = new DependencyGraph();

        Assert.IsFalse(graph.HasDependents("A"));
    }

    // --- Tests for HasDependees Method ---

    [TestMethod]
    public void HasDependees_TestADependees_ReturnsFalse()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");

        Assert.IsFalse(graph.HasDependees("A"));
    }

    [TestMethod]
    public void HasDependees_TestEmptyGraph_ReturnsFalse()
    {
        var graph = new DependencyGraph();

        Assert.IsFalse(graph.HasDependees("A"));
    }

    // --- Tests for Size Method ---

    [TestMethod]
    public void Size_TestEmptyGraph_ReturnsEqual()
    {
        var graph = new DependencyGraph();
        int size = graph.Size;

        Assert.AreEqual(0, size);
    }

    [TestMethod]
    public void Size_TestSingleDependency_ReturnsEqual()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");
        int size = graph.Size;

        Assert.AreEqual(1, size);
    }

    [TestMethod]
    public void Size_TestMultipleDependencies_ReturnsEqual()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");
        graph.AddDependency("A", "C");
        graph.AddDependency("A", "D");
        int size = graph.Size;

        Assert.AreEqual(3, size);
    }

    // --- Tests for RemoveDependency Method ---

    [TestMethod]
    public void RemoveDependency_TestRemoveSingleDependency_ReturnsEqual()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");
        graph.RemoveDependency("A", "B");

        Assert.AreEqual(0, graph.Size);
    }

    // --- Tests for GetDependents Method ---

    [TestMethod]
    public void GetDependents_NodeAHasDependentB_ReturnsEqual()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");
        var dependents = graph.GetDependents("A");

        CollectionAssert.AreEquivalent(new List<string> {"B"}, dependents.ToList());
    }

    [TestMethod]
    public void GetDependents_NodeBHasNoDependents_ReturnsEqual()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");
        var dependents = graph.GetDependents("B");

        CollectionAssert.AreEqual(new List<string>(), dependents.ToList());
    }

    // --- Tests for GetDependees Method ---

    [TestMethod]
    public void GetDependees_NodeBHasDependeesA_ReturnsEquivalent()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");
        var dependees = graph.GetDependees("B");

        CollectionAssert.AreEquivalent(new List<string> {"A"}, dependees.ToList());
    }

    [TestMethod]
    public void GetDependees_NodeAHasNoDependees_ReturnsEqual()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");
        var dependees = graph.GetDependees("A");

        CollectionAssert.AreEqual(new List<string>(), dependees.ToList());
    }

    // --- Tests for ReplaceDependents Method ---

    [TestMethod]
    public void ReplaceDependents_ReplacesDependentsOfNodeAWithNodeC_ReturnsEquivalent()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");
        graph.ReplaceDependents("A", new List<string> {"C"});
        var dependents = graph.GetDependents("A").ToList();

        CollectionAssert.AreEquivalent(new List<string> {"C"}, dependents);
    }

    // --- Tests for ReplaceDependees Method ---

    [TestMethod]
    public void ReplaceDependees_ReplacesDependeesOfNodeBWithNodeC_ReturnsEquivalent()
    {
        var graph = new DependencyGraph();
        graph.AddDependency("A", "B");
        graph.ReplaceDependees("B", new List<string> {"C"});
        var dependees = graph.GetDependees("B").ToList();

        CollectionAssert.AreEquivalent(new List<string> {"C"}, dependees);
    }








    

    // -- Teacher PS3 Tests ---

    // ************************** TESTS ON EMPTY DGs ************************* //

    /// <summary>
    ///   Empty graph should contain nothing.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("1")]
    public void Size_EmptyGraph_ReturnsZero()
    {
        DependencyGraph dg = new();
        Assert.AreEqual(0, dg.Size);
    }

    /// <summary>
    ///   Empty graph should not have a node with dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("2")]
    public void HasDependees_EmptyGraph_NoDependees()
    {
        DependencyGraph dg = new();
        Assert.IsFalse(dg.HasDependees("x"));
    }

    /// <summary>
    ///   Empty graph should not have a node with dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("3")]
    public void HasDependents_EmptyGraph_NoDependents()
    {
        DependencyGraph dg = new();
        Assert.IsFalse(dg.HasDependents("x"));
    }

    /// <summary>
    ///   Removing from an empty DG shouldn't fail.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("4")]
    public void RemoveDependency_EmptyGraph_NoEffect()
    {
        DependencyGraph dg = new();
        Assert.IsFalse(dg.HasDependents("x"));
        dg.RemoveDependency("x", "y");
        Assert.IsFalse(dg.HasDependents("x"));
    }

    /// <summary>
    ///   Replace on an empty DG shouldn't fail.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("5")]
    public void ReplaceDependents_EmptyGraph_NoEffect()
    {
        DependencyGraph dg = new();
        dg.ReplaceDependents("x", []);
    }

    /// <summary>
    ///   Replace on an empty DG shouldn't fail.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("6")]
    public void ReplaceDependees_EmptyGraph_NoEffect()
    {
        DependencyGraph dg = new();
        dg.ReplaceDependees("y", []);
    }

    // ************************ MORE TESTS ON EMPTY DGs *********************** //

    /// <summary>
    ///   Add one element, check the size, remove the element, check the size.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("7")]
    public void AddRemove_FromEmpty_OneThenZero()
    {
        DependencyGraph dg = new();
        dg.AddDependency("x", "y");
        Assert.AreEqual(1, dg.Size);
        dg.RemoveDependency("x", "y");
        Assert.AreEqual(0, dg.Size);
    }

    /// <summary>
    ///   Add multiple (two) elements, check the size, remove them, check the size.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("8")]
    public void AddRemove_FromEmpty_TwoThenZero()
    {
        DependencyGraph t = new();
        t.AddDependency("x", "y");
        Assert.IsTrue(t.HasDependees("y"));
        Assert.IsTrue(t.HasDependents("x"));
        t.RemoveDependency("x", "y");
        Assert.IsFalse(t.HasDependees("y"));
        Assert.IsFalse(t.HasDependents("x"));
    }

    /// <summary>
    ///    <para>
    ///      Check if after adding the x->y we have the correct dependees and
    ///      the correct dependents.
    ///    </para>
    ///    <para>
    ///      After removing, there should be no contents left.
    ///    </para>
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("9")]
    public void AddAndRemoveDependency_FromEmpty_Valid()
    {
        DependencyGraph dg = new();
        dg.AddDependency("x", "y");

        Assert.IsTrue(dg.GetDependees("y").Matches(["x"]));
        Assert.IsTrue(dg.GetDependents("x").Matches(["y"]));

        dg.RemoveDependency("x", "y");

        Assert.IsTrue(dg.GetDependees("y").Matches([]));
        Assert.IsTrue(dg.GetDependents("x").Matches([]));
    }

    /// <summary>
    ///   Add a dependency and the number of dependees should be 1.
    ///   Remove it, and the number should be zero.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("10")]
    public void GetDependees_AddThenRemove_SizeOneThenZero()
    {
        DependencyGraph dg = new();
        dg.AddDependency("x", "y");
        Assert.AreEqual(1, dg.GetDependees("y").Count());
        dg.RemoveDependency("x", "y");
        Assert.AreEqual(0, dg.GetDependees("x").Count());
    }

    /// <summary>
    ///   Removing from an empty DG shouldn't fail.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("11")]
    public void RemoveDependency_DoItTwice_ShouldNotError()
    {
        DependencyGraph dg = new();
        dg.AddDependency("x", "y");
        Assert.AreEqual(dg.Size, 1);
        dg.RemoveDependency("x", "y");
        Assert.AreEqual(dg.Size, 0);
        dg.RemoveDependency("x", "y");
        Assert.AreEqual(dg.Size, 0);
    }

    /// <summary>
    ///   Replace dependencies with an empty list should not fail.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("12")]
    public void RemoveAndReplace_CanReplaceWithEmpty()
    {
        DependencyGraph dg = new();
        dg.AddDependency("x", "y");
        Assert.AreEqual(dg.Size, 1);

        dg.RemoveDependency("x", "y");
        dg.ReplaceDependents("x", []);
        dg.ReplaceDependees("y", []);
        Assert.AreEqual(dg.Size, 0);
    }

    // ********************** Making Sure that Static Variables Weren't Used ****************** //

    /// <summary>
    ///   <para>
    ///     It should be possible to have more than one DG at a time.
    ///   </para>
    ///   <remark>
    ///     This test is repeated to increase its weighting in the grading.
    ///   </remark>
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("13")]
    public void Constructor_CreateMultipleInstances_EachIsIndependentOfTheOther_1()
    {
        DependencyGraph dg_1 = new();
        DependencyGraph dg_2 = new();
        dg_1.AddDependency("x", "y");
        Assert.AreEqual(1, dg_1.Size);
        Assert.AreEqual(0, dg_2.Size);
    }

    /// <summary>
    ///   Increase weight of Previous Test. Not necessary in Non-Grading test suite.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("14")]
    public void Constructor_CreateMultipleInstances_EachIsIndependentOfTheOther_2()
    {
        Constructor_CreateMultipleInstances_EachIsIndependentOfTheOther_1();
    }

    /// <summary>
    ///   Increase weight of Previous Test. Not necessary in Non-Grading test suite.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("15")]
    public void TestStatic1_3()
    {
        Constructor_CreateMultipleInstances_EachIsIndependentOfTheOther_1();
    }

    /// <summary>
    ///   Increase weight of Previous Test. Not necessary in Non-Grading test suite.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("16")]
    public void TestStatic1_4()
    {
        Constructor_CreateMultipleInstances_EachIsIndependentOfTheOther_1();
    }

    /// <summary>
    ///   Increase weight of Previous Test. Not necessary in Non-Grading test suite.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("17")]
    public void TestStatic1_5()
    {
        Constructor_CreateMultipleInstances_EachIsIndependentOfTheOther_1();
    }

    /**************************** SIMPLE NON-EMPTY TESTS ****************************/

    /// <summary>
    ///   Add four dependencies and the size should be four.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("18")]
    public void Size_AddFour_ResultIsFour()
    {
        DependencyGraph dg = new();
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");
        dg.AddDependency("c", "b");
        dg.AddDependency("b", "d");
        Assert.AreEqual(4, dg.Size);
    }

    /// <summary>
    ///    After adding two items that depend on "b", check the dependees on the "b" node.
    ///    There should be two.
    ///  </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("19")]
    public void AddDependencyGetDependees_AddTwoDependenciesToSameNode_GetDependeesValueOfTwo()
    {
        DependencyGraph dg = new();

        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");
        dg.AddDependency("c", "b");
        dg.AddDependency("b", "d");

        var dependees = dg.GetDependees("b");

        Assert.AreEqual(2, dependees.Count());
        Assert.IsTrue(dependees.Matches(["a", "c"]));
    }

    /// <summary>
    ///   Given a -> b, a -> c, c -> b, and b -> d.
    ///   a has dependents. a does not have dependees.  b had both dependents and dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("20")]
    public void HasDependentsHasDependees_SmallGraph_ReturnsAppropriateTrueFalse()
    {
        DependencyGraph dg = new();
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");
        dg.AddDependency("c", "b");
        dg.AddDependency("b", "d");
        Assert.IsTrue(dg.HasDependents("a"));
        Assert.IsFalse(dg.HasDependees("a"));
        Assert.IsTrue(dg.HasDependents("b"));
        Assert.IsTrue(dg.HasDependees("b"));
    }

    /// <summary>
    ///    Check that the simple graph contains the right dependents and dependees.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("21")]
    public void GetDependeesGetDependents_SimpleGraph_ContainsCorrectValues()
    {
        DependencyGraph dg = new();
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");
        dg.AddDependency("c", "b");
        dg.AddDependency("b", "d");

        IEnumerable<string> dependentsOfA = dg.GetDependents("a");
        IEnumerable<string> dependeesOfB = dg.GetDependees("b");

        Assert.IsTrue(dependentsOfA.Matches(["b", "c"]));
        Assert.IsTrue(dependeesOfB.Matches(["a", "c"]));
    }

    /// <summary>
    ///   Test that a simple graph does not have values for items
    ///   that were not added to the graph....
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("22")]
    public void GetDependentsGetDependees_GetValuesFromValidandInvalidNodes_ReturnsZeroOrOne()
    {
        DependencyGraph dg = new();
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");
        dg.AddDependency("c", "b");
        dg.AddDependency("b", "d");

        IEnumerable<string> dependeesOfE = dg.GetDependees("e");
        IEnumerable<string> dependentsOfE = dg.GetDependents("e");
        IEnumerable<string> dependeesOfF = dg.GetDependees("f");
        IEnumerable<string> dependentsOfF = dg.GetDependents("f");
        IEnumerable<string> dependeesOfD = dg.GetDependees("d");

        Assert.AreEqual(0, dependeesOfE.Count());
        Assert.AreEqual(0, dependeesOfF.Count());
        Assert.AreEqual(1, dependeesOfD.Count());
        Assert.AreEqual(0, dependentsOfE.Count());
        Assert.AreEqual(0, dependentsOfF.Count());
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("23")]
    public void AddDependency_Duplicates_CorrectSize()
    {
        DependencyGraph t = new();
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("a", "b");
        t.AddDependency("c", "b");
        t.AddDependency("b", "d");
        t.AddDependency("c", "b");
        Assert.AreEqual(4, t.Size);
    }

    /// <summary>
    ///   Test that "set" functionality is correct. Cannot add the same item multiple times.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("24")]
    public void AddDependency_AddTheSameItemMultipleTimes_ShouldOnlyAddOnce()
    {
        DependencyGraph t = new();
        t.AddDependency("a", "b");
        t.AddDependency("a", "b");
        t.AddDependency("c", "b");
        t.AddDependency("c", "b");
        Assert.AreEqual(2, t.GetDependees("b").Count());
        Assert.AreEqual(1, t.GetDependents("a").Count());
        Assert.AreEqual(1, t.GetDependents("c").Count());
    }

    /// <summary>
    ///   Attempting to add the same dependency multiple time also
    ///   does not affect the HasDependents functionality.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("25")]
    public void HasDependentsDependees_AddDuplicatesDependencies_DoesNotChangeGraph()
    {
        DependencyGraph t = new();
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("a", "b");
        t.AddDependency("c", "b");
        t.AddDependency("b", "d");
        t.AddDependency("c", "b");
        Assert.IsTrue(t.HasDependents("a"));
        Assert.IsFalse(t.HasDependees("a"));
        Assert.IsTrue(t.HasDependents("b"));
        Assert.IsTrue(t.HasDependees("b"));
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("26")]
    public void AddDependency_Duplicates_CorrectContents()
    {
        DependencyGraph t = new();
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("a", "b");
        t.AddDependency("c", "b");
        t.AddDependency("b", "d");
        t.AddDependency("c", "b");

        IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependees("b").GetEnumerator();
        Assert.IsTrue(e.MoveNext());

        string s1 = e.Current;
        Assert.IsTrue(e.MoveNext());
        string s2 = e.Current;
        Assert.IsFalse(e.MoveNext());
        Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

        e = t.GetDependees("c").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("a", e.Current);
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependees("d").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("b", e.Current);
        Assert.IsFalse(e.MoveNext());
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("27")]
    public void AddDependency_Duplicates_CorrectContents2()
    {
        DependencyGraph t = new();
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("a", "b");
        t.AddDependency("c", "b");
        t.AddDependency("b", "d");
        t.AddDependency("c", "b");

        IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
        Assert.IsTrue(e.MoveNext());

        string s1 = e.Current;
        Assert.IsTrue(e.MoveNext());
        string s2 = e.Current;
        Assert.IsFalse(e.MoveNext());
        Assert.IsTrue(((s1 == "b") && (s2 == "c")) || ((s1 == "c") && (s2 == "b")));

        e = t.GetDependents("b").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("d", e.Current);
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependents("c").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("b", e.Current);
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependents("d").GetEnumerator();
        Assert.IsFalse(e.MoveNext());
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("28")]
    public void AddRemove_ComplexDependencies_CorrectSize()
    {
        DependencyGraph t = new();
        t.AddDependency("x", "y");
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("a", "d");
        t.AddDependency("c", "b");
        t.RemoveDependency("a", "d");
        t.AddDependency("e", "b");
        t.AddDependency("b", "d");
        t.RemoveDependency("e", "b");
        t.RemoveDependency("x", "y");
        Assert.AreEqual(4, t.Size);
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("29")]
    public void AddRemove_ComplexDependencies_CorrectDependeesCount()
    {
        DependencyGraph t = new();
        t.AddDependency("x", "y");
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("a", "d");
        t.AddDependency("c", "b");
        t.RemoveDependency("a", "d");
        t.AddDependency("e", "b");
        t.AddDependency("b", "d");
        t.RemoveDependency("e", "b");
        t.RemoveDependency("x", "y");
        Assert.AreEqual(2, t.GetDependees("b").Count());
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("30")]
    public void AddRemove_ComplexDependencies_CorrectContents()
    {
        DependencyGraph t = new();
        t.AddDependency("x", "y");
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("a", "d");
        t.AddDependency("c", "b");
        t.RemoveDependency("a", "d");
        t.AddDependency("e", "b");
        t.AddDependency("b", "d");
        t.RemoveDependency("e", "b");
        t.RemoveDependency("x", "y");
        Assert.IsTrue(t.HasDependents("a"));
        Assert.IsFalse(t.HasDependees("a"));
        Assert.IsTrue(t.HasDependents("b"));
        Assert.IsTrue(t.HasDependees("b"));
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("31")]
    public void AddRemove_ComplexDependencies_CorrectContents2()
    {
        DependencyGraph t = new();
        t.AddDependency("x", "y");
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("a", "d");
        t.AddDependency("c", "b");
        t.RemoveDependency("a", "d");
        t.AddDependency("e", "b");
        t.AddDependency("b", "d");
        t.RemoveDependency("e", "b");
        t.RemoveDependency("x", "y");

        IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependees("b").GetEnumerator();
        Assert.IsTrue(e.MoveNext());

        string s1 = e.Current;
        Assert.IsTrue(e.MoveNext());
        string s2 = e.Current;
        Assert.IsFalse(e.MoveNext());
        Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

        e = t.GetDependees("c").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("a", e.Current);
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependees("d").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("b", e.Current);
        Assert.IsFalse(e.MoveNext());
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("32")]
    public void AddRemove_ComplexDependencies_CorrectContents3()
    {
        DependencyGraph t = new();
        t.AddDependency("x", "y");
        t.AddDependency("a", "b");
        t.AddDependency("a", "c");
        t.AddDependency("a", "d");
        t.AddDependency("c", "b");
        t.RemoveDependency("a", "d");
        t.AddDependency("e", "b");
        t.AddDependency("b", "d");
        t.RemoveDependency("e", "b");
        t.RemoveDependency("x", "y");

        IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
        Assert.IsTrue(e.MoveNext());

        string s1 = e.Current;
        Assert.IsTrue(e.MoveNext());
        string s2 = e.Current;
        Assert.IsFalse(e.MoveNext());
        Assert.IsTrue(((s1 == "b") && (s2 == "c")) || ((s1 == "c") && (s2 == "b")));

        e = t.GetDependents("b").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("d", e.Current);
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependents("c").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("b", e.Current);
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependents("d").GetEnumerator();
        Assert.IsFalse(e.MoveNext());
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("33")]
    public void ReplaceDependees_OnEmptyGraph_AddsNewDependees()
    {
        DependencyGraph dg = new();

        dg.ReplaceDependees("b", ["a"]);

        Assert.AreEqual(1, dg.Size);
        Assert.IsTrue(new HashSet<string> { "b" }.SetEquals(dg.GetDependents("a")));
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("34")]
    public void Replace_ComplexDependencies_CorrectSize()
    {
        DependencyGraph t = new();
        t.AddDependency("x", "b");
        t.AddDependency("a", "z");
        t.ReplaceDependents("b", []);
        t.AddDependency("y", "b");
        t.ReplaceDependents("a", ["c"]);
        t.AddDependency("w", "d");
        t.ReplaceDependees("b", ["a", "c"]);
        t.ReplaceDependees("d", ["b"]);
        Assert.AreEqual(4, t.Size);
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("35")]
    public void Replace_ComplexDependencies_CorrectDependeesCount()
    {
        DependencyGraph t = new();
        t.AddDependency("x", "b");
        t.AddDependency("a", "z");
        t.ReplaceDependents("b", []);
        t.AddDependency("y", "b");
        t.ReplaceDependents("a", ["c"]);
        t.AddDependency("w", "d");
        t.ReplaceDependees("b", ["a", "c"]);
        t.ReplaceDependees("d", ["b"]);
        Assert.AreEqual(2, t.GetDependees("b").Count());
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("36")]
    public void Replace_ComplexDependencies_CorrectContents()
    {
        DependencyGraph t = new();
        t.AddDependency("x", "b");
        t.AddDependency("a", "z");
        t.ReplaceDependents("b", []);
        t.AddDependency("y", "b");
        t.ReplaceDependents("a", ["c"]);
        t.AddDependency("w", "d");
        t.ReplaceDependees("b", ["a", "c"]);
        t.ReplaceDependees("d", ["b"]);
        Assert.IsTrue(t.HasDependents("a"));
        Assert.IsFalse(t.HasDependees("a"));
        Assert.IsTrue(t.HasDependents("b"));
        Assert.IsTrue(t.HasDependees("b"));
    }


    [TestMethod]
    [Timeout(2000)]
    [TestCategory("37")]
    public void Replace_ComplexDependencies_CorrectContents2()
    {
        DependencyGraph t = SetupComplexDependencies();

        IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependees("b").GetEnumerator();
        Assert.IsTrue(e.MoveNext());

        string s1 = e.Current;
        Assert.IsTrue(e.MoveNext());
        string s2 = e.Current;
        Assert.IsFalse(e.MoveNext());
        Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

        e = t.GetDependees("c").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("a", e.Current);
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependees("d").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("b", e.Current);
        Assert.IsFalse(e.MoveNext());
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("38")]
    public void Replace_ComplexDependencies_CorrectContents3()
    {
        DependencyGraph t = SetupComplexDependencies();

        IEnumerator<string> e = t.GetDependents("a").GetEnumerator();
        Assert.IsTrue(e.MoveNext());

        string s1 = e.Current;
        Assert.IsTrue(e.MoveNext());
        string s2 = e.Current;
        Assert.IsFalse(e.MoveNext());
        Assert.IsTrue(((s1 == "b") && (s2 == "c")) || ((s1 == "c") && (s2 == "b")));

        e = t.GetDependents("b").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("d", e.Current);
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependents("c").GetEnumerator();
        Assert.IsTrue(e.MoveNext());
        Assert.AreEqual("b", e.Current);
        Assert.IsFalse(e.MoveNext());

        e = t.GetDependents("d").GetEnumerator();
        Assert.IsFalse(e.MoveNext());
    }

    // ************************** STRESS TESTS REPEATED MULTIPLE TIMES ******************************** //

    /// <summary>
    ///    Using lots of data.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("39")]
    public void StressTest1_1()
    {
        // Dependency graph
        DependencyGraph t = new();

        // A bunch of strings to use
        const int SIZE = 200;
        string[] letters = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            letters[i] = "a" + i;
        }

        // The correct answers
        HashSet<string>[] dents = new HashSet<string>[SIZE];
        HashSet<string>[] dees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dents[i] = [];
            dees[i] = [];
        }

        // Add a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                t.AddDependency(letters[i], letters[j]);
                dents[i].Add(letters[j]);
                dees[j].Add(letters[i]);
            }
        }

        // Remove a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 4; j < SIZE; j += 4)
            {
                t.RemoveDependency(letters[i], letters[j]);
                dents[i].Remove(letters[j]);
                dees[j].Remove(letters[i]);
            }
        }

        // Add some back
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j += 2)
            {
                t.AddDependency(letters[i], letters[j]);
                dents[i].Add(letters[j]);
                dees[j].Add(letters[i]);
            }
        }

        // Remove some more
        for (int i = 0; i < SIZE; i += 2)
        {
            for (int j = i + 3; j < SIZE; j += 3)
            {
                t.RemoveDependency(letters[i], letters[j]);
                dents[i].Remove(letters[j]);
                dees[j].Remove(letters[i]);
            }
        }

        // Make sure everything is right
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
            Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
        }
    }

    /// <summary>
    ///   Increase weight of Stress Test 1.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("40")]
    public void StressTest1_2()
    {
        StressTest1_1();
    }

    /// <summary>
    ///   Increase weight of Stress Test 1.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("41")]
    public void StressTest1_3()
    {
        StressTest1_1();
    }

    // ********************************** ANOTHER STESS TEST, REPEATED ******************** //

    /// <summary>
    ///    Using lots of data with replacement. 
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("42")]
    public void StressTest8_1()
    {
        // Dependency graph
        DependencyGraph t = new();

        // A bunch of strings to use
        const int SIZE = 400;
        string[] letters = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            letters[i] = "a" + i;
        }

        // The correct answers
        HashSet<string>[] dents = new HashSet<string>[SIZE];
        HashSet<string>[] dees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dents[i] = [];
            dees[i] = [];
        }

        // Add a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                t.AddDependency(letters[i], letters[j]);
                dents[i].Add(letters[j]);
                dees[j].Add(letters[i]);
            }
        }

        // Remove a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 2; j < SIZE; j += 3)
            {
                t.RemoveDependency(letters[i], letters[j]);
                dents[i].Remove(letters[j]);
                dees[j].Remove(letters[i]);
            }
        }

        // Replace a bunch of dependents
        for (int i = 0; i < SIZE; i += 2)
        {
            HashSet<string> newDents = [];
            for (int j = 0; j < SIZE; j += 5)
            {
                newDents.Add(letters[j]);
            }

            t.ReplaceDependents(letters[i], newDents);

            foreach (string s in dents[i])
            {
                dees[int.Parse(s[1..])].Remove(letters[i]);
            }

            foreach (string s in newDents)
            {
                dees[int.Parse(s[1..])].Add(letters[i]);
            }

            dents[i] = newDents;
        }

        // Make sure everything is right
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
            Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
        }
    }

    /// <summary>
    ///   Increase weight of StressTest8.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("43")]
    public void StressTest8_2()
    {
        StressTest8_1();
    }

    /// <summary>
    ///   Increase weight of StressTest8.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("44")]
    public void StressTest10()
    {
        StressTest8_1();
    }

    // ********************************** A THIRD STESS TEST, REPEATED ******************** //

    /// <summary>
    ///    Using lots of data with replacement.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("45")]
    public void StressTest15_1()
    {
        // Dependency graph
        DependencyGraph t = new();

        // A bunch of strings to use
        const int SIZE = 1000;
        string[] letters = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            letters[i] = "a" + i;
        }

        // The correct answers
        HashSet<string>[] dents = new HashSet<string>[SIZE];
        HashSet<string>[] dees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dents[i] = [];
            dees[i] = [];
        }

        // Add a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                t.AddDependency(letters[i], letters[j]);
                dents[i].Add(letters[j]);
                dees[j].Add(letters[i]);
            }
        }

        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
            Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
        }

        // Remove a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 2; j < SIZE; j += 3)
            {
                t.RemoveDependency(letters[i], letters[j]);
                dents[i].Remove(letters[j]);
                dees[j].Remove(letters[i]);
            }
        }

        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
            Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
        }

        // Replace a bunch of dependees
        for (int i = 0; i < SIZE; i += 2)
        {
            HashSet<string> newDees = [];
            for (int j = 0; j < SIZE; j += 9)
            {
                newDees.Add(letters[j]);
            }

            t.ReplaceDependees(letters[i], newDees);

            foreach (string s in dees[i])
            {
                dents[int.Parse(s[1..])].Remove(letters[i]);
            }

            foreach (string s in newDees)
            {
                dents[int.Parse(s[1..])].Add(letters[i]);
            }

            dees[i] = newDees;
        }

        // Make sure everything is right
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
            Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
        }
    }

    /// <summary>
    ///   Increase weight of StressTest15.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("46")]
    public void StressTest15_2()
    {
        StressTest15_1();
    }

    /// <summary>
    ///   Increase weight of StressTest15.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("47")]
    public void StressTest17()
    {
        StressTest15_1();
    }

    /// <summary>
    ///   Helper code to build a simple dependency graph.
    /// </summary>
    /// <returns> The new graph. </returns>
    private static DependencyGraph SetupComplexDependencies()
    {
        DependencyGraph t = new();
        t.AddDependency("x", "b");
        t.AddDependency("a", "z");
        t.ReplaceDependents("b", []);
        t.AddDependency("y", "b");
        t.ReplaceDependents("a", ["c"]);
        t.AddDependency("w", "d");
        t.ReplaceDependees("b", ["a", "c"]);
        t.ReplaceDependees("d", ["b"]);
        return t;
    }

}

/// <summary>
///   Helper methods for the tests above.
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    ///   Check to see if the two "sets" (source and items) match, i.e.,
    ///   contain exactly the same values.
    /// </summary>
    /// <param name="source"> original container.</param>
    /// <param name="items"> elements to match against.</param>
    /// <returns> true if every element in source is in items and vice versa. They are the "same set".</returns>
    public static bool Matches(this IEnumerable<string> source, params string[] items)
    {
        return (source.Count() == items.Length) && items.All(item => source.Contains(item));
    }

}