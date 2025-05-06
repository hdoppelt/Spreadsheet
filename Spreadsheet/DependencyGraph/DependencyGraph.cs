namespace CS3500.DependencyGraph;

/// <summary>
///   <para>
///     (s1,t1) is an ordered pair of strings, meaning t1 depends on s1.
///     (in other words: s1 must be evaluated before t1.)
///   </para>
///   <para>
///     A DependencyGraph can be modeled as a set of ordered pairs of strings.
///     Two ordered pairs (s1,t1) and (s2,t2) are considered equal if and only
///     if s1 equals s2 and t1 equals t2.
///   </para>
///   <remarks>
///     Recall that sets never contain duplicates.
///     If an attempt is made to add an element to a set, and the element is already
///     in the set, the set remains unchanged.
///   </remarks>
///   <para>
///     Given a DependencyGraph DG:
///   </para>
///   <list type="number">
///     <item>
///       If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
///       (The set of things that depend on s.)
///     </item>
///     <item>
///       If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
///       (The set of things that s depends on.)
///     </item>
///   </list>
///   <para>
///      For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}.
///   </para>
///   <code>
///     dependents("a") = {"b", "c"}
///     dependents("b") = {"d"}
///     dependents("c") = {}
///     dependents("d") = {"d"}
///     dependees("a")  = {}
///     dependees("b")  = {"a"}
///     dependees("c")  = {"a"}
///     dependees("d")  = {"b", "d"}
///   </code>
/// </summary>
public class DependencyGraph
{

    /// <summary>
    ///     Dictionary to store the dependents of each node
    /// </summary>
    private Dictionary<string, HashSet<string>> dependents;

    /// <summary>
    ///     Dictionary to store the dependees of each node
    /// </summary>
    private Dictionary<string, HashSet<string>> dependees;

    /// <summary>
    ///   Initializes a new instance of the <see cref="DependencyGraph"/> class.
    ///   The initial DependencyGraph is empty.
    /// </summary>
    public DependencyGraph()
    {

        // Initialize empty dictionaries for dependents
        dependents = new Dictionary<string, HashSet<string>>();

        // Initialize empty dictionaries for dependees
        dependees = new Dictionary<string, HashSet<string>>();

    }

    /// <summary>
    /// <para>Adds the ordered pair (dependee, dependent), if it doesn't exist.</para>
    ///
    /// <para>
    ///   This can be thought of as: dependee must be evaluated before dependent
    /// </para>
    /// </summary>
    /// <param name="dependee"> the name of the node that must be evaluated first</param>
    /// <param name="dependent"> the name of the node that cannot be evaluated until after dependee</param>
    public void AddDependency(string dependee, string dependent)
    {

        // Add the dependent to the dependents of the dependee (forward relation)
        if (!dependents.ContainsKey(dependee))
        {
            dependents[dependee] = new HashSet<string>();
        }

        dependents[dependee].Add(dependent);

        // Add the dependee to the dependees of the dependent (backward relation)
        if (!dependees.ContainsKey(dependent))
        {
            dependees[dependent] = new HashSet<string>();
        }

        dependees[dependent].Add(dependee);
    }

    /// <summary>
    ///   <para>
    ///     Removes the ordered pair (dependee, dependent), if it exists.
    ///   </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first</param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until after dependee</param>
    public void RemoveDependency(string dependee, string dependent)
    {

        // Remove the dependent from the dependents of the dependee (forward relation)
        if (dependents.ContainsKey(dependee))
        {
            dependents[dependee].Remove(dependent);

            // If the set of dependents is now empty, remove the dependee from the dictionary
            if (dependents[dependee].Count == 0)
            {
                dependents.Remove(dependee);
            }
        }

        // Remove the dependee from the dependees of the dependent (backward relation)
        if (dependees.ContainsKey(dependent))
        {
            dependees[dependent].Remove(dependee);

            // If the set of dependees is now empty, remove the dependent from the dictionary
            if (dependees[dependent].Count == 0)
            {
                dependees.Remove(dependent);
            }
        }

    }

    /// <summary>
    ///   Reports whether the given node has dependents (i.e., other nodes depend on it).
    /// </summary>
    /// <param name="nodeName"> The name of the node.</param>
    /// <returns> true if the node has dependents. </returns>
    public bool HasDependents(string nodeName)
    {
        // Check if the node exists in the dependents dictionary and has at least one dependent
        return dependents.ContainsKey(nodeName) && dependents[nodeName].Count > 0;
    }

