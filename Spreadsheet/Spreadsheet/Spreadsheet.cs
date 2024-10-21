/// <copyright file="Spreadsheet.cs" company="UofU-CS3500">
///     Copyright (c) 2024 UofU-CS3500. All rights reserved.
/// </copyright>
///
/// Written by Joe Zachary for CS 3500, September 2013
/// Update by Profs Kopta and de St. Germain, Fall 2021, Fall 2024
///     - Updated return types
///     - Updated documentation
///
/// Name: Harrison Doppelt
/// Date: 09/27/2024

namespace CS3500.Spreadsheet;

using CS3500.Formula;
using CS3500.DependencyGraph;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.VisualBasic;
using System.Text.Json.Serialization;

/// <summary>
///     <para>
///         Thrown to indicate that a read or write attempt has failed with
///         an expected error message informing the user of what went wrong.
///     </para>
/// </summary>
public class SpreadsheetReadWriteException : Exception
{

    /// <summary>
    ///     <para>
    ///         Creates the exception with a message defining what went wrong.
    ///     </para>
    /// </summary>
    /// 
    /// <param name="msg"> An informative message to the user. </param>
    public SpreadsheetReadWriteException(string msg)
    : base(msg)
    {
    }

}

/// <summary>
///     <para>
///         Thrown to indicate that a change to a cell will cause a circular dependency.
///     </para>
/// </summary>
public class CircularException : Exception
{
}

/// <summary>
///     <para>
///         Thrown to indicate that a name parameter was invalid.
///     </para>
/// </summary>
public class InvalidNameException : Exception
{
}

/// <summary>
/// 
///     <para>
///         An Spreadsheet object represents the state of a simple spreadsheet.  A
///         spreadsheet represents an infinite number of named cells.
///     </para>
///     
///     <para>
///         Valid Cell Names: A string is a valid cell name if and only if it is one or
///         more letters followed by one or more numbers, e.g., A5, BC27.
///     </para>
///     
///     <para>
///         Cell names are case insensitive, so "x1" and "X1" are the same cell name.
///         Your code should normalize (uppercased) any stored name but accept either.
///     </para>
///     
///     <para>
///         A spreadsheet represents a cell corresponding to every possible cell name.  (This
///         means that a spreadsheet contains an infinite number of cells.)  In addition to
///         a name, each cell has a contents and a value.  The distinction is important.
///     </para>
///     
///     <para>
///         The <b>contents</b> of a cell can be (1) a string, (2) a double, or (3) a Formula.
///         If the contents of a cell is set to the empty string, the cell is considered empty.
///     </para>
///     
///     <para>
///         By analogy, the contents of a cell in Excel is what is displayed on
///         the editing line when the cell is selected.
///     </para>
///     
///     <para>
///         In a new spreadsheet, the contents of every cell is the empty string. Note:
///         this is by definition (it is IMPLIED, not stored).
///     </para>
///     
///     <para>
///         The <b>value</b> of a cell can be (1) a string, (2) a double, or (3) a FormulaError.
///         (By analogy, the value of an Excel cell is what is displayed in that cell's position
///         in the grid.) We are not concerned with cell values yet, only with their contents,
///         but for context:
///     </para>
///     
///     <list type="number">
///         <item> If a cell's contents is a string, its value is that string. </item>
///         <item> If a cell's contents is a double, its value is that double. </item>
///         <item>
///             <para>
///                 If a cell's contents is a Formula, its value is either a double or a FormulaError,
///                 as reported by the Evaluate method of the Formula class.  For this assignment,
///                 you are not dealing with values yet.
///             </para>
///         </item>
///     </list>
///     
///     <para>
///         Spreadsheets are never allowed to contain a combination of Formulas that establish
///         a circular dependency.  A circular dependency exists when a cell depends on itself,
///         either directly or indirectly.
///         For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
///         A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
///         dependency.
///     </para>
///     
/// </summary>
public class Spreadsheet
{
    /// <summary>
    ///     Dictionary to map cell names to their corresponding Cell objects
    /// </summary>
    private Dictionary<string, Cell> Cells;

