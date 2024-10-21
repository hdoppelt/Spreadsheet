/// <copyright file="Formula_PS2.cs" company="UofU-CS3500">
///     Copyright (c) 2024 UofU-CS3500. All rights reserved.
/// </copyright>
/// <summary> 
///     <para>
///         This code is provides to start your assignment. It was written
///         by Profs Joe, Danny, and Jim. You should keep this attribution
///         at the top of your code where you have your header comment, along
///         with the other required information.
///     </para>
///     <para>
///         You should remove/add/adjust comments in your file as appropriate
///         to represent your work and any changes you make.
///     </para>
/// </summary>
/// 
/// Name: Harrison Doppelt
/// Date: 09/27/2024

namespace CS3500.Formula;

using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
///   <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one ore more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
///   </para>
///   <para>
///     Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
///     a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
///     and "x 23" consists of a variable "x" and a number "23".  Otherwise, spaces are to be removed.
///   </para>
///   <para>
///     For Assignment Two, you are to implement the following functionality:
///   </para>
///   <list type="bullet">
///     <item>
///        Formula Constructor which checks the syntax of a formula.
///     </item>
///     <item>
///        Get Variables
///     </item>
///     <item>
///        ToString
///     </item>
///   </list>
/// </summary>
public class Formula
{

    /// <summary>
    ///     Regular expression pattern to match valid variables in the formula.
    ///     Variables consist of one or more letters followed by one or more digits.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    /// <summary>
    ///     Regular expression pattern to match valid numbers in the formula, 
    ///     including decimal and scientific notation.
    /// </summary>
    private const string NumberRegExPattern = @"^\d*(\.\d+)?([eE][+-]?\d+)?$";

    /// <summary>
    ///     Regular expression pattern to match valid operators (+, -, *, /).
    /// </summary>
    private const string OperatorRegExPattern = @"(?<!\d)[\+\-*/](?!\d)";

    /// <summary>
    ///     List of tokens that represent the components of the formula.
    /// </summary>
    private List<string> tokens;

    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message.  See the assignment
    ///     specifications for the syntax rules you are to implement.
    ///   </para>
    ///   <para>
    ///     Non Exhaustive Example Errors:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///        Invalid variable name, e.g., x, x1x  (Note: x1 is valid, but would be normalized to X1)
    ///     </item>
    ///     <item>
    ///        Empty formula, e.g., string.Empty
    ///     </item>
    ///     <item>
    ///        Mismatched Parentheses, e.g., "(("
    ///     </item>
    ///     <item>
    ///        Invalid Following Rule, e.g., "2x+5"
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula(string formula)
    {

        // Split the formula string into individual tokens
        tokens = GetTokens(formula);

        // Check if the formula contains at least one token
        OneTokenRule(tokens);

        // Ensure all tokens are valid (variables, numbers, operators, or parentheses)
        ValidTokenRule(tokens);

        // Validate that parentheses are balanced (matching open and close)
        ParenthesesRule(tokens);

        // Ensure the first token is valid (must be a number, variable, or opening parenthesis)
        FirstTokenRule(tokens);

        // Ensure the last token is valid (must be a number, variable, or closing parenthesis)
        LastTokenRule(tokens);

        // Ensure that a valid token follows parentheses or operators
        ParenthesisOperatorFollowingRule(tokens);

        // Ensure that a valid operator or closing parenthesis follows numbers, variables, or closing parentheses
        ExtraFollowingRule(tokens);

    }

    /// <summary>
    ///     Checks if the formula contains at least one token.
    /// </summary>
    /// <param name="tokens">A list of tokens representing parts of the formula.</param>
    /// <exception cref="FormulaFormatException">
    ///     Thrown when the token list is empty, indicating that the formula does not contain any valid tokens.
    /// </exception>
    private void OneTokenRule(List<string> tokens)
    {

        // If the token list is empty
        if (tokens.Count == 0)
        {
            throw new FormulaFormatException("Formula must contain at least one token.");
        }

    }