    /// <summary>
    ///   Reports whether the given node has dependees (i.e., depends on one or more other nodes).
    /// </summary>
    /// <returns> true if the node has dependees.</returns>
    /// <param name="nodeName">The name of the node.</param>
    public bool HasDependees(string nodeName)
    {
        // Check if the node exists in the dependees dictionary and has at least one dependee
        return dependees.ContainsKey(nodeName) && dependees[nodeName].Count > 0;
    }

    /// <summary>
    /// The number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
        get
        {
            int size = 0;

            // Iterate over each entry in the dependents dictionary
            foreach (var dependentsSet in dependents.Values)
            {
                // Add the number of dependents for each node to the total size
                size += dependentsSet.Count;
            }

            return size;
        }
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependents of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependents of nodeName. </returns>
    public IEnumerable<string> GetDependents(string nodeName)
    {
        if (dependents.ContainsKey(nodeName))
        {
            // Return the dependents of the node
            return dependents[nodeName];
        }

        // If the node has no dependents, return an empty list
        return new List<string>();
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependees of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependees of nodeName. </returns>
    public IEnumerable<string> GetDependees(string nodeName)
    {
        if (dependees.ContainsKey(nodeName))
        {
            // Return the dependees of the node
            return dependees[nodeName];
        }

        // If the node has no dependees, return an empty list
        return new List<string>();
    }

    /// <summary>
    ///   Removes all existing ordered pairs of the form (nodeName, *).  Then, for each
    ///   t in newDependents, adds the ordered pair (nodeName, t).
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependents are being replaced </param>
    /// <param name="newDependents"> The new dependents for nodeName</param>
    public void ReplaceDependents(string nodeName, IEnumerable<string> newDependents)
    {

        // Remove all current dependents of the node (if any)
        if (dependents.ContainsKey(nodeName))
        {
            foreach (var dependent in dependents[nodeName])
            {

                // Remove the corresponding dependees (backward relation)
                dependees[dependent].Remove(nodeName);

                // If the dependent has no more dependees, remove it from dependees
                if (dependees[dependent].Count == 0)
                {
                    dependees.Remove(dependent);
                }
            }

            dependents.Remove(nodeName);
        }

        // Add the new dependents
        foreach (var dependent in newDependents)
        {

            // Add to dependents dictionary
            if (!dependents.ContainsKey(nodeName))
            {
                dependents[nodeName] = new HashSet<string>();
            }

            dependents[nodeName].Add(dependent);

            // Add to dependees dictionary (backward relation)
            if (!dependees.ContainsKey(dependent))
            {
                dependees[dependent] = new HashSet<string>();
            }

            dependees[dependent].Add(nodeName);
        }
    }

    /// <summary>
    ///   <para>
    ///     Removes all existing ordered pairs of the form (*, nodeName).  Then, for each
    ///     t in newDependees, adds the ordered pair (t, nodeName).
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependees are being replaced</param>
    /// <param name="newDependees"> The new dependees for nodeName</param>
    public void ReplaceDependees(string nodeName, IEnumerable<string> newDependees)
    {

        // Remove all current dependees of the node (if any)
        if (dependees.ContainsKey(nodeName))
        {
            foreach (var dependee in dependees[nodeName])
            {

                // Remove the corresponding dependents (forward relation)
                dependents[dependee].Remove(nodeName);

                // If the dependee has no more dependents, remove it from dependents
                if (dependents[dependee].Count == 0)
                {
                    dependents.Remove(dependee);
                }
            }

            dependees.Remove(nodeName);
        }

        // Add the new dependees
        foreach (var dependee in newDependees)
        {

            // Add to dependees dictionary
            if (!dependees.ContainsKey(nodeName))
            {
                dependees[nodeName] = new HashSet<string>();
            }

            dependees[nodeName].Add(dependee);

            // Add to dependents dictionary (forward relation)
            if (!dependents.ContainsKey(dependee))
            {
                dependents[dependee] = new HashSet<string>();
            }

            dependents[dependee].Add(nodeName);
        }
    }

}