    /// <summary>
    ///     DependencyGraph to track dependencies between cells based on their names
    /// </summary>
    private DependencyGraph dependencyGraph = new DependencyGraph();

    public Spreadsheet()
    {
        Cells = new Dictionary<string, Cell>();
    }

    /// <summary>
    ///     Represents a cell that contains a name and contents.
    ///     Contents can be a double, string, or Fomula.
    /// </summary>
    private class Cell
    {
        /// <summary>
        ///     The contents of the cell.
        ///     Can be a double, string, or Formula.
        /// </summary>
        public object Contents { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// 
        /// <param name="contents">
        ///     The contents of the cell. Can be a double, string, or Formula.
        /// </param>
        /// 
        /// <remarks>
        ///     The contents of the cell can be a formula, a string, or a numeric value.
        ///     The <see cref="Contents"/> property is set based on this parameter.
        /// </remarks>
        public Cell(object contents)
        {
            Contents = contents;
        }
    }

    /// <summary>
    ///     True if this spreadsheet has been changed since it was 
    ///     created or saved (whichever happened most recently),
    ///     False otherwise.
    /// </summary>
    public bool Changed { get; private set; }

    /// <summary>
    ///     Provides a copy of the normalized names of all of the cells in the spreadsheet
    ///     that contain information (i.e., non-empty cells).
    /// </summary>
    /// 
    /// <returns>
    ///     A set of the names of all the non-empty cells in the spreadsheet.
    /// </returns>
    public ISet<string> GetNamesOfAllNonemptyCells()
    {
        return new HashSet<string>(Cells.Keys);
    }

    /// <summary>
    ///     Returns the contents (as opposed to the value) of the named cell.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///     Thrown if the name is invalid.
    ///     Done with ValidateCellName helper method.
    /// </exception>
    ///
    /// <param name="name">The name of the spreadsheet cell to query. </param>
    /// 
    /// <returns>
    ///     The contents as either a string, a double, or a Formula.
    ///     See the class header summary.
    /// </returns>
    public object GetCellContents(string name)
    {
        ValidateCellName(name);
        name = name.ToUpper();

        // If the cell exists in the dictionary, return contents
        if (Cells.ContainsKey(name))
        {
            return Cells[name].Contents;
        }

        // If the cell doesn't exist or is empty, return an empty string
        return "";
    }

    /// <summary>
    ///     Set the contents of the named cell to the given number.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///     If the name is invalid, throw an InvalidNameException.
    ///     Done with ValidateCellName helper method.
    /// </exception>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="number"> The new contents of the cell. </param>
    /// 
    /// <returns>
    ///     <para>
    ///         This method returns an ordered list consisting of the passed in name
    ///         followed by the names of all other cells whose value depends, directly
    ///         or indirectly, on the named cell.
    ///     </para>
    ///     <para>
    ///         The order must correspond to a valid dependency ordering for recomputing
    ///         all of the cells, i.e., if you re-evaluate each cells in the order of the list,
    ///         the overall spreadsheet will be correctly updated.
    ///     </para>
    ///     <para>
    ///         For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///         list [A1, B1, C1] is returned, i.e., A1 was changed, so then A1 must be
    ///         evaluated, followed by B1, followed by C1.
    ///     </para>
    /// </returns>
    private IList<string> SetCellContents(string name, double number)
    {
        // If the cell already exists in the dictionary
        if (Cells.ContainsKey(name))
        {
            RemoveExistingDependencies(name);

            // Update the cell's contents in the dictionary
            Cells[name] = new Cell(number);
        }

        // If the cell does not exist in the dictionary
        else
        {
            // Add the cell name and contents to the dictionary
            Cells.Add(name, new Cell(number));
        }

        // Get the list of cells that need to be recalculated
        IList<string> cellsToRecalculate = GetCellsToRecalculate(name).ToList();

        // Return the list of cells that need to be recalculated
        return cellsToRecalculate;
    }

