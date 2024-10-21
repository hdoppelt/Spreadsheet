/// <summary> 
///     <para>
///         Test Class for Formula Class
///     </para>
/// </summary>
/// 
/// Name: Harrison Doppelt
/// Date: 09/27/2024

namespace FormulaTests;

using CS3500.Formula;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

[TestClass]
public class FormulaSyntaxTests
{
    // --- Personal PS2 Tests ---

    // --- Tests for One Token Rule ---

    [TestMethod]
    public void FormulaConstructor_TestOneTokenNumber_Valid()
    {
        _ = new Formula("1");
    }

    [TestMethod]
    public void FormulaConstructor_TestOneTokenVariable_Valid()
    {
        _ = new Formula("a1");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOneTokenWhiteSpace_Invalid()
    {
        _ = new Formula(" ");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOneTokenEmpty_Invalid()
    {
        _ = new Formula("");
    }

    // --- Tests for Valid Token Rule ---

    [TestMethod]
    public void FormulaConstructor_TestValidTokenSingleNumber_Valid()
    {
        _ = new Formula("1");
    }

    [TestMethod]
    public void FormulaConstructor_TestValidTokenDoubleNumber_Valid()
    {
        _ = new Formula("11");
    }

    [TestMethod]
    public void FormulaConstructor_TestValidTokenTenthDecimal_Valid()
    {
        _ = new Formula("1.1");
    }

    [TestMethod]
    public void FormulaConstructor_TestValidTokenHundrethDecimal_Valid()
    {
        _ = new Formula("1.11");
    }

    [TestMethod]
    public void FormulaConstructor_TestValidTokenPositiveUppercaseExponent_Valid()
    {
        _ = new Formula("1E2");
    }

    [TestMethod]
    public void FormulaConstructor_TestValidTokenPositiveLowercaseExponent_Valid()
    {
        _ = new Formula("1e2");
    }

    [TestMethod]
    public void FormulaConstructor_TestValidTokenNegativeExponent_Valid()
    {
        _ = new Formula("1E-2");
    }

    [TestMethod]
    public void FormulaConstructor_TestValidTokenDecimalUppercaseExponent_Valid()
    {
        _ = new Formula("1.5E-2");
    }

    [TestMethod]
    public void FormulaConstructor_TestValidTokenDecimalLowercaseExponent_Valid()
    {
        _ = new Formula("1.5e-2");
    }

    [TestMethod]
    public void FormulaConstructor_TestValidTokenSingleVariable_Valid()
    {
        _ = new Formula("a1");
    }

    [TestMethod]
    public void FormulaConstructor_TestValidTokenDoubleVariable_Valid()
    {
        _ = new Formula("aa11");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestValidTokenVariable_Invalid()
    {
        _ = new Formula("1a");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestValidTokenSymbol_Invalid()
    {
        _ = new Formula("$");
    }

    // --- Tests for Closing Parentheses Rule ---

    [TestMethod]
    public void FormulaConstructor_TestClosingParenthesesRuleSingle_Valid()
    {
        _ = new Formula("(1)");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingParenthesesRuleCloseOpen_Invalid()
    {
        _ = new Formula(")(");
    }

    // --- Tests for Balenced Parentheses Rule ---

    [TestMethod]
    public void FormulaConstructor_TestBalencedParenthesesRuleDouble_Valid()
    {
        _ = new Formula("((1))");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestBalencedParenthesesRuleExtraClose_Invalid()
    {
        _ = new Formula("(()()))");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestBalencedParenthesesRuleExtraOpen_Invalid()
    {
        _ = new Formula("((()())");
    }

    // --- Tests for First Token Rule ---

    [TestMethod]
    public void FormulaConstructor_TestFirstTokenRuleNumber_Valid()
    {
        _ = new Formula("5 + a1");
    }

    [TestMethod]
    public void FormulaConstructor_TestFirstTokenRuleDecimal_Valid()
    {
        _ = new Formula("5.5 + a1");
    }

    [TestMethod]
    public void FormulaConstructor_TestFirstTokenRuleExponent_Valid()
    {
        _ = new Formula("5E2 + a1");
    }

    [TestMethod]
    public void FormulaConstructor_TestFirstTokenRuleVariable_Valid()
    {
        _ = new Formula("a1 + 5");
    }

    [TestMethod]
    public void FormulaConstructor_TestFirstTokenRuleOpeningParenthesis_Valid()
    {
        _ = new Formula("(a1 + 5)");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenRuleOperator_Invalid()
    {
        _ = new Formula("+ a1");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenRuleClosingParenthesis_Invalid()
    {
        _ = new Formula(") a1 + 5");
    }

    // --- Tests for Last Token Rule ---

    [TestMethod]
    public void FormulaConstructor_TestLastTokenRuleNumber_Valid()
    {
        _ = new Formula("a1 + 10");
    }

    [TestMethod]
    public void FormulaConstructor_TestLastTokenRuleDecimal_Valid()
    {
        _ = new Formula("a1 + 10.5");
    }

    [TestMethod]
    public void FormulaConstructor_TestLastTokenRuleExponent_Valid()
    {
        _ = new Formula("a1 + 10E5");
    }

    [TestMethod]
    public void FormulaConstructor_TestLastTokenRuleVariable_Valid()
    {
        _ = new Formula("10 + a1");
    }

    [TestMethod]
    public void FormulaConstructor_TestLastTokenRuleClosingParentheses_Valid()
    {
        _ = new Formula("(a1 + 5)");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenRuleOpeningParentheses_Invalid()
    {
        _ = new Formula("a1 + 5 (");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenRuleOperator_Invalid()
    {
        _ = new Formula("a1 + 5 +");
    }

    // --- Tests for Parenthesis/Operator Following Rule ---

    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowingRuleNumber_Valid()
    {
        _ = new Formula("1+1");
    }

    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowingRuleVariable_Valid()
    {
        _ = new Formula("1+a1");
    }

    [TestMethod]
    public void FormulaConstructor_TestOperatorFollowingRuleOpeningParenthesis_Valid()
    {
        _ = new Formula("1+(a1)");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOperatorFollowingRuleClosedParenthesis_Invalid()
    {
        _ = new Formula("(1+)");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOperatorFollowingRuleOperator_Invalid()
    {
        _ = new Formula("1++1");
    }

    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowingRuleNumber_Valid()
    {
        _ = new Formula("(1)");
    }

    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowingRuleVariable_Valid()
    {
        _ = new Formula("(a1)");
    }

    [TestMethod]
    public void FormulaConstructor_TestOpeningParenthesisFollowingRuleOpeningParenthesis_Valid()
    {
        _ = new Formula("((1))");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOpeningParenthesisFollowingRuleClosingParentheses_Invalid()
    {
        _ = new Formula("()");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOpeningParenthesisFollowingRuleOperator_Invalid()
    {
        _ = new Formula("(+1)");
    }

    // --- Tests for Extra Following Rule ---

    [TestMethod]
    public void FormulaConstructor_TestNumberExtraFollowingRuleOperator_Valid()
    {
        _ = new Formula("1.5+1");
    }

    [TestMethod]
    public void FormulaConstructor_TestNumberExtraFollowingRuleClosingParenthesis_Valid()
    {
        _ = new Formula("(5)");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNumberExtraFollowingRuleVariable_Invalid()
    {
        _ = new Formula("1a5");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNumberExtraFollowingRuleOpeningParenthesis_Invalid()
    {
        _ = new Formula("5(1+1)");
    }

    [TestMethod]
    public void FormulaConstructor_TestVariableExtraFollowingRuleOperator_Valid()
    {
        _ = new Formula("a1+a5");
    }

    [TestMethod]
    public void FormulaConstructor_TestVariableExtraFollowingRuleClosingParenthesis_Valid()
    {
        _ = new Formula("(a1)");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestVariableExtraFollowingRuleVariable_Invalid()
    {
        _ = new Formula("a1a1");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestVariableExtraFollowingRuleOpeningParenthesis_Invalid()
    {
        _ = new Formula("a1(1)");
    }

    [TestMethod]
    public void FormulaConstructor_TestClosingParenthesisExtraFollowingRuleOperator_Valid()
    {
        _ = new Formula("(1)+1");
    }

    [TestMethod]
    public void FormulaConstructor_TestClosingParenthesisExtraFollowingRuleClosingParenthesis_Valid()
    {
        _ = new Formula("((a1))");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingParenthesisExtraFollowingRuleVariable_Invalid()
    {
        _ = new Formula("(a1)a1");
    }

    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingParenthesisExtraFollowingRuleOpeningParenthesis_Invalid()
    {
        _ = new Formula("(a1)(a1)");
    }

    // --- Tests for ToString ---

    [TestMethod]
    public void ToString_TestVariable_Valid()
    {
        string formulaInput = "a1";
        Formula formula = new Formula(formulaInput);
        string result = formula.ToString();
        Assert.AreEqual("A1", result);
    }

    [TestMethod]
    public void ToString_TestSpaces_Valid()
    {
        string formulaInput = "a1 + a1";
        Formula formula = new Formula(formulaInput);
        string result = formula.ToString();
        Assert.AreEqual("A1+A1", result);
    }

    [TestMethod]
    public void ToString_TestNumber_Valid()
    {
        string formulaInput = "1";
        Formula formula = new Formula(formulaInput);
        string result = formula.ToString();
        Assert.AreEqual("1", result);
    }

    [TestMethod]
    public void ToString_TestDecimal_Valid()
    {
        string formulaInput = "1.000";
        Formula formula = new Formula(formulaInput);
        string result = formula.ToString();
        Assert.AreEqual("1", result);
    }

    [TestMethod]
    public void ToString_TestExponent_Valid()
    {
        string formulaInput = "1E2";
        Formula formula = new Formula(formulaInput);
        string result = formula.ToString();
        Assert.AreEqual("100", result);
    }

    // --- Tests for GetVariables ---

    [TestMethod]
    public void GetVariables_SingleVariable_Valid()
    {
        string formulaInput = "x1";
        Formula formula = new Formula(formulaInput);
        ISet<string> variables = formula.GetVariables();
        CollectionAssert.AreEquivalent(new List<string> { "X1" }, new List<string>(variables));
    }

    [TestMethod]
    public void GetVariables_DoubleVariable_Valid()
    {
        string formulaInput = "x1 + y1";
        Formula formula = new Formula(formulaInput);
        ISet<string> variables = formula.GetVariables();
        CollectionAssert.AreEquivalent(new List<string> { "X1", "Y1" }, new List<string>(variables));
    }

    [TestMethod]
    public void GetVariables_DuplicateVariable_Valid()
    {
        string formulaInput = "x1 + x1";
        Formula formula = new Formula(formulaInput);
        ISet<string> variables = formula.GetVariables();
        CollectionAssert.AreEquivalent(new List<string> { "X1" }, new List<string>(variables));
    }

    [TestMethod]
    public void GetVariables_IgnoresNumbers_Valid()
    {
        string formulaInput = "x1 + 5 + y1";
        Formula formula = new Formula(formulaInput);
        ISet<string> variables = formula.GetVariables();
        CollectionAssert.AreEquivalent(new List<string> { "X1", "Y1" }, new List<string>(variables));
    }










    // --- Teacher PS2 Tests ---

    // --- Tests One Token Rule ---

    /// <summary>
    ///   Test that an empty formula throws the formula format exception.
    /// </summary>
    [TestMethod]
    [TestCategory("1")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOneToken_Fails()
    {
        _ = new Formula(string.Empty);
    }

    /// <summary>
    ///   Test that an empty formula, but with spaces, also fails.
    /// </summary>
    [TestMethod]
    [TestCategory("2")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOneTokenSpaces_Fails()
    {
        _ = new Formula("  ");
    }

    // --- Test Valid Token Rules ---

    /// <summary>
    ///   Test that invalid tokens throw the appropriate exception.
    /// </summary>
    [TestMethod]
    [TestCategory("3")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidTokensOnly_Fails()
    {
        _ = new Formula("$");
    }

    /// <summary>
    ///   Test for another invalid token in the formula.
    /// </summary>
    [TestMethod]
    [TestCategory("4")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidTokenInFormula_Fails()
    {
        _ = new Formula("5 + 5 ,");
    }

    /// <summary>
    ///   Test that _all_ the valid tokens can be parsed,
    ///   e.g., math operators, numbers, variables, parens.
    /// </summary>
    [TestMethod]
    [TestCategory("5")]
    public void FormulaConstructor_TestValidTokenTypes_Succeeds()
    {
        _ = new Formula("5 + (1-2) * 3.14 / 1e6 + 0.2E-9 - A1 + bb22");
    }

    // --- Test Closing Parenthesis Rule ---

    /// <summary>
    ///   Test that a closing paren cannot occur without
    ///   an opening paren first.
    /// </summary>
    [TestMethod]
    [TestCategory("6")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingWithoutOpening_Fails()
    {
        _ = new Formula("5 )");
    }

    /// <summary>
    ///   Test that the number of closing parens cannot be larger than
    ///   the number of opening parens already seen.
    /// </summary>
    [TestMethod]
    [TestCategory("7")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingAfterBalanced_Fails()
    {
        _ = new Formula("(5 + 5))");
    }

    /// <summary>
    ///   Test that even when "balanced", the order of parens must be correct.
    /// </summary>
    [TestMethod]
    [TestCategory("8")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosingBeforeOpening_Fails()
    {
        _ = new Formula("5)(");
    }

    /// <summary>
    ///   Make sure multiple/nested parens that are correct, are accepted.
    /// </summary>
    [TestMethod]
    [TestCategory("9")]
    public void FormulaConstructor_TestValidComplexParens_Succeeds()
    {
        _ = new Formula("(5 + ((3+2) - 5 / 2))");
    }

    // --- Test Balanced Parentheses Rule ---

    /// <summary>
    ///   Make sure that an unbalanced parentheses set throws an exception.
    /// </summary>
    [TestMethod]
    [TestCategory("10")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestUnclosedParens_Fails()
    {
        _ = new Formula("(5 + 2");
    }

    /// <summary>
    ///   Test that multiple sets of balanced parens work properly.
    /// </summary>
    [TestMethod]
    [TestCategory("11")]
    public void FormulaConstructor_TestManyParens_Succeeds()
    {
        _ = new Formula("(1 + 2) - (1 + 2) - (1 + 2)");
    }

    /// <summary>
    ///   Test that lots of balanced nested parentheses are accepted.
    /// </summary>
    [TestMethod]
    [TestCategory("12")]
    public void FormulaConstructor_TestDeeplyNestedParens_Succeeds()
    {
        _ = new Formula("(((5)))");
    }

    // --- Test First Token Rule ---

    /// <summary>
    ///   The first token cannot be a closing paren.
    /// </summary>
    [TestMethod]
    [TestCategory("13")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidFirstTokenClosingParen_Fails()
    {
        _ = new Formula(")");
    }

    /// <summary>
    ///   Test that the first token cannot be a math operator (+).
    /// </summary>
    [TestMethod]
    [TestCategory("14")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidFirstTokenPlus_Fails()
    {
        _ = new Formula("+");
    }

    /// <summary>
    ///   Test that the first token cannot be a math operator (*).
    /// </summary>
    [TestMethod]
    [TestCategory("15")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidFirstTokenMultiply_Fails()
    {
        _ = new Formula("*");
    }

    /// <summary>
    ///   Test that an integer number can be a valid first token.
    /// </summary>
    [TestMethod]
    [TestCategory("16")]
    public void FormulaConstructor_TestValidFirstTokenInteger_Succeeds()
    {
        _ = new Formula("1");
    }

    /// <summary>
    ///   Test that a floating point number can be a valid first token.
    /// </summary>
    [TestMethod]
    [TestCategory("17")]
    public void FormulaConstructor_TestValidFirstTokenFloat_Succeeds()
    {
        _ = new Formula("1.0");
    }

    // --- Test Last Token Rule ---

    /// <summary>
    ///   Make sure the last token is valid, in this case, not an operator (plus).
    /// </summary>
    [TestMethod]
    [TestCategory("18")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidLastTokenPlus_Fails()
    {
        _ = new Formula("5 +");
    }

    /// <summary>
    ///   Make sure the last token is valid, in this case, not a closing paren.
    /// </summary>
    [TestMethod]
    [TestCategory("19")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidLastTokenClosingParen_Fails()
    {
        _ = new Formula("5 (");
    }

    // --- Test Parentheses/Operator Following Rule ---

    /// <summary>
    ///   Test that after an opening paren, there cannot be an invalid token, in this
    ///   case a math operator (+).
    /// </summary>
    [TestMethod]
    [TestCategory("20")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOpAfterOpenParen_Fails()
    {
        _ = new Formula("( + 2)");
    }

    /// <summary>
    ///   Test that a closing paren cannot come after an opening paren.
    /// </summary>
    [TestMethod]
    [TestCategory("21")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestEmptyParens_Fails()
    {
        _ = new Formula("()");
    }

    // --- Test Extra Following Rule ---

    /// <summary>
    ///   Make sure that two consecutive numbers are invalid.
    /// </summary>
    [TestMethod]
    [TestCategory("22")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestConsecutiveNumbers_Fails()
    {
        _ = new Formula("5 5");
    }

    /// <summary>
    ///   Test that two consecutive operators is invalid.
    /// </summary>
    [TestMethod]
    [TestCategory("23")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestConsecutiveOps_Fails()
    {
        _ = new Formula("5+-2");
    }

    /// <summary>
    ///   Test that a closing paren cannot come after an operator (plus).
    /// </summary>
    [TestMethod]
    [TestCategory("24")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestCloseParenAfterOp()
    {
        _ = new Formula("(5+)2");
    }

    /// <summary>
    ///   Test bad variable name.
    /// </summary>
    [TestMethod]
    [TestCategory("25")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidVariableName_Throws()
    {
        _ = new Formula("a");
    }

    // Get Vars Tests
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("26")]
    public void GetVars_BasicVariable_ReturnsVariable()
    {
        Formula f = new("2+X1");
        ISet<string> vars = f.GetVariables();

        Assert.IsTrue(vars.SetEquals(["X1"]));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("27")]
    public void GetVariables_ManyVariables_ReturnsThemAll()
    {
        Formula f = new("X1+X2+X3+X4+A1+B1+C5");
        ISet<string> vars = f.GetVariables();

        Assert.IsTrue(vars.SetEquals(["X1", "X2", "X3", "X4", "A1", "B1", "C5"]));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("28")]
    public void TestGetVars_ManySameVariable_ReturnsUniqueVariable()
    {
        Formula f = new("X1+X1+X1+X1+X1+X1+X1+X1+X1+X1+X1");
        ISet<string> vars = f.GetVariables();

        Assert.IsTrue(vars.SetEquals(["X1"]));
    }

    // To String Tests
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("29")]
    public void ToString_BasicFormula_ReturnsSameFormula()
    {
        Formula f1 = new("2+A1");

        Assert.IsTrue(f1.ToString().Equals("2+A1"));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("30")]
    public void ToString_Numbers_UsesCanonicalForm()
    {
        Formula f1 = new("2.0000+A1");
        Assert.IsTrue(f1.ToString().Equals("2+A1"));
        f1 = new("2.0000-3");
        Assert.IsTrue(f1.ToString().Equals("2-3"));
        f1 = new("2.0000-3e2");
        Assert.IsTrue(f1.ToString().Equals("2-300"));
        f1 = new("1e20");
        Assert.IsTrue(f1.ToString().Equals("1E+20"));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("31")]
    public void ToString_SpacesInFormula_SpacesRemoved()
    {
        Formula f1 = new("        2             +                    A1          ");
        Assert.IsTrue(f1.ToString().Equals("2+A1"));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("32")]
    public void NormalizerAndToString_LowerCaseAndUpperCase_ResultInSameString()
    {
        Formula f1 = new("2+x1");
        Formula f2 = new("2+X1");

        Assert.IsTrue(f1.ToString().Equals("2+X1"));
        Assert.IsTrue(f1.ToString().Equals(f2.ToString()));
    }

    // Normalizer tests
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("32")]
    public void NormalizerAndGetVars_LowerCaseVariable_UpCasesVariable()
    {
        Formula f = new("2+x1");
        ISet<string> vars = f.GetVariables();

        Assert.IsTrue(vars.SetEquals(["X1"]));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("33")]
    public void GetVars_ManyCaseSwappingVariables_UpCasesAll()
    {
        Formula f = new("x1+X2+x3+X4+a1+B1+c5");
        ISet<string> vars = f.GetVariables();

        Assert.IsTrue(vars.SetEquals(["X1", "X2", "X3", "X4", "A1", "B1", "C5"]));
    }

    // Some more general syntax errors detected by the constructor
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("34")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestSingleOperator()
    {
        new Formula("+");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("35")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraOperator()
    {
        new Formula("2+5+");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("36")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraCloseParen()
    {
        new Formula("2+5*7)");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("37")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestExtraOpenParen()
    {
        new Formula("((3+5*7)");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("38")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestXasMultiply()
    {
        new Formula("5x5");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("39")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator2()
    {
        new Formula("5+5x");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("40")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator3()
    {
        new Formula("5+7+(5)8");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("41")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestNoOperator4()
    {
        new Formula("5 5");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("42")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void TestDoubleOperator()
    {
        new Formula("5 + + 3");
    }

    // Some more complicated formula evaluations
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("43")]
    public void FormulaConstructor_TestComplex_IsValid()
    {
        Formula f = new("y1*3-8/2+4*(8-9*2)/14*x7");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("44")]
    public void FormulaConstructor_MatchingParens_EachLeftHasARight()
    {
        Formula f = new("x1+(x2+(x3+(x4+(x5+x6))))");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("45")]
    public void FormulaConstructor_TestLotsOfLeftParens_IsValidAndMatching()
    {
        Formula f = new("((((x1+x2)+x3)+x4)+x5)+x6");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("46")]
    public void ToString_Whitespace_RemovedInCannonicalForm()
    {
        Formula f1 = new("X1+X2");
        Formula f2 = new(" X1  +  X2   ");
        Assert.IsTrue(f1.ToString().Equals(f2.ToString()));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("47")]
    public void ToString_DifferentNumberRepresentations_EquateToSameCanonicalForm()
    {
        Formula f1 = new("2+X1*3.00");
        Formula f2 = new("2.00+X1*3.0");
        Assert.IsTrue(f1.ToString().Equals(f2.ToString()));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("48")]
    public void ToString_DifferentNumberRepresentations_EquateToSameCanonicalForm2()
    {
        Formula f1 = new("1e-2 + X5 + 17.00 * 19 ");
        Formula f2 = new("   0.0100  +     X5+ 17 * 19.00000 ");
        Assert.IsTrue(f1.ToString().Equals(f2.ToString()));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("49")]
    public void ToString_DifferentFormulas_HaveDifferentStrings()
    {
        Formula f1 = new("2");
        Formula f2 = new("5");
        Assert.IsTrue(f1.ToString() != f2.ToString());
    }

    // Tests of GetVariables method
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("50")]
    public void GetVariables_NoVariables_ReturnsEmptySet()
    {
        Formula f = new("2*5");
        Assert.IsFalse(f.GetVariables().Any());
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("51")]
    public void GetVariables_OneVariable_ReturnsTheOne()
    {
        Formula f = new("2*X2");
        List<string> actual = new(f.GetVariables());
        HashSet<string> expected = ["X2"];
        Assert.AreEqual(actual.Count, 1);
        Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("52")]
    public void GetVariables_TwoVariables_ReturnsBoth()
    {
        Formula f = new("2*X2+Y3");
        List<string> actual = new(f.GetVariables());
        HashSet<string> expected = ["Y3", "X2"];
        Assert.AreEqual(actual.Count, 2);
        Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("53")]
    public void GetVariables_Duplicated_ReturnsOnlyOneValue()
    {
        Formula f = new("2*X2+X2");
        List<string> actual = new(f.GetVariables());
        HashSet<string> expected = ["X2"];
        Assert.AreEqual(actual.Count, 1);
        Assert.IsTrue(expected.SetEquals(actual));
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("54")]
    public void GetVariables_LotsOfVariablesWithOperatorsAndRepeats_ReturnsCompleteList()
    {
        Formula f = new("X1+Y2*X3*Y2+Z7+X1/Z8");
        List<string> actual = new(f.GetVariables());
        HashSet<string> expected = ["X1", "Y2", "X3", "Z7", "Z8"];
        Assert.AreEqual(actual.Count, 5);
        Assert.IsTrue(expected.SetEquals(actual));
    }

    // Test some longish valid formulas.
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("55")]
    public void FormulaConstructor_LongComplexFormula_IsAValidFormula()
    {
        _ = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
    }

    [TestMethod]
    [Timeout(2000)]
    [TestCategory("56")]
    public void FormulaConstructor_LongComplexFormula2_IsAValidFormula()
    {
        _ = new Formula("5 + (1-2) * 3.14 / 1e6 + 0.2E-9 - A1 + bb22");
    }










    // --- Personal PS4 Tests ---

    // -- Tests for Equals ---

    [TestMethod]
    public void Equals_NullObject_ReturnsFalse()
    {
        Formula formula = new Formula("2+3");
        bool result = formula.Equals(null);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_SameObject_ReturnsTrue()
    {
        Formula formula = new Formula("2+3");
        bool result = formula.Equals(formula);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_EquivalentFormulas_ReturnsTrue()
    {
        Formula formula1 = new Formula("2+3");
        Formula formula2 = new Formula("2.0+3.0");
        bool result = formula1.Equals(formula2);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_DifferentFormulas_ReturnsFalse()
    {
        Formula formula1 = new Formula("2+3");
        Formula formula2 = new Formula("2+4");
        bool result = formula1.Equals(formula2);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_FormulaWithDifferentSpacing_ReturnsTrue()
    {
        Formula formula1 = new Formula("2+3");
        Formula formula2 = new Formula(" 2   +  3 ");
        bool result = formula1.Equals(formula2);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_NonFormulaObject_ReturnsFalse()
    {
        Formula formula = new Formula("2+3");
        string nonFormulaObject = "Not a Formula";
        bool result = formula.Equals(nonFormulaObject);

        Assert.IsFalse(result);
    }

    // -- Tests for Equality Operator ---

    [TestMethod]
    public void EqualityOperator_DoubleNullFormulas_ReturnsTrue()
    {
        Formula formula1 = null;
        Formula formula2 = null;

        Assert.IsTrue(formula1 == formula2);
    }

    [TestMethod]
    public void EqualityOperator_SingleNullFormula_ReturnsFalse()
    {
        Formula formula1 = null;
        Formula formula2 = new Formula("2+3");

        Assert.IsFalse(formula1 == formula2);
    }

    [TestMethod]
    public void EqualityOperator_EquivalentFormulas_ReturnsTrue()
    {
        Formula formula1 = new Formula("2+3");
        Formula formula2 = new Formula("2+3");

        Assert.IsTrue(formula1 == formula2);
    }

    // -- Tests for Inequality Operator ---

    [TestMethod]
    public void InequalityOperator_DifferentFormulas_ReturnsTrue()
    {
        Formula formula1 = new Formula("2+3");
        Formula formula2 = new Formula("2+4");

        Assert.IsTrue(formula1 != formula2);
    }

    // -- Tests for GetHashCode ---

    [TestMethod]
    public void GetHashCode_EquivalentFormulas_ReturnsEqual()
    {
        Formula formula1 = new Formula("2+3");
        Formula formula2 = new Formula("2+3");
        int hashCode1 = formula1.GetHashCode();
        int hashCode2 = formula2.GetHashCode();

        Assert.AreEqual(hashCode1, hashCode2);
    }

    [TestMethod]
    public void GetHashCode_DifferentFormulas_ReturnsNotEqual()
    {
        Formula formula1 = new Formula("2+3");
        Formula formula2 = new Formula("2+4");
        int hashCode1 = formula1.GetHashCode();
        int hashCode2 = formula2.GetHashCode();

        Assert.AreNotEqual(hashCode1, hashCode2);
    }

    // -- Tests for Evaluate ---

    // NUMBER TESTS

    [TestMethod]
    public void Evaluate_NumberAddNumber_ReturnsNumber()
    {
        Formula formula1 = new Formula("1+1");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(2.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMinusNumber_ReturnsNumber()
    {
        Formula formula1 = new Formula("2-1");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(1.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMultiplyNumber_ReturnsNumber()
    {
        Formula formula1 = new Formula("2*2");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(4.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberDivideNumber_ReturnsNumber()
    {
        Formula formula1 = new Formula("4/2");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(2.0, result);
    }

    [TestMethod]
    public void Evaluate_ZeroDivideNumber_ReturnsNumber()
    {
        Formula formula1 = new Formula("0/4");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(0.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberDivideZero_ReturnsFormulaError()
    {
        Formula formula1 = new Formula("4/0");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    // VARIABLE TESTS

    [TestMethod]
    public void Evaluate_NumberAddVariable_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("2+A1");
        Lookup variableLookup = s =>
        {
            if (s == "A1") return 5.0;
            throw new ArgumentException();
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.AreEqual(7.0, result);
    }

    [TestMethod]
    public void Evaluate_VariableAddNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("A1+2");
        Lookup variableLookup = s =>
        {
            if (s == "A1") return 5.0;
            throw new ArgumentException();
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.AreEqual(7.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMinusVariable_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("5-A1");
        Lookup variableLookup = s =>
        {
            if (s == "A1") return 1.0;
            throw new ArgumentException();
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.AreEqual(4.0, result);
    }

    [TestMethod]
    public void Evaluate_VariableMinusNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("A1-5");
        Lookup variableLookup = s =>
        {
            if (s == "A1") return 10.0;
            throw new ArgumentException();
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.AreEqual(5.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMultiplyVariable_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("2*A1");
        Lookup variableLookup = s =>
        {
            if (s == "A1") return 5.0;
            throw new ArgumentException();
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.AreEqual(10.0, result);
    }

    [TestMethod]
    public void Evaluate_VariableMultiplyNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("A1*2");
        Lookup variableLookup = s =>
        {
            if (s == "A1") return 5.0;
            throw new ArgumentException();
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.AreEqual(10.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberDivideVariable_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("4/A1");
        Lookup variableLookup = s =>
        {
            if (s == "A1") return 2.0;
            throw new ArgumentException();
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.AreEqual(2.0, result);
    }

    [TestMethod]
    public void Evaluate_VariableDivideNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("A1/2");
        Lookup variableLookup = s =>
        {
            if (s == "A1") return 4.0;
            throw new ArgumentException();
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.AreEqual(2.0, result);
    }

    [TestMethod]
    public void Evaluate_VariableZeroDivideNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("A1/2");
        Lookup variableLookup = s =>
        {
            if (s == "A1") return 0.0;
            throw new ArgumentException();
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.AreEqual(0.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberDivideVariableZero_ReturnsFormulaError()
    {
        Formula formula1 = new Formula("4/A1");
        Lookup variableLookup = s =>
        {
            if (s == "A1") return 0.0;
            throw new ArgumentException();
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    [TestMethod]
    public void Evaluate_FormulaWithUnknownVariable_ReturnsFormulaError()
    {
        Formula formula1 = new Formula("2+A1");
        Lookup variableLookup = s =>
        {
            throw new ArgumentException("Variable not found");
        };
        var result = formula1.Evaluate(variableLookup);

        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    // REPEATED OPERATOR TESTS

    [TestMethod]
    public void Evaluate_RepeatedAddition_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("1+2+3+4+5");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(15.0, result);
    }

    [TestMethod]
    public void Evaluate_RepeatedSubtraction_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("10-5-2-1");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(2.0, result);
    }

    [TestMethod]
    public void Evaluate_RepeatedMultiplication_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("5*4*3*2*1");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(120.0, result);
    }

    [TestMethod]
    public void Evaluate_RepeatedDivision_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("100/2/5/2");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(5.0, result);
    }

    // LONG COMPLEX TEST

    [TestMethod]
    public void Evaluate_LongComplexHybrid_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("1+(4/2)*2-5/(10/5)*2");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(0.0, result);
    }

    // PARENTHESES TEST

    [TestMethod]
    public void Evaluate_NumberAddParenthesesNumberAddNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("1+(1+1)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(3.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberAddParenthesesNumberMinusNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("1+(4-2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(3.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberAddParenthesesNumberMultiplyNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("1+(4*2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(9.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberAddParenthesesNumberDivideNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("1+(4/2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(3.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberAddParenthesesZeroDivideNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("1+(0/4)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(1.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberAddParenthesesNumberDivisionZero_ReturnsFormulaError()
    {
        Formula formula1 = new Formula("1+(2/0)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    [TestMethod]
    public void Evaluate_NumberMinusParenthesesNumberMinusNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("20-(20-10)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(10.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMinusParenthesesNumberAddNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("20-(5+5)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(10.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMinusParenthesesNumberMultiplyNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("20-(5*2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(10.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMinusParenthesesNumberDivideNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("20-(20/10)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(18.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMinusParenthesesZeroDivideNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("20-(0/2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(20.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMinusParenthesesNumberDivisionZero_ReturnsFormulaError()
    {
        Formula formula1 = new Formula("20-(5/0)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    [TestMethod]
    public void Evaluate_NumberMultiplyParenthesesNumberMultiplyNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("2*(2*2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(8.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMultiplyParenthesesNumberAddNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("2*(2+2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(8.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMultiplyParenthesesNumberMinusNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("2*(4-2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(4.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMultiplyParenthesesNumberDivideNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("2*(4/2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(4.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMultiplyParenthesesZeroDivideNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("2*(0/4)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(0.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberMultiplyParenthesesNumberDivisionZero_ReturnsFormulaError()
    {
        Formula formula1 = new Formula("2*(4/0)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    [TestMethod]
    public void Evaluate_NumberDivideParenthesesNumberDivideNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("10/(10/2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(2.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberDivideParenthesesNumberAddNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("10/(2+3)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(2.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberDivideParenthesesNumberMinusNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("10/(10-5)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(2.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberDivideParenthesesNumberMultiplyNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("20/(5*2)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(2.0, result);
    }

    [TestMethod]
    public void Evaluate_ZeroDivideParenthesesNumberDivideNumber_ReturnsCorrectValue()
    {
        Formula formula1 = new Formula("0/(10/5)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.AreEqual(0.0, result);
    }

    [TestMethod]
    public void Evaluate_NumberDivideParenthesesNumberDivisionZero_ReturnsFormulaError()
    {
        Formula formula1 = new Formula("20/(5/0)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    [TestMethod]
    public void Evaluate_NumberDivideParenthesesZeroDivideNumber_ReturnsFormulaError()
    {
        Formula formula1 = new Formula("10/(0/4)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    [TestMethod]
    public void Evaluate_ZeroDivideParenthesesZeroDivideNumber_ReturnsFormulaError()
    {
        Formula formula1 = new Formula("0/(0/4)");
        Lookup simpleLookup = s => throw new ArgumentException();
        var result = formula1.Evaluate(simpleLookup);

        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }










    // -- Teacher PS4 Tests ---

    /// <summary>
    ///   Test that a single number equals itself.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("1")]
    public void Evaluate_SingleNumber_Equals5()
    {
        Formula formula = new("5");
        Assert.AreEqual(5.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test that a single variable evaluates to the expected value.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("2")]
    public void Evaluate_SingleVariable_EqualsLookup()
    {
        Formula formula = new("X5");
        Assert.AreEqual(13.0, formula.Evaluate(s => 13));
        Assert.AreEqual(20.0, formula.Evaluate(s => 20));
        Assert.AreEqual(-1.0, formula.Evaluate(s => -1));
    }

    /// <summary>
    ///   Test that simple addition works.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("3")]
    public void Evaluate_AdditionOperator_Equals8()
    {
        Formula formula = new("5+3");
        Assert.AreEqual(8.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test that simple subtraction works.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("4")]
    public void Evaluate_SubtractionOperator_Equals8()
    {
        Formula formula = new("18-10");
        Assert.AreEqual(8.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test that simple multiplication works.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("5")]
    public void Evaluate_MultiplicationOperator_Equals8()
    {
        Formula formula = new("2*4");
        Assert.AreEqual(8.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test that simple division works.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("6")]
    public void Evaluate_DivisionOperator_Equals8()
    {
        Formula formula = new("16/2");
        Assert.AreEqual(8.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test that Variables work with Arithmetic.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("7")]
    public void Evaluate_VariablePlusValue_Equals8()
    {
        Formula formula = new("2+X1");
        Assert.AreEqual(8.0, formula.Evaluate(s => 6));
    }

    /// <summary>
    ///   Test multiple variables.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("8")]
    public void Evaluate_MultipleVariables_Equals100()
    {
        Formula formula = new("X1+X2+X3+X4");
        Assert.AreEqual(100.0, formula.Evaluate(s => (s == "X1") ? 55 : 15));
    }

    /// <summary>
    ///   Test variables normalization.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("9")]
    public void Evaluate_LowerCaseVariable_Equals100()
    {
        Formula formula = new("x1+X1");
        Assert.AreEqual(100.0, formula.Evaluate(s => 50));
    }

    /// <summary>
    ///   Test that an unknown variable returns a formula error object.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("10")]
    public void Evaluate_TestUnknownVariable()
    {
        Formula formula = new("2+X1");
        var result = formula.Evaluate(s => { throw new ArgumentException("Unknown variable"); });
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    /// <summary>
    ///   Test order of operation precedence * before +.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("11")]
    public void Evaluate_OperatorPrecedence_MultiplicationThenAdd()
    {
        Formula formula = new("2*3+2");
        Assert.AreEqual(8.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test order of operation precedence * before +.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("12")]
    public void Evaluate_OperatorPrecedence_SubtractThenMultiplication()
    {
        Formula formula = new("26-6*3");
        Assert.AreEqual(8.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test that parentheses override precedence rules (or that they have
    ///   the highest precedence).
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("13")]
    public void Evaluate_ParenthesesBeforeTimes_Equals8()
    {
        Formula formula = new("(2+2)*2");
        Assert.AreEqual(8.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test that parentheses have higher precedence even when
    ///   they come last.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("14")]
    public void Evaluate_ParenthesesAfterTimes_Equals100()
    {
        Formula formula = new("20*(6-1)");
        Assert.AreEqual(100.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Evaluate that parentheses don't make a difference when
    ///   they shouldn't make a difference.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("15")]
    public void Evaluate_PlusInParentheses_Equals100()
    {
        Formula formula = new("25+(25+25)+25");
        Assert.AreEqual(100.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Evaluate a slightly more involved expression with
    ///   parentheses and order of precedence.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("16")]
    public void Evaluate_PlusTimesAndParentheses_Equals100()
    {
        Formula formula = new("2+(3+5*9)-(50-100)");
        Assert.AreEqual(100.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test operators directly after parentheses.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("17")]
    public void Evaluate_OperatorAfterParens_Equals100()
    {
        Formula formula = new("(10*11)-10/1");
        Assert.AreEqual(100.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test another more complex set of parentheses with all operators.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("18")]
    public void Evaluate_TestComplexAllOperatorsAndParentheses_Equals100()
    {
        Formula formula = new("200-3*(3+5)*3/2-(8*8)");
        Assert.AreEqual(100.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test another complex equation.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("19")]
    public void Evaluate_ComplexAndParentheses_Equals100()
    {
        Formula formula = new("(2+3*5+(3+4*8)*5+2)-94");
        Assert.AreEqual(100.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test division by zero.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("20")]
    public void Evaluate_DirectDivideByZero_FormulaError()
    {
        Formula formula = new("5/0");
        var result = formula.Evaluate(s => 0);
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    /// <summary>
    ///   Divide by zero as computed by variables.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("21")]
    public void TestDivideByZeroVars()
    {
        Formula f = new("(5 + X1) / (X1 - 3)");
        var result = f.Evaluate(s => 3);
        Assert.IsInstanceOfType(result, typeof(FormulaError));
    }

    /// <summary>
    ///   Test complex formula with multiple variables.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("22")]
    public void Evaluate_ComplexMultiVar_EqualsNegative18()
    {
        Formula formula = new("(Y1*3-8/2+4*(8-9*2)/2*X7)-6");
        Assert.AreEqual(-18.0, formula.Evaluate(s => (s == "X7") ? 1 : 4));
    }

    /// <summary>
    ///   Lots of nested parens, following on the right.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("23")]
    public void Evaluate_TestComplexNestedParensRight_Equals6()
    {
        Formula formula = new("x1+(x2+(x3+(x4+(x5+x6))))");
        Assert.AreEqual(6.0, formula.Evaluate(s => 1));
    }

    /// <summary>
    ///  Lots of nested parens, starting on the left side.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("24")]
    public void Evaluate_TestComplexNestedParensLeft_Equals12()
    {
        Formula formula = new("((((x1+x2)+x3)+x4)+x5)+x6");
        Assert.AreEqual(12.0, formula.Evaluate(s => 2));
    }

    /// <summary>
    ///   Simple Repeated Variable.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("25")]
    public void Evaluate_RepeatedVarWithVariousOperators_Equals3()
    {
        Formula formula = new("a4-a4*a4/a4+a4");
        Assert.AreEqual(3.0, formula.Evaluate(s => 3));
    }

    /// <summary>
    ///   Test that the formula is not using a shared stack between
    ///   calls.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("26")]
    public void Evaluate_SeparateStacks_StacksClearedEachTimeAndEquals15()
    {
        Formula formula = new("2*6+3");
        Assert.AreEqual(15.0, formula.Evaluate(s => 0));
        Assert.AreEqual(15.0, formula.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test that the formula is not using a shared stack between
    ///   multiple formulas.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("27")]
    public void Evaluate_FormulasAreIndependent_Equal15_14_11()
    {
        Formula formula1 = new("2*6+3");
        Formula formula2 = new("2*6+2");
        Formula formula3 = new("2*6-1");
        Assert.AreEqual(15.0, formula1.Evaluate(s => 0));
        Assert.AreEqual(14.0, formula2.Evaluate(s => 0));
        Assert.AreEqual(11.0, formula3.Evaluate(s => 0));
    }

    /// <summary>
    ///   Test that variable values don't matter if there are no variables.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("28")]
    public void Evaluate_VariablesHaveValueButFormulaHasNoVariables_Equals10()
    {
        Formula formula = new("2*6+3");
        Assert.AreEqual(15.0, formula.Evaluate(s => 100));
    }

    /// <summary>
    ///   Check a formula that computes a lot of decimal places.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("29")]
    public void Evaluate_ComplexLotsOfDecimalPlaces_Equals514285714285714()
    {
        Formula f = new("y1*3-8/2+4*(8-9*2)/14*x7");
        double result = (double)f.Evaluate(s => (s == "X7") ? 1 : 4);
        Assert.AreEqual(5.14285714285714, result, 1e-9);
    }

    /// <summary>
    ///   Check a formula that computes pi to 10 decimal places using
    ///   10000 adds and subtracts.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("30")]
    public void Evaluate_ComputePiStress_PiTo4DecimalPlaces()
    {
        StringBuilder formulaString = new("4 * ( 1");
        bool negative = true;
        for (int i = 3; i < 10000; i += 2)
        {
            formulaString.Append((negative ? "-" : "+") + $"1/{i}");
            negative = !negative;
        }

        formulaString.Append(')');
        Formula f = new(formulaString.ToString());
        double result = (double)f.Evaluate(s => 0);
        Assert.AreEqual(3.1415926535, result, 1e-3);
    }

    // Equality and Hash tests

    /// <summary>
    ///   Test basic equality of two identical formulas.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("31")]
    public void Equals_TwoSameFormula_AreEqual()
    {
        Formula f1 = new("X1+X2");
        Formula f2 = new("X1+X2");
        Assert.IsTrue(f1.Equals(f2));
    }

    /// <summary>
    ///   Test that whitespace doesn't matter to equals.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("32")]
    public void Equals_CheckWhitespace_SameEquation()
    {
        Formula f1 = new("X1+X2");
        Formula f2 = new(" X1  +  X2   ");
        Assert.IsTrue(f1.Equals(f2));
    }

    /// <summary>
    ///   Test that different number notations don't matter to equals.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("33")]
    public void Equals_DifferentNumberSyntaxes_SameFormula()
    {
        Formula f1 = new("2+X1*3.00");
        Formula f2 = new("2.00+X1*3.0");
        Assert.IsTrue(f1.Equals(f2));
    }

    /// <summary>
    ///   Test a little more complex string equality (canonical form).
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("34")]
    public void Equals_MoreComplexEquality_SameFormula()
    {
        Formula f1 = new("1e-2 + X5 + 17.00 * 19 ");
        Formula f2 = new("   0.0100  +     X5+ 17 * 19.00000 ");
        Assert.IsTrue(f1.Equals(f2));
    }

    /// <summary>
    ///   Test on null and empty string.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("35")]
    public void Equals_NullAndEmptyString_NotEqual()
    {
        Formula f = new("2");

        Assert.IsFalse(f.Equals(null));
        Assert.IsFalse(f.Equals(string.Empty));
    }

    /// <summary>
    ///   Test on a different object type.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("36")]
    public void Equals_NonFormulaObject_NotEqual()
    {
        Formula f = new("2");

        Assert.IsFalse(f.Equals(new List<string>()));
    }

    /// <summary>
    ///   Test the == operator.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("37")]
    public void OperatorDoubleEquals_TwoDifferentObjects_AreEqual()
    {
        Formula f1 = new("2");
        Formula f2 = new("2");

        Assert.IsTrue(f1 == f2);
    }

    /// <summary>
    ///   Test that == shows that two different formula are different.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("38")]
    public void OperatorDoubleEquals_TwoDifferentFormula_NotEqual()
    {
        Formula f1 = new("2");
        Formula f2 = new("5");
        Assert.IsFalse(f1 == f2);
    }

    /// <summary>
    ///   Test the not equals operator.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("39")]
    public void OperatorNotEqual_TwoEqualFormula_NotEqual()
    {
        Formula f1 = new("2");
        Formula f2 = new("2");
        Assert.IsFalse(f1 != f2);
    }

    /// <summary>
    ///   Test thenot equals operator on two different objects with different values.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("40")]
    public void OperatorNotEqual_TwoDifferentFormulas_NotEqual()
    {
        Formula f1 = new("2");
        Formula f2 = new("5");
        Assert.IsTrue(f1 != f2);
    }

    /// <summary>
    ///   Test that the hashcode of two alike formulas are the same.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("41")]
    public void GetHashCode_EqualFormulas_SameHashCodes()
    {
        Formula f1 = new("2*5");
        Formula f2 = new("2*5");
        Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
    }

    /// <summary>
    ///   Technically the hashcodes could not be equal and still be valid,
    ///   extremely unlikely though. Check their implementation if this fails.
    ///   <para>
    ///     While it is very unlikely that two different formula will have
    ///     the same hashcode, it becomes ridiculously unlikely if we do
    ///     three formulas.
    ///   </para>
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("42")]
    public void GetHashCode_DifferentFormulas_DifferentCodes()
    {
        Formula f1 = new("2*5");
        Formula f2 = new("3/8*2+(7)");
        Formula f3 = new("1");
        Assert.IsTrue(f1.GetHashCode() != f2.GetHashCode() ||
                       f2.GetHashCode() != f3.GetHashCode());
    }

    /// <summary>
    ///   Check to make sure that the hash code is computed on the
    ///   normalized form.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("43")]
    public void GetHashCode_CheckCanonicalForms_AreSame()
    {
        Formula f1 = new("2 * 5 + 4.00 - x1");
        Formula f2 = new("2*5+4-X1");
        Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
    }

    // A set of tests just to verify no regression has taken place!

    /// <summary>
    ///  Test invalid formula (really a previous assignment verification).
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("44")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidFormula_Throws()
    {
        _ = new Formula("+");
    }

    /// <summary>
    ///   Test extra operator at end.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("45")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ExtraOperator_Throws()
    {
        _ = new Formula("2+5+");
    }

    /// <summary>
    ///   Test extra parentheses.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("46")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ExtraParentheses_Throws()
    {
        _ = new Formula("2+5*7)");
    }

    /// <summary>
    ///   Test Invalid Variable Name.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("47")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidVariable_Throws()
    {
        _ = new Formula("xx");
    }

    /// <summary>
    ///   Test no implicit multiplication (5)(5) does not equal 25.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("48")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_NoImplicitMultiplication_Throws()
    {
        _ = new Formula("5+7+(5)8");
    }

    /// <summary>
    ///   Test that Empty Formula throws.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("49")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_Empty_Throws()
    {
        _ = new Formula(string.Empty);
    }

    /// <summary>
    ///   Test that ToString continues to work.
    /// </summary>
    [TestMethod]
    [Timeout(5000)]
    [TestCategory("50")]
    public void FormulaToString_CreatesEqualFormula_EqualEachOther()
    {
        Formula f1 = new("(1+2*(3/4))");
        Formula f2 = new(f1.ToString());

        Assert.AreEqual(f1.Evaluate(s => 0), f2.Evaluate(s => 0));
        Assert.AreEqual(f1.ToString(), f2.ToString());
    }

}