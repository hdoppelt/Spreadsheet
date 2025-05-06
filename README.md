# Spreadsheet

A modular spreadsheet application built using C#, .NET 8, and Blazor, supporting real-time formula evaluation, dependency tracking, and an interactive grid-based UI.

## Features
- Formula Parser: Supports arithmetic expressions with operator precedence and parenthesis handling (e.g., =A1+B2*C3).
- Dependency Graph: Dynamically tracks cell relationships to prevent cycles and update dependent cells efficiently.
- Blazor GUI: Interactive grid UI built with Razor components and two-way data binding.
- Unit Tested: Core logic is rigorously tested using MSTest.
- Modular Design: Divided into independent projects (Formula, DependencyGraph, Spreadsheet, GUI).

## 🛠️ Technologies Used
- .NET 8
- Blazor
- C#
- MSTest (for unit testing)

## 🚀 Getting Started
**Prerequisites**
- .NET 8 SDK
- Visual Studio 2022 or later

**Build & Run**
1. Clone the repository:
```bash
https://github.com/hdoppelt/Sprite-Editor.git
```
2. Open the Spreadsheet.sln solution file in Visual Studio.
3. Set the GUI project as the Startup Project.
4. Build and run the project within Visual Studio.

## 📄 License
This project is licensed under the MIT License. See the LICENSE file for details.

## 👤 Author
hdoppelt