    /// <summary>
    ///     Validates each token in the formula to ensure it is either a valid variable, number, operator, or parenthesis.
    /// </summary>
    /// <param name="tokens">A list of tokens representing parts of the formula.</param>
    /// <exception cref="FormulaFormatException">
    ///     Thrown when a token is found that is not a valid variable, number, operator, or parenthesis.
    /// </exception>
    private void ValidTokenRule(List<string> tokens)
    {

        // Regular expression pattern for recognizing parentheses (either '(' or ')')
        string ParenthesisRegExPattern = @"[\(\)]";

        // Loop through each token in the list
        foreach (var token in tokens)
        {

            // Check if the token is a valid variable, number, operator, or parenthesis
            bool isValidVariable = IsVar(token);
            bool isValidNumber = Regex.IsMatch(token, NumberRegExPattern);
            bool isValidOperator = Regex.IsMatch(token, OperatorRegExPattern);
            bool isValidParenthesis = Regex.IsMatch(token, ParenthesisRegExPattern);

            // If the token is none of the above (invalid), throw an exception
            if (!isValidVariable && !isValidNumber && !isValidOperator && !isValidParenthesis)
            {
                throw new FormulaFormatException($"Invalid token: {token}");
            }

        }

    }

    /// <summary>
    ///     Validates that the number of opening and closing parentheses in the formula are balanced.
    /// </summary>
    /// <param name="tokens">A list of tokens representing parts of the formula.</param>
    /// <exception cref="FormulaFormatException">
    ///     Thrown when there are unmatched closing parentheses or if some opening parentheses are not closed.
    /// </exception>
    private void ParenthesesRule(List<string> tokens)
    {

        // Counter to track the number of unbalanced opening parentheses
        int openParenthesesCount = 0;

        // Iterate through each token in the formula
        foreach (var token in tokens)
        {

            // If the token is an opening parenthesis, increment the counter
            if (token == "(")
            {
                openParenthesesCount++;
            }

            // If the token is a closing parenthesis, decrement the counter
            else if (token == ")")
            {

                openParenthesesCount--;

                // If there are more closing parentheses than opening, throw an exception
                if (openParenthesesCount < 0)
                {
                    throw new FormulaFormatException("Closing parenthesis without matching opening parenthesis.");
                }

            }

        }

        // If there are unmatched opening parentheses left, throw an exception
        if (openParenthesesCount != 0)
        {
            throw new FormulaFormatException("Mismatched parentheses: Some opening parentheses are not closed.");
        }

    }

    /// <summary>
    ///     Ensures that the first token in the formula is either a number, variable, or an opening parenthesis.
    /// </summary>
    /// <param name="tokens">A list of tokens representing parts of the formula.</param>
    /// <exception cref="FormulaFormatException">
    ///     Thrown when the first token is not a number, variable, or an opening parenthesis.
    /// </exception>
    private void FirstTokenRule(List<string> tokens)
    {

        // Get the first token from the token list
        string firstToken = tokens[0];

        // If the first token is not a variable, a number, or an opening parenthesis, throw an exception
        if (!IsVar(firstToken) && !Regex.IsMatch(firstToken, NumberRegExPattern) && firstToken != "(")
        {
            throw new FormulaFormatException("The first token must be a number, a variable, or an opening parenthesis.");
        }

    }

    /// <summary>
    ///     Ensures that the last token in the formula is either a number, variable, or a closing parenthesis.
    /// </summary>
    /// <param name="tokens">A list of tokens representing parts of the formula.</param>
    /// <exception cref="FormulaFormatException">
    ///     Thrown when the last token is not a number, variable, or a closing parenthesis.
    /// </exception>
    private void LastTokenRule(List<string> tokens)
    {

        // Get the last token from the token list
        string lastToken = tokens[tokens.Count - 1];

        // If the last token is not a variable, a number, or a closing parenthesis, throw an exception
        if (!IsVar(lastToken) && !Regex.IsMatch(lastToken, NumberRegExPattern) && lastToken != ")")
        {
            throw new FormulaFormatException("The last token must be a number, a variable, or a closing parenthesis.");
        }

    }

