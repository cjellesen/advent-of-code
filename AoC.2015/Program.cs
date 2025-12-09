using AoC.Shared;

AoCHttpClient client = new("53616c7465645f5f38e25880c8c6f076bf367278aa8040ad6fcd7a359fbb84b070c3c264e4308106f99ab17f218ae527d19188a1de86434d196f629d47c1bed4");

var dayOne = new DayOne(client);
dayOne.Test();
await dayOne.RunPuzzle();

var dayTwo = new DayTwo(client);
dayTwo.Test();
await dayTwo.RunPuzzle();

var dayThree = new DayThree(client);
dayThree.Test();
await dayThree.RunPuzzle();
