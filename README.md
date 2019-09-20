[![Build Status](https://dev.azure.com/AzureDisaster/Wheel%20Of%20Fortune/_apis/build/status/azureDisaster.wheel_of_fortune?branchName=master)](https://dev.azure.com/AzureDisaster/Wheel%20Of%20Fortune/_build/latest?definitionId=1&branchName=master)

# Wheel of Fortune
A basic wheel of fortune console app! Version 2.0!

## Getting Started
Fork this repo and clone it locally. Open the file called 'WheelOfFortune.sln' in Visual Studio. Once you click run, the game will begin.

For reference, the hardcoded phrases are located in WOFClassLib.Tests/Phrase.cs

Once you begin you will be prompted for the number of users, and their names. 
You can attempt to guess a letter, but in true Wheel of Fortune fashion you may never attempt to guess a phrase before guessing a letter first. The spelling must be exact, but the game is case insensitive. 
Once someone guesses the phrase or last letter the game ends.

### Prerequisites
[Visual Studio](https://visualstudio.microsoft.com/vs/)

## Running the tests
The tests are all located in 'WOFClassLib.Tests'. Make sure you build prior to running the tests.
```
Build ---> Build Solution
```

Once built, you can run the tests:
```
Test ---> Windows ----> Test Explorer ---> WOFClassLibTests ----> Run all tests (green, upper left)
```

## Authors

* **Denzale Reeze** - *Puzzle class, & associated tests* - [denzalereese](https://github.com/denzalereese)
* **Diane Kato** - *Player class, roasts & associated tests* - [dnkato](https://github.com/dnkato)
* **Du Tram** - *Game class, exception handling & bug fixes* - [dtram52](https://github.com/dtram52)
* **Lisette Hamilton** - *Game class, documentation, & bug fixes* - [lphamilton](https://github.com/lphamilton)

## Acknowledgments

* Hat tip to the [LEAP Apprentiship Program](http://www.industryexplorers.com/)!

