using AoC.Shared;

AoCHttpClient client = new("");

var dayOne = new DayOne(client);
dayOne.Test();
await dayOne.RunPuzzle();

var dayTwo = new DayTwo(client);
dayTwo.Test();
await dayTwo.RunPuzzle();

var dayThree = new DayThree(client);
dayThree.Test();
await dayThree.RunPuzzle();