    /// <summary>
    ///     The contents of the named cell becomes the given text.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///     If the name is invalid, throw an InvalidNameException.
    ///     Done with ValidateCellName helper method.
    /// </exception>
    /// 
    /// <param name="name"> The name of the cell. </param>
    /// <param name="text"> The new contents of the cell. </param>
    /// 
    /// <returns>
    ///     The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, string text)
    {
        // If the cell already exists in the dictionary
        if (Cells.ContainsKey(name))
        {
            RemoveExistingDependencies(name);

            // If text is not an empty string
            if (text != "")
            {
                // Update the cell's contents in the dictionary
                Cells[name] = new Cell(text);
            }

            // If text is an empty string
            else
            {
                // Remove cell from dictionary
                Cells.Remove(name);
            }
        }

        // If the cell does not exist in the dictionary
        else
        {
            // If text is not an empty string
            if (text != "")
            {
                // Add the cell name and contents to the dictionary
                Cells.Add(name, new Cell(text));
            }
        }

        // Get the list of cells that need to be recalculated
        IList<string> cellsToRecalculate = GetCellsToRecalculate(name).ToList();

        // Return the list of cells that need to be recalculated
        return cellsToRecalculate;
    }

    /// <summary>
    ///     Set the contents of the named cell to the given formula.
    /// </summary>
    /// 
    /// <exception cref="InvalidNameException">
    ///     If the name is invalid, throw an InvalidNameException.
    ///     Done with ValidateCellName helper method.
    /// </exception>
    /// <exception cref="CircularException">
    ///     <para>
    ///         If changing the contents of the named cell to be the formula would
    ///         cause a circular dependency, throw a CircularException, and no
    ///         change is made to the spreadsheet.
    ///     </para>
    /// </exception>
    /// 
    /// <param name="name"> The name of the cell. </param>
    /// <param name="formula"> The new contents of the cell. </param>
    /// 
    /// <returns>
    ///     The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, Formula formula)
    {
        // Save original dependencies and contents
        IEnumerable<string> originalDependees = dependencyGraph.GetDependees(name).ToList();

        RemoveExistingDependencies(name);

        // Extract the variables from the new formula
        IEnumerable<string> variables = formula.GetVariables();

        // Loop through variables in formula
        foreach (string variable in variables)
        {
            // Add the new dependencies to the dependencyGraph
            dependencyGraph.AddDependency(variable, name);
        }

        IList<string> cellsToRecalculate;

        try
        {
            // Get the list of cells to recalculate (checks for circular dependencies)
            cellsToRecalculate = GetCellsToRecalculate(name).ToList();
        }
        catch (CircularException)
        {
            // Revert dependencies and contents if a circular exception occurs
            foreach (string variable in variables)
            {
                dependencyGraph.RemoveDependency(variable, name);
            }

            foreach (string originalDependee in originalDependees)
            {
                dependencyGraph.AddDependency(originalDependee, name);
            }

            throw;
        }

        // If the cell already exists in the dictionary
        if (Cells.ContainsKey(name))
        {
            // Update the cell's contents in the dictionary
            Cells[name] = new Cell(formula);
        }

        // If the cell does not exist in the dictionary
        else
        {
            // Add the cell name and contents to the dictionary
            Cells.Add(name, new Cell(formula));
        }

        // Return the list of cells that need to be recalculated
        return cellsToRecalculate;
    }