    /// <summary>
    ///     Ensures that any token following an opening parenthesis or an operator 
    ///     is either a number, variable, or another opening parenthesis.
    /// </summary>
    /// <param name="tokens">A list of tokens representing parts of the formula.</param>
    /// <exception cref="FormulaFormatException">
    ///     Thrown when a token following an opening parenthesis or an operator 
    ///     is not a valid number, variable, or an opening parenthesis.
    /// </exception>
    private void ParenthesisOperatorFollowingRule(List<string> tokens)
    {

        int i = 0;

        // Loop through the token list until the second-to-last token
        while (i < tokens.Count - 1)
        {

            // Get the current token and the next token in the list
            string currentToken = tokens[i];
            string nextToken = tokens[i + 1];

            // If the current token is either an opening parenthesis or an operator
            if (currentToken == "(" || Regex.IsMatch(currentToken, OperatorRegExPattern))
            {

                // If the next token is not a variable, a number, or an opening parenthesis, throw an exception
                if (!IsVar(nextToken) && !Regex.IsMatch(nextToken, NumberRegExPattern) && nextToken != "(")
                {
                    throw new FormulaFormatException($"Invalid token following '{currentToken}': {nextToken}");
                }

            }

            // Move to the next token in the list
            i++;
        }

    }

    /// <summary>
    ///     Ensures that any token following a number, variable, or closing parenthesis 
    ///     is either an operator or a closing parenthesis.
    /// </summary>
    /// <param name="tokens">A list of tokens representing parts of the formula.</param>
    /// <exception cref="FormulaFormatException">
    ///     Thrown when a token following a number, variable, or closing parenthesis 
    ///     is not an operator or a closing parenthesis.
    /// </exception>
    private void ExtraFollowingRule(List<string> tokens)
    {

        int i = 0;

        // Compile regex patterns once for better performance
        Regex numberPattern = new Regex(NumberRegExPattern);
        Regex operatorPattern = new Regex(OperatorRegExPattern);

        // Loop through the token list until the second-to-last token
        while (i < tokens.Count - 1)
        {

            // Get the current token and the next token in the list
            string currentToken = tokens[i];
            string nextToken = tokens[i + 1];

            // Check if currentToken is a number, variable, or closing parenthesis
            bool isNumberOrVarOrClosingParenthesis = numberPattern.IsMatch(currentToken) || IsVar(currentToken) || currentToken == ")";

            // If the current token is a number, variable, or closing parenthesis
            if (isNumberOrVarOrClosingParenthesis)
            {
                // If the next token is not an operator or closing parenthesis, throw an exception
                if (!operatorPattern.IsMatch(nextToken) && nextToken != ")")
                {
                    throw new FormulaFormatException($"Invalid token following '{currentToken}': {nextToken}");
                }
            }

            // Move to the next token in the list
            i++;
        }

    }

    /// <summary>
    ///   <para>
    ///     Returns a set of all the variables in the formula.
    ///   </para>
    ///   <remarks>
    ///     Important: no variable may appear more than once in the returned set, even
    ///     if it is used more than once in the Formula.
    ///   </remarks>
    ///   <list type="bullet">
    ///     <item>new("x1+y1*z1").GetVariables() should return a set containing "X1", "Y1", and "Z1".</item>
    ///     <item>new("x1+X1"   ).GetVariables() should return a set containing "X1".</item>
    ///   </list>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables()
    {

        // Create a HashSet to store the unique variables (case-insensitive)
        HashSet<string> variables = new HashSet<string>();

        // Iterate through each token in the formula
        foreach (var token in tokens)
        {

            // If the token is a valid variable
            if (IsVar(token))
            {

                // Convert the variable to uppercase and add it to the set
                // HashSet ensures no duplicates are added
                variables.Add(token.ToUpper());

            }

        }

        // Return the set of variables
        return variables;
    }

