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

## 📁 Project Structure
```bash
Spreadsheet/
├── Formula/                  # Formula parsing and evaluation logic
├── DependencyGraph/         # Handles dependency relations among spreadsheet cells
├── Spreadsheet/             # Core spreadsheet model and integration logic
├── GUI/                     # Blazor frontend (Razor components and layout)
├── *Tests/                  # Unit test projects for each module
```

## 🚀 Getting Started
TODO

## 📄 License
This project is licensed under the MIT License. See the LICENSE file for details.

## 👤 Author
hdoppelt