    /// <summary>
    ///     <para>
    ///         Set the contents of the named cell to be the provided string
    ///         which will either represent (1) a string, (2) a number, or 
    ///         (3) a formula (based on the prepended '=' character).
    ///     </para>
    ///     
    ///     <para>
    ///         Rules of parsing the input string:
    ///     </para>
    ///     <list type="bullet">
    ///     
    ///         <item>
    ///             <para>
    ///                 If 'content' parses as a double, the contents of the named
    ///                 cell becomes that double.
    ///             </para>
    ///         </item>
    ///         
    ///         <item>
    ///             If the string does not begin with an '=', the contents of the 
    ///             named cell becomes 'content'.
    ///         </item>
    ///         
    ///         <item>
    ///             <para>
    ///                 If 'content' begins with the character '=', an attempt is made
    ///                 to parse the remainder of content into a Formula f using the Formula
    ///                 constructor.  There are then three possibilities:
    ///             </para>
    ///             <list type="number">
    ///                 <item>
    ///                     If the remainder of content cannot be parsed into a Formula, a 
    ///                     CS3500.Formula.FormulaFormatException is thrown.
    ///                 </item>
    ///                 <item>
    ///                     Otherwise, if changing the contents of the named cell to be f
    ///                     would cause a circular dependency, a CircularException is thrown,
    ///                     and no change is made to the spreadsheet.
    ///                 </item>
    ///                 <item>
    ///                     Otherwise, the contents of the named cell becomes f.
    ///                 </item>
    ///             </list>
    ///         </item>
    ///         
    ///     </list>
    /// </summary>
    /// 
    /// <returns>
    ///     <para>
    ///         The method returns a list consisting of the name plus the names 
    ///         of all other cells whose value depends, directly or indirectly, 
    ///         on the named cell. The order of the list should be any order 
    ///         such that if cells are re-evaluated in that order, their dependencies 
    ///         are satisfied by the time they are evaluated.
    ///     </para>
    ///     
    ///     <example>
    ///         For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///         list {A1, B1, C1} is returned.
    ///     </example>
    /// </returns>
    /// 
    /// <exception cref="InvalidNameException">
    ///     If name is invalid, throws an InvalidNameException.
    /// </exception>
    /// 
    /// <exception cref="CircularException">
    ///     If a formula would result in a circular dependency, throws CircularException.
    /// </exception>
    public IList<string> SetContentsOfCell(string name, string content)
    {
        ValidateCellName(name);
        name = name.ToUpper();
        IList<string> cellsRecalculate;

        // If the content parses as a double (double)
        if (double.TryParse(content, out double doubleNum))
        {
            cellsRecalculate = SetCellContents(name, doubleNum);
        }

        // If the content does not begin with an '=' (string)
        else if (!content.StartsWith("="))
        {
            cellsRecalculate = SetCellContents(name, content);
        }

        // If the content begins with the character '=' (formula)
        else
        {
            // Removing '=' from the front of the string
            string contentFormula = content.Substring(1);

            try
            {
                Formula f = new Formula(contentFormula);
                cellsRecalculate = SetCellContents(name, f);
            }
            catch (CircularException)
            {
                throw;
            }
        }

        // Mark the spreadsheet as changed
        Changed = true;

        // Return the list of cells that need to be recalculated
        return cellsRecalculate;
    }

    /// <summary>
    ///     Helper Method that checks if the name is valid.
    /// </summary>
    /// 
    /// <param name="name"></param>
    /// 
    /// <exception cref="InvalidNameException">
    ///     If the name is invalid, throw an InvalidNameException.
    /// </exception>
    private void ValidateCellName(string name)
    {
        // Check if the name is a valid variable name
        if (!Formula.IsVar(name))
        {
            throw new InvalidNameException();
        }
    }

    /// <summary>
    ///     Helper method that removes all existing dependencies for the specified cell name in the dependency graph.
    /// </summary>
    /// 
    /// <param name="name"> The name of the cell </param>
    private void RemoveExistingDependencies(string name)
    {
        // Iterate through each old dependee of the current cell
        foreach (string oldDependee in dependencyGraph.GetDependees(name))
        {
            // Remove the dependency between the old dependee and the current cell
            dependencyGraph.RemoveDependency(oldDependee, name);
        }
    }

    /// <summary>
    ///     Returns an enumeration, without duplicates, of the names of all cells whose
    ///     values depend directly on the value of the named cell.
    /// </summary>
    /// 
    /// <param name="name"> This <b>MUST</b> be a valid name. </param>
    /// 
    /// <returns>
    ///     <para>
    ///         Returns an enumeration, without duplicates, of the names of all cells
    ///         that contain formulas containing name.
    ///     </para>
    ///     <para> For example, suppose that: </para>
    ///     <list type="bullet">
    ///         <item> A1 contains 3 </item>
    ///         <item> B1 contains the formula A1 * A1 </item>
    ///         <item> C1 contains the formula B1 + A1 </item>
    ///         <item> D1 contains the formula B1 - C1 </item>
    ///     </list>
    ///     <para> The direct dependents of A1 are B1 and C1. </para>
    /// </returns>
    private IEnumerable<string> GetDirectDependents(string name)
    {
        return dependencyGraph.GetDependents(name);
    }