    /// <summary>
    ///   <para>
    ///     Returns a string representation of a canonical form of the formula.
    ///   </para>
    ///   <para>
    ///     The string will contain no spaces.
    ///   </para>
    ///   <para>
    ///     If the string is passed to the Formula constructor, the new Formula f 
    ///     will be such that this.ToString() == f.ToString().
    ///   </para>
    ///   <para>
    ///     All of the variables in the string will be normalized.  This
    ///     means capital letters.
    ///   </para>
    ///   <para>
    ///       For example:
    ///   </para>
    ///   <code>
    ///       new("x1 + y1").ToString() should return "X1+Y1"
    ///       new("X1 + 5.0000").ToString() should return "X1+5".
    ///   </code>
    ///   <para>
    ///     This code should execute in O(1) time.
    ///   <para>
    /// </summary>
    /// <returns>
    ///   A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns>
    public override string ToString()
    {

        // Initialize a StringBuilder to build the result string efficiently
        StringBuilder result = new StringBuilder();

        // Loop through each token in the formula
        foreach (var token in tokens)
        {

            // If the token is a variable
            if (IsVar(token))
            {

                // Convert it to uppercase and append to result
                result.Append(token.ToUpper());

            }

            // If the token is a number
            else if (IsNumber(token))
            {

                // Normalize the number format
                double number = double.Parse(token);

                // Append to result
                result.Append(number.ToString("G"));

            }

            // For any other token (Operators or Parentheses), append it directly
            else
            {
                result.Append(token);
            }

        }

        // Return the final canonical form of the formula as a string
        return result.ToString();
    }

    /// <summary>
    ///     Determines whether the given token is a valid number, which could be in decimal or scientific notation.
    /// </summary>
    /// <param name="token">The token to be checked.</param>
    /// <returns>
    ///     True if the token is a valid number (decimal or scientific notation); otherwise, false.
    /// </returns>
    private bool IsNumber(string token)
    {

        // Use double.TryParse to determine if the token can be parsed as a double (valid number)
        // The 'out _' discards the parsed value as it's not needed, only checking validity
        return double.TryParse(token, out _);

    }

    /// <summary>
    ///   Reports whether "token" is a variable.  It must be one or more letters
    ///   followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1. </returns>
    public static bool IsVar(string token)
    {

        // Create a regular expression pattern to match a valid variable (one or more letters followed by numbers)
        // The pattern ensures that the entire string matches this format
        string standaloneVarPattern = $"^{VariableRegExPattern}$";

        // Use Regex.IsMatch to check if the token matches the variable pattern
        return Regex.IsMatch(token, standaloneVarPattern);

    }

    /// <summary>
    ///   <para>
    ///     Given an expression, enumerates the tokens that compose it.
    ///   </para>
    ///   <para>
    ///     Tokens returned are:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>left paren</item>
    ///     <item>right paren</item>
    ///     <item>one of the four operator symbols</item>
    ///     <item>a string consisting of one or more letters followed by one or more numbers</item>
    ///     <item>a double literal</item>
    ///     <item>and anything that doesn't match one of the above patterns</item>
    ///   </list>
    ///   <para>
    ///     There are no empty tokens; white space is ignored (except to separate other tokens).
    ///   </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens(string formula)
    {
        List<string> results = [];

        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        string pattern = string.Format(
                                        "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern,
                                        rpPattern,
                                        opPattern,
                                        VariableRegExPattern,
                                        doublePattern,
                                        spacePattern);

        foreach (string s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                results.Add(s);
            }
        }

