/// <copyright file="SpreadsheetPage.razor.cs" company="UofU-CS3500">
///     Copyright (c) 2024 UofU-CS3500. All rights reserved.
/// </copyright>
/// 
/// Name: Harrison Doppelt & Victor Valdez Landa
/// Date: 10/24/2024

namespace GUI.Client.Pages;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using CS3500.Spreadsheet;
using System.Net.Http.Json;
using CS3500.Formula;

/// <summary>
///     Represents the logic and interface for managing the Spreadsheet GUI page.
///     This class handles all interactions with the spreadsheet, including cell selection,
///     content updates, saving/loading files, and updating the spreadsheet view.
///     It connects the spreadsheet logic with the user interface and ensures proper
///     data handling and display.
/// </summary>
public partial class SpreadsheetPage
{
    /// <summary>
    ///     Based on your computer, you could shrink/grow this value based on performance.
    /// </summary>
    private const int ROWS = 50;

    /// <summary>
    ///     Number of columns, which will be labeled A-Z.
    /// </summary>
    private const int COLS = 26;

    /// <summary>
    ///     Provides an easy way to convert from an index to a letter (0 -> A)
    /// </summary>
    private char[] Alphabet {get;} = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    /// <summary>
    ///     Gets or sets the name of the file to be saved
    /// </summary>
    private string FileSaveName {get; set;} = "Spreadsheet.sprd";

    /// <summary>
    ///     <para> Gets or sets the data for all of the cells in the spreadsheet GUI. </para>
    ///     <remarks>Backing Store for HTML</remarks>
    /// </summary>
    private string[,] CellsBackingStore {get; set;} = new string[ROWS, COLS];

    /// <summary>
    ///     Default starting cell name.
    /// </summary>
    private string SelectedCell = "A1";

    /// <summary>
    ///     Default empty cell content.
    /// </summary>
    private string SelectedContents = string.Empty;

    /// <summary>
    ///     Default empty cell value.
    /// </summary>
    private object SelectedValue = string.Empty;

    /// <summary>
    ///     The currently selected row in the spreadsheet. Defaults to row 0 (first row).
    ///     This is used to keep track of which row is active based on user interaction.
    /// </summary>
    private int SelectedRow = 0;

    /// <summary>
    ///     The currently selected column in the spreadsheet. Defaults to column 0 (first column).
    ///     This is used to keep track of which column is active based on user interaction.
    /// </summary>
    private int SelectedCol = 0;

    /// <summary>
    ///     Reference to the HTML TextArea element used in the spreadsheet for editing cell contents.
    ///     This is used to programmatically focus on or manipulate the text area.
    /// </summary>
    private ElementReference TextArea;

    /// <summary>
    ///     Initialize a new Spreadsheet
    /// </summary>
    private Spreadsheet sheet = new();

    /// <summary>
    ///     Handler for when a cell is clicked
    /// </summary>
    /// <param name="row">The row component of the cell's coordinates</param>
    /// <param name="col">The column component of the cell's coordinates</param>
    private void CellClicked(int row, int col)
    {
        SelectedCell = $"{Alphabet[col]}{row+1}";
        //Added back the two since an error displaying contents and it was only displaying the first cell, FIXED!!!!!!!
        SelectedRow = row;
        SelectedCol = col;
        SelectedContents = CellsBackingStore[row, col];
        SelectedValue = sheet.GetCellValue(SelectedCell);
        if (SelectedValue is FormulaError) {
            SelectedValue = "FormulaError";
        }
        TextArea.FocusAsync();
    }

    /// <summary>
    ///     Handles the event when the content of a cell is changed by the user.
    /// </summary>
    /// <param name="e">The event arguments containing the new content value for the cell.</param>
    /// <returns>A task that represents the asynchronous operation of updating the cell content and value.</returns>
    /// <remarks>
    ///     This method attempts to update the cell's content in the spreadsheet and recalculate its evaluated value. 
    ///     If an error occurs during this process, such as an invalid formula, an error message is displayed in a pop-up.
    ///     The backing store for the spreadsheet is updated with the new content, and the text area is refocused after the operation completes.
    /// </remarks>
    private async Task CellContentChanged(ChangeEventArgs e)
    {
        string data = e.Value!.ToString() ?? "";

        try
        {
            sheet.SetContentsOfCell(SelectedCell, data);
            SelectedValue = sheet.GetCellValue(SelectedCell);

            if (SelectedValue is FormulaError)
            {
                SelectedValue = "FormulaError";
            }

            CellsBackingStore[SelectedRow, SelectedCol] = data;
            SelectedContents = data;
            await TextArea.FocusAsync();
            StateHasChanged();
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Error in cell {SelectedCell}: \n{ex.Message}");
        }
    }

    /// <summary>
    ///     Saves the current spreadsheet, by providing a download of a file
    ///     containing the json representation of the spreadsheet.
    /// </summary>
    private async void SaveFile()
    {
        string jsonSpreadsheet = sheet.GetSpreadsheetAsJson();
        await JSRuntime.InvokeVoidAsync("downloadFile", FileSaveName, jsonSpreadsheet);
    }

    /// <summary>
    ///     This method will run when the file chooser is used, for loading a file.
    ///     Uploads a file containing a json representation of a spreadsheet, and 
    ///     replaces the current sheet with the loaded one.
    /// </summary>
    /// <param name="args">The event arguments, which contains the selected file name</param>
    private async void HandleFileChooser(EventArgs args)
    {
        try
        {
            string fileContent = string.Empty;

            InputFileChangeEventArgs eventArgs = args as InputFileChangeEventArgs ?? throw new Exception("Unable to get file name");
            if (eventArgs.FileCount == 1)
            {
                var file = eventArgs.File;
                if (file is null)
                {
                    return;
                }

                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);

                // fileContent will contain the contents of the loaded file
                fileContent = await reader.ReadToEndAsync();

                // Use the loaded fileContent to replace the current spreadsheet
                sheet.LoadSpreadsheetContents(fileContent);
                UpdateCellsBackingStore();
                
                StateHasChanged();
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine("An error occurred while loading the file..." + e);
        }
    }

    /// <summary>
    /// Helper method that updates the CellsBackingStore array when a file is loaded.
    /// </summary>
    private void UpdateCellsBackingStore()
    {
        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col < COLS; col++)
            {
                string cellName = $"{Alphabet[col]}{row + 1}";
                var cellContent = sheet.GetCellContents(cellName);

                if (cellContent is Formula)
                {
                    // If the content is a formula, add a = before it
                    CellsBackingStore[row, col] = $"={cellContent}";
                }
                else
                {
                    // Otherwise, just store the content
                    CellsBackingStore[row, col] = cellContent?.ToString() ?? string.Empty;
                }
            }
        }
    }

    /// <summary>
    ///     Clears all the cells in the spreadsheet by setting their contents to an empty string.
    ///     Resets the selected cell, selected contents, and selected value to their default states.
    /// </summary>
    private void ClearAllCells()
    {
        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col < COLS; col++)
            {
                SelectedCell = $"{Alphabet[col]}{row + 1}";
                sheet.SetContentsOfCell(SelectedCell, "");
                CellsBackingStore[row, col] = string.Empty;
            }
        }

        SelectedCell = "A1";
        SelectedContents = string.Empty;
        SelectedValue = string.Empty;

        StateHasChanged();
    }
}