    /// <summary>
    /// 
    ///     <para>
    ///         This method is implemented for you, but makes use of your GetDirectDependents.
    ///     </para>
    ///     <para>
    ///         Returns an enumeration of the names of all cells whose values must
    ///         be recalculated, assuming that the contents of the cell referred
    ///         to by name has changed.  The cell names are enumerated in an order
    ///         in which the calculations should be done.
    ///     </para>
    ///     
    ///     <exception cref="CircularException">
    ///         If the cell referred to by name is involved in a circular dependency,
    ///         throws a CircularException.
    ///     </exception>
    ///     
    ///     <para>
    ///         For example, suppose that:
    ///     </para>
    ///     <list type="number">
    ///         <item>
    ///             A1 contains 5
    ///         </item>
    ///         <item>
    ///             B1 contains the formula A1 + 2.
    ///         </item>
    ///         <item>
    ///             C1 contains the formula A1 + B1.
    ///         </item>
    ///         <item>
    ///             D1 contains the formula A1 * 7.
    ///         </item>
    ///         <item>
    ///             E1 contains 15
    ///         </item>
    ///     </list>
    ///     <para>
    ///         If A1 has changed, then A1, B1, C1, and D1 must be recalculated,
    ///         and they must be recalculated in an order which has A1 first, and B1 before C1
    ///         (there are multiple such valid orders).
    ///         The method will produce one of those enumerations.
    ///     </para>
    ///     <para>
    ///         PLEASE NOTE THAT THIS METHOD DEPENDS ON THE METHOD GetDirectDependents.
    ///         IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
    ///     </para>
    ///     
    /// </summary>
    /// 
    /// <param name="name"> The name of the cell. Requires that name be a valid cell name. </param>
    /// 
    /// <returns>
    ///     Returns an enumeration of the names of all cells whose values must
    ///     be recalculated.
    /// </returns>
    private IEnumerable<string> GetCellsToRecalculate(string name)
    {
        // Initialize a linked list to store the names of cells that need to be recalculated
        LinkedList<string> changed = new();

        // Initialize a hash set to keep track of visited cells and avoid processing the same cell multiple times
        HashSet<string> visited = [];

        // Call the Visit method to perform a depth-first traversal starting from the cell 'name'
        // This will add all cells that need recalculating to the 'changed' list in the correct order
        Visit(name, name, visited, changed);

        // Return the ordered list of cells that need recalculating
        return changed;
    }

    /// <summary>
    ///     A helper method for the GetCellsToRecalculate method. 
    ///     Recursively traverses the dependency graph to find all cells that directly or indirectly depend on the given cell.
    /// </summary>
    /// 
    /// <param name="start"> The original cell that initiated the recalculation process. </param>
    /// <param name="name"> The current cell being visited in the traversal. </param>
    /// <param name="visited"> A set of cells that have already been visited to prevent redundant processing. </param>
    /// <param name="changed"> A linked list of cells that need to be recalculated in the correct order. </param>
    private void Visit(string start, string name, ISet<string> visited, LinkedList<string> changed)
    {
        // Mark the current cell as visited
        visited.Add(name);

        // Iterate over all direct dependents of the current cell
        foreach (string n in GetDirectDependents(name))
        {
            // Check for a circular dependency: If the dependent cell is the original start cell, throw an exception
            if (n.Equals(start))
            {
                throw new CircularException();
            }

            // If the cell hasn't been visited yet, recursively visit it
            else if (!visited.Contains(n))
            {
                Visit(start, n, visited, changed);
            }
        }

        // Add the current cell to the front of the 'changed' list, ensuring that it is processed after its dependents
        changed.AddFirst(name);
    }