        return results;
    }

    /// <summary>
    ///   <para>
    ///     Determines if two formula objects represent the same formula.
    ///   </para>
    ///   <para>
    ///     By definition, if the parameter is null or does not reference 
    ///     a Formula Object then return false.
    ///   </para>
    ///   <para>
    ///     Two Formulas are considered equal if their canonical string representations
    ///     (as defined by ToString) are equal.  
    ///   </para>
    /// </summary>
    /// <param name="obj"> The other object.</param>
    /// <returns>
    ///   True if the two objects represent the same formula.
    /// </returns>
    public override bool Equals(object? obj)
    {

        // If obj is a Formula, cast and compare their canonical string representations
        if (obj is Formula otherFormula)
        {

            // Compare the canonical string representations
            return this.ToString() == otherFormula.ToString();

        }

        // If it's not a Formula object
        return false;

    }

    /// <summary>
    ///   <para>
    ///     Reports whether f1 == f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are the same.</returns>
    public static bool operator ==(Formula f1, Formula f2)
    {

        // Use the Equals method to check for equality
        return Equals(f1, f2);

    }

    /// <summary>
    ///   <para>
    ///     Reports whether f1 != f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are not equal to each other.</returns>
    public static bool operator !=(Formula f1, Formula f2)
    {

        // Invert result of equality operator
        return !(f1 == f2);

    }

    /// <summary>
    ///   <para>
    ///     Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
    ///     case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
    ///     randomly-generated unequal Formulas have the same hash code should be extremely small.
    ///   </para>
    /// </summary>
    /// <returns> The hashcode for the object. </returns>
    public override int GetHashCode()
    {

        // Use the hash code of the canonical string representation
        return this.ToString().GetHashCode();

    }

    /// <summary>
    ///   <para>
    ///     Evaluates this Formula, using the lookup delegate to determine the values of
    ///     variables.
    ///   </para>
    ///   <remarks>
    ///     When the lookup method is called, it will always be passed a normalized (capitalized)
    ///     variable name.  The lookup method will throw an ArgumentException if there is
    ///     not a definition for that variable token.
    ///   </remarks>
    ///   <para>
    ///     If no undefined variables or divisions by zero are encountered when evaluating
    ///     this Formula, the numeric value of the formula is returned.  Otherwise, a 
    ///     FormulaError is returned (with a meaningful explanation as the Reason property).
    ///   </para>
    ///   <para>
    ///     This method should never throw an exception.
    ///   </para>
    /// </summary>
    /// <param name="lookup">
    ///   <para>
    ///     Given a variable symbol as its parameter, lookup returns the variable's value
    ///     (if it has one) or throws an ArgumentException (otherwise).  This method will expect 
    ///     variable names to be normalized.
    ///   </para>
    /// </param>
    /// <returns> Either a double or a FormulaError, based on evaluating the formula.</returns>
    public object Evaluate(Lookup lookup)
    {
        // Initialize stack for values
        Stack<double> valueStack = new Stack<double>();

        // Initialize stack for operators
        Stack<string> operatorStack = new Stack<string>();

        // Initialize double to hold result of computation
        double result;

        // Loop through each token in the list
        foreach (string token in tokens)
        {
            // If token is a number
            if (IsNumber(token))
            {
                // If '*' or '/' is at the top of the operator stack
                if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                {
                    // Pop value and operator stacks
                    double popNumber = valueStack.Pop();
                    string popOperator = operatorStack.Pop();

                    // If operator is '*'
                    if (popOperator == "*")
                    {
                        // Apply popped operator '*' to popped number and token
                        result = popNumber * double.Parse(token);
                    }
                    // Operator is '/'
                    else
                    {
                        // Handle divide by zero error
                        if (double.Parse(token) == 0)
                        {
                            return new FormulaError("Division by zero");
                        }
                        // Apply popped operator '/' to popped number and token
                        result = popNumber / double.Parse(token);
                    }
                    // Push result onto the value stack
                    valueStack.Push(result);
                }
                // Otherwise, push token onto the value stack
                else
                {
                    valueStack.Push(double.Parse(token));
                }
            }

            // If token is a variable
            else if (IsVar(token))
            {
                // double to hold variable value
                double variableValue;

                // Lookup variable value
                try
                {
                    variableValue = lookup(token.ToUpper());
                }
                // Catch exception if variable has no value
                catch (ArgumentException)
                {
                    return new FormulaError($"Unknown variable: {token}");
                }

                // If '*' or '/' is at the top of the operator stack
                if (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                {
                    // Pop value and operator stacks
                    double popNumber = valueStack.Pop();
                    string popOperator = operatorStack.Pop();

                    // If operator is '*'
                    if (popOperator == "*")
                    {
                        // Apply popped operator '*' to popped number and variable
                        result = popNumber * variableValue;
                    }
                    // Operator is '/'
                    else
                    {
                        // Handle divide by zero error
                        if (variableValue == 0)
                        {
                            return new FormulaError("Division by zero");
                        }
                        // Apply popped operator '/' to popped number and variable
                        result = popNumber / variableValue;
                    }
                    // Push result onto the value stack
                    valueStack.Push(result);
                }
                // Otherwise, push variable onto the value stack
                else
                {
                    valueStack.Push(variableValue);
                }
            }

            // If token is '+' or '-'
            else if (token == "+" || token == "-")
            {
                // If '+' or '-' is at the top of the operator stack
                while (operatorStack.Count > 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                {
                    // Pop value stack twice
                    double rightOperand = valueStack.Pop();
                    double leftOperand = valueStack.Pop();

                    // Pop operator stack once
                    string popOperator = operatorStack.Pop();

                    // Apply popped operator to the popped numbers
                    result = popOperator == "+" ? leftOperand + rightOperand : leftOperand - rightOperand;

                    // Push result onto the value stack
                    valueStack.Push(result);
                }
                // Push token onto the operator stack
                operatorStack.Push(token);
            }

            // If token is '*' or '/'
            else if (token == "*" || token == "/")
            {
                // Push token onto the operator stack
                operatorStack.Push(token);
            }

            // If token is '('
            else if (token == "(")
            {
                // Push token onto the operator stack
                operatorStack.Push(token);
            }

            // If token is ')'
            else if (token == ")")
            {
                // Process until we find a matching '('
                while (operatorStack.Peek() != "(")
                {
                    // Pop value stack twice
                    double rightOperand = valueStack.Pop();
                    double leftOperand = valueStack.Pop();

                    // Pop operator stack once
                    string popOperator = operatorStack.Pop();

                    // Apply popped operator to the popped numbers
                    result = popOperator == "+" ? leftOperand + rightOperand : leftOperand - rightOperand;

                    // Push result onto the value stack
                    valueStack.Push(result);
                }

                // Pop the '(' from the operator stack
                operatorStack.Pop();

                // If '*' or '/' is at the top of the operator stack
                while (operatorStack.Count > 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                {
                    // Pop value stack twice
                    double rightOperand = valueStack.Pop();
                    double leftOperand = valueStack.Pop();

                    // Pop operator stack once
                    string popOperator = operatorStack.Pop();

                    // If operator is '*'
                    if (popOperator == "*")
                    {
                        // Apply popped operator '*' to popped numbers
                        result = leftOperand * rightOperand;
                    }
                    // If operator is '/'
                    else
                    {
                        // Handle divide by zero error
                        if (rightOperand == 0)
                        {
                            return new FormulaError("Division by zero");
                        }
                        // Apply popped operator '/' to popped numbers
                        result = leftOperand / rightOperand;
                    }
                    // Push result onto the value stack
                    valueStack.Push(result);
                }
            }

        }

        // While operator stack is not empty
        while (operatorStack.Count > 0)
        {

            // One operator left on the operator stack ('+' or '-')
            string popOperator = operatorStack.Pop();

            // Two values on the value stack
            double rightOperand = valueStack.Pop();
            double leftOperand = valueStack.Pop();

            // Apply operator to the two values
            result = popOperator == "+" ? leftOperand + rightOperand : leftOperand - rightOperand;

            // Push result to valuestack
            valueStack.Push(result);

            // Return result as the value of the expression
            return valueStack.Pop();
        }
        
        // Operator stack is empty
        // Value stack contains a single number
        // Pop it and return it as the value of the expression
        return valueStack.Pop();
    }

}

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public class FormulaError
{

    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaError"/> class.
    ///   <para>
    ///     Constructs a FormulaError containing the explanatory reason.
    ///   </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error occurred.</param>
    public FormulaError(string message)
    {
        Reason = message;
    }

    /// <summary>
    ///  Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }

}

/// <summary>
///   Any method meeting this type signature can be used for
///   looking up the value of a variable.
/// </summary>
/// <exception cref="ArgumentException">
///   If a variable name is provided that is not recognized by the implementing method,
///   then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
///   The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup(string variableName);

/// <summary>
///   Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{

    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    ///   <para>
    ///      Constructs a FormulaFormatException containing the explanatory message.
    ///   </para>
    /// </summary>
    /// <param name="message"> A developer defined message describing why the exception occured.</param>
    public FormulaFormatException(string message)
        : base(message)
    {
        // All this does is call the base constructor. No extra code needed.
    }

}