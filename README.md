# 🃏 Texas Hold'em Poker Tournament Game

[![C#](https://img.shields.io/badge/C%23-8.0-239120?style=flat&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![WPF](https://img.shields.io/badge/WPF-.NET%20Framework-512BD4?style=flat&logo=dotnet)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A professional Texas Hold'em poker tournament game with intelligent AI opponents, built with WPF and MVVM architecture.

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Screenshots](#-screenshots)
- [How to Play](#-how-to-play)
- [Installation](#-installation)
- [Game Structure](#-game-structure)
- [Architecture](#-architecture)
- [Design Patterns](#-design-patterns)
- [Programming Principles](#-programming-principles)
- [Refactoring Techniques](#-refactoring-techniques)
- [Project Structure](#-project-structure)
- [Technologies Used](#-technologies-used)
- [Future Improvements](#-future-improvements)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Overview

Texas Hold'em Poker Tournament Game is a complete poker simulation where you compete against 4 AI bots in tournament-style gameplay.

### Key Statistics

- Codebase: ~3,000+ lines of C# code  
- AI Strategies: 4 unique bot playing styles  
- Tournaments: 4 difficulty levels  
- Design Patterns: 3 implemented patterns  
- Technical Debt: Minimal  

---

## ✨ Features

### 🎮 Core Gameplay

- Full Poker Cycle (Pre-flop → Showdown)  
- Realistic Betting System  
- Automatic Hand Evaluation  
- Split Pot Handling  

### 🤖 AI Opponents

| Strategy | Behavior | Difficulty |
|----------|----------|------------|
| Aggressive | Frequent raises, bluffs | Medium |
| Conservative | Strong hands only | Hard |
| Tight | Very selective | Expert |
| Loose | Unpredictable | Easy |

### 🏆 Tournament System

| Tournament | Buy-In | Prize Pool | Big Blind |
|------------|--------|------------|-----------|
| Paris | $10,000 | $30,000 | $200 |
| Rio de Janeiro | $75,000 | $250,000 | $1,500 |
| Sydney | $500,000 | $1,500,000 | $10,000 |
| Tokyo | $3,000,000 | $10,000,000 | $60,000 |

### 💾 Persistence

- AES-256 encrypted balance  
- Auto-save system  

### 🎨 UI

- Card animations  
- Dealer/blinds indicators  
- Responsive controls  

---

## 🖼️ Screenshots

| Main Menu | Game Table | Win Screen |
|----------|------------|-----------|
| ![](https://via.placeholder.com/300x200) | ![](https://via.placeholder.com/300x200) | ![](https://via.placeholder.com/300x200) |

---

## 🎲 How to Play

1. Pre-flop – get 2 cards, bet  
2. Flop – 3 cards, bet  
3. Turn – 4th card  
4. River – final card  
5. Showdown – best hand wins  

### Hand Rankings

1. Royal Flush  
2. Straight Flush  
3. Four of a Kind  
4. Full House  
5. Flush  
6. Straight  
7. Three of a Kind  
8. Two Pair  
9. One Pair  
10. High Card  

---

## 💻 Installation

### Requirements

- Windows 10/11  
- .NET Framework 4.7.2+  
- Visual Studio 2019+  

### Setup

```bash
git clone https://github.com/YOUR_USERNAME/TexasHoldemWPF.git
cd TexasHoldemWPF
```

Open solution:

```bash
start TexasHoldemWPF.sln
```

Build:

```bash
msbuild TexasHoldemWPF.sln /p:Configuration=Release
```

Run:

```bash
bin\Release\TexasHoldemWPF.exe
```

---

## 🏗️ Game Structure

```
START → PRE-FLOP → FLOP → TURN → RIVER → SHOWDOWN → REPEAT
```

---

## 🏛️ Architecture

The project follows MVVM:

- **View** – XAML UI  
- **ViewModel** – UI logic  
- **Model** – game logic  

---

## 🔧 Design Patterns

- Strategy Pattern (bot AI)  
- State Pattern (game flow)  
- Factory Pattern (player creation)  

---

## 📐 Programming Principles

- SOLID principles  
- DRY  
- Separation of concerns  

---

## ♻️ Refactoring Techniques

- Extract Method  
- Extract Class  
- Replace Conditional with Polymorphism  
- Introduce Parameter Object  

---

## 📁 Project Structure

```
TexasHoldemWPF/
│
├── Models/
├── ViewModels/
├── Views/
├── Enums/
├── Resources/
├── Services/
│
├── App.xaml
├── MainWindow.xaml
└── README.md
```

---

## 🛠️ Technologies Used

- C# 8.0  
- WPF (.NET Framework 4.7.2)  
- MVVM  
- LINQ  
- AES-256  

---

## 🚀 Future Improvements

- Multiplayer  
- Leaderboards  
- More AI strategies  
- Sound & animations  
- Mobile version  

---

## 🤝 Contributing

1. Fork the repo  
2. Create branch  
3. Commit changes  
4. Push  
5. Open Pull Request  

---

## 📄 License

MIT License
