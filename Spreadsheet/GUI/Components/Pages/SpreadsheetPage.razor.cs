﻿// <copyright file="SpreadsheetPage.razor.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace GUI.Client.Pages;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using CS3500.Spreadsheet;

/// <summary>
/// TODO: Fill in
/// </summary>
public partial class SpreadsheetPage
{
    /// <summary>
    /// Based on your computer, you could shrink/grow this value based on performance.
    /// </summary>
    private const int ROWS = 50;

    /// <summary>
    /// Number of columns, which will be labeled A-Z.
    /// </summary>
    private const int COLS = 26;

    /// <summary>
    /// Provides an easy way to convert from an index to a letter (0 -> A)
    /// </summary>
    private char[] Alphabet { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();


    /// <summary>
    ///   Gets or sets the name of the file to be saved
    /// </summary>
    private string FileSaveName { get; set; } = "Spreadsheet.sprd";


    /// <summary>
    ///   <para> Gets or sets the data for all of the cells in the spreadsheet GUI. </para>
    ///   <remarks>Backing Store for HTML</remarks>
    /// </summary>
    private string[,] CellsBackingStore { get; set; } = new string[ROWS, COLS];



    //ADDED FROM LECTURE
    // Member variables used to keep track of internal state, which as
    // which row/col is selected, and to control UI elements
    // See lecture for details
    private string SelectedCell = "A1";
    private string CurrentContents = string.Empty;
    private ElementReference TextArea;
    private int SelectedRow = 0;
    private int SelectedCol = 0;
    //Temporary
    private Spreadsheet ss = new();
    private object SelectedValue = "";

    //ADDED FROM LECTURE
    /// <summary>
    /// Handler for when a cell is clicked
    /// </summary>
    /// <param name="row">The row component of the cell's coordinates</param>
    /// <param name="col">The column component of the cell's coordinates</param>
    private void CellClicked( int row, int col )
    {
        SelectedCell = $"{Alphabet[col]}{row + 1}";
        SelectedRow = row;
        SelectedCol = col;
        CurrentContents = CellsBackingStore[row, col];
        SelectedValue = ss.GetCellValue(SelectedCell);
        TextArea.FocusAsync();
    }


    private async Task CellContentChanged(ChangeEventArgs e)
    {
        string data = e.Value!.ToString() ?? "";

        try
        {
            // Attempt to set the content in the spreadsheet
            ss.SetContentsOfCell(SelectedCell, data);

            // Recalculate the value of the selected cell
            SelectedValue = ss.GetCellValue(SelectedCell);

            // Update the current contents and backing store
            CellsBackingStore[SelectedRow, SelectedCol] = data;
            CurrentContents = data;

            // Refocus the text area
            await TextArea.FocusAsync();
        }
        catch (Exception ex)
        {
            // Handle formula or other evaluation errors
            await JSRuntime.InvokeVoidAsync("alert", $"Error in cell {SelectedCell}: {ex.Message}");
        }
    }





    /// <summary>
    /// Saves the current spreadsheet, by providing a download of a file
    /// containing the json representation of the spreadsheet.
    /// </summary>
    private async void SaveFile()
    {
        await JSRuntime.InvokeVoidAsync( "downloadFile", FileSaveName, 
            "replace this with the json representation of the current spreadsheet" );
    }

    /// <summary>
    /// This method will run when the file chooser is used, for loading a file.
    /// Uploads a file containing a json representation of a spreadsheet, and 
    /// replaces the current sheet with the loaded one.
    /// </summary>
    /// <param name="args">The event arguments, which contains the selected file name</param>
    private async void HandleFileChooser( EventArgs args )
    {
        try
        {
            string fileContent = string.Empty;

            InputFileChangeEventArgs eventArgs = args as InputFileChangeEventArgs ?? throw new Exception("unable to get file name");
            if ( eventArgs.FileCount == 1 )
            {
                var file = eventArgs.File;
                if ( file is null )
                {
                    return;
                }

                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);

                // fileContent will contain the contents of the loaded file
                fileContent = await reader.ReadToEndAsync();

                // TODO: Use the loaded fileContent to replace the current spreadsheet

                StateHasChanged();
            }
        }
        catch ( Exception e )
        {
            Debug.WriteLine( "an error occurred while loading the file..." + e );
        }
    }

}