    /// <summary>
    ///   <para>
    ///     Return the value of the named cell.
    ///   </para>
    /// </summary>
    /// <param name="name"> The cell in question. </param>
    /// <returns>
    ///   Returns the value (as opposed to the contents) of the named cell.  The return
    ///   value should be either a string, a double, or a CS3500.Formula.FormulaError.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object GetCellValue(string name)
    {
        ValidateCellName(name);
        name = name.ToUpper();

        // If the cell exists, get its contents
        if (Cells.ContainsKey(name))
        {
            object content = Cells[name].Contents;

            // If the content is a double, return the double value
            if (content is double d)
            {
                return d;
            }

            // If the content is a string, return the string value
            else if (content is string s)
            {
                return s;
            }

            // If the content is a formula, evaluate it
            else
            {
                Formula f = (Formula)content;

                // Evaluate the formula using the Lookup function
                object result = f.Evaluate(Lookup);

                // Return the result (either a double or a FormulaError)
                return result;
            }
        }

        // If the cell doesn't exist or is empty, return an empty string
        return "";
    }

    /// <summary>
    ///     Retrieves the numeric value of the specified cell.
    /// </summary>
    /// 
    /// <param name="name">
    ///     The name of the cell whose value is being looked up.
    /// </param>
    /// 
    /// <returns>
    ///     The numeric (double) value of the specified cell.
    /// </returns>
    /// 
    /// <exception cref="ArgumentException">
    ///     Thrown if the specified cell does not contain a numeric value (double).
    /// </exception>
    private double Lookup(string name)
    {
        // Get the cell value using GetCellValue
        object value = GetCellValue(name);

        // If the value is a double, return it
        if (value is double)
        {
            return (double)value;
        }

        // If the value is not numeric, throw an exception
        throw new ArgumentException($"Cell {name} does not contain a numeric value.");
    }

    /// <summary>
    ///     <para>
    ///         Return the value of the named cell, as defined by
    ///         <see cref="GetCellValue(string)"/>.
    ///     </para>
    /// </summary>
    /// 
    /// <param name="name"> The cell in question. </param>
    /// 
    /// <returns>
    ///     <see cref="GetCellValue(string)"/>
    /// </returns>
    /// 
    /// <exception cref="InvalidNameException">
    ///     If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object this[string name]
    {
        get { return GetCellValue(name); }
    }

    /// <summary>
    ///     Constructs a spreadsheet using the saved data in the file refered to by
    ///     the given filename. 
    ///     <see cref="Save(string)"/>
    /// </summary>
    /// 
    /// <exception cref="SpreadsheetReadWriteException">
    ///     Thrown if the file can not be loaded into a spreadsheet for any reason
    /// </exception>
    /// 
    /// <param name="filename">The path to the file containing the spreadsheet to load</param>
    public Spreadsheet(string filename)
    {
        Cells = new Dictionary<string, Cell>();

        try
        {
            // Read the JSON file
            string jsonData = File.ReadAllText(filename);

            // Deserialize the JSON data into a dictionary containing the "Cells" object
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(jsonData);

            // Check if the deserialized object contains the "Cells" key
            if (jsonObject != null)
            {
                var cells = jsonObject["Cells"];

                // Populate the spreadsheet with the deserialized cells
                foreach (var cellEntry in cells)
                {
                    var cellName = cellEntry.Key;

                    // Validate cell name
                    try
                    {
                        ValidateCellName(cellName);
                    }
                    catch (Exception)
                    {
                        throw new SpreadsheetReadWriteException($"Invalid cell name: {cellName}");
                    }

                    var stringForm = cellEntry.Value["StringForm"];

                    // If contents is a Formula
                    if (stringForm.StartsWith("="))
                    {
                        try
                        {
                            SetCellContents(cellName, new Formula(stringForm.Substring(1)));
                        }
                        catch (FormulaFormatException)
                        {
                            throw new SpreadsheetReadWriteException($"Invalid formula in cell {cellName}");
                        }
                        catch (CircularException)
                        {
                            throw new SpreadsheetReadWriteException($"Circular dependency detected in cell {cellName}");
                        }
                    }

                    // If contents is a double
                    else if (double.TryParse(stringForm, out double number))
                    {
                        SetCellContents(cellName, number);
                    }

                    // If contents is a string
                    else
                    {
                        SetCellContents(cellName, stringForm);
                    }
                }
            }
        }
        catch (FileNotFoundException)
        {
            throw new SpreadsheetReadWriteException("File not found: " + filename);
        }
        catch (Exception)
        {
            throw new SpreadsheetReadWriteException("Error reading spreadsheet from file: " + filename);
        }
    }

    /// <summary>
    ///     <para>
    ///         Writes the contents of this spreadsheet to the named file using a JSON format.
    ///         If the file already exists, overwrite it.
    ///     </para>
    ///     <para>
    ///         The output JSON should look like the following.
    ///     </para>
    ///     <para>
    ///         For example, consider a spreadsheet that contains a cell "A1" 
    ///         with contents being the double 5.0, and a cell "B3" with contents 
    ///         being the Formula("A1+2"), and a cell "C4" with the contents "hello".
    ///     </para>
    ///   <para>
    ///      This method would produce the following JSON string:
    ///   </para>
    ///   <code>
    ///   {
    ///     "Cells": {
    ///       "A1": {
    ///         "StringForm": "5"
    ///       },
    ///       "B3": {
    ///         "StringForm": "=A1+2"
    ///       },
    ///       "C4": {
    ///         "StringForm": "hello"
    ///       }
    ///     }
    ///   }
    ///   </code>
    ///   <para>
    ///     You can achieve this by making sure your data structure is a dictionary 
    ///     and that the contained objects (Cells) have property named "StringForm"
    ///     (if this name does not match your existing code, use the JsonPropertyName 
    ///     attribute).
    ///   </para>
    ///   <para>
    ///     There can be 0 cells in the dictionary, resulting in { "Cells" : {} } 
    ///   </para>
    ///   <para>
    ///     Further, when writing the value of each cell...
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///       If the contents is a string, the value of StringForm is that string
    ///     </item>
    ///     <item>
    ///       If the contents is a double d, the value of StringForm is d.ToString()
    ///     </item>
    ///     <item>
    ///       If the contents is a Formula f, the value of StringForm is "=" + f.ToString()
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="filename"> The name (with path) of the file to save to.</param>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   If there are any problems opening, writing, or closing the file, 
    ///   the method should throw a SpreadsheetReadWriteException with an
    ///   explanatory message.
    /// </exception>
    public void Save(string filename)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        try
        {
            // Create a dictionary to store the cell data in the correct format
            var cellsDictionary = new Dictionary<string, Dictionary<string, string>>();

            // Iterate over all non-empty cells in the spreadsheet
            foreach (var cellName in GetNamesOfAllNonemptyCells())
            {
                // Get the cell contents
                var cellContents = GetCellContents(cellName);

                // Generate the string form of the content
                string stringForm = GenerateStringForm(cellContents);

                // Add the cell to the dictionary
                cellsDictionary[cellName] = new Dictionary<string, string>
                {
                    {"StringForm", stringForm}
                };
            }

            // Create the final dictionary with "Cells" as the root key
            var jsonObject = new Dictionary<string, object>
            {
                {"Cells", cellsDictionary}
            };

            // Serialize the dictionary to JSON
            string json = JsonSerializer.Serialize(jsonObject, options);

            // Write the JSON to the specified file
            File.WriteAllText(filename, json);
        }
        catch (Exception)
        {
            // If any error occurs during save, throw a custom exception
            throw new SpreadsheetReadWriteException("Error saving spreadsheet to file.");
        }

        // After saving, set Changed to false
        Changed = false;
    }

    /// <summary>
    ///     Converts the content of a cell into its string representation.
    /// </summary>
    /// 
    /// <param name="cellContents">
    ///     The contents of the cell. Can be a string, double, or Formula.
    /// </param>
    /// 
    /// <returns>
    ///     A string representation of the cell contents:
    ///     - If the content is a string, the original string is returned.
    ///     - If the content is a double, the number is returned as a string.
    ///     - If the content is a Formula, the formula string is prefixed with an "=".
    /// </returns>
    private string GenerateStringForm(object cellContents)
    {
        // if the content is a string
        if (cellContents is string s)
        {
            return s;
        }

        // If the content is a double
        else if (cellContents is double d)
        {
            return d.ToString();
        }

        // If the content is a Formula
        else
        {
            Formula f = (Formula)cellContents;
            return "=" + f.ToString();
        }
    }
}