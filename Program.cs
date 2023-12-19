using System.Text.RegularExpressions;

List<ScratchCard> scratchcards = new List<ScratchCard>();
string file = @"C:\Users\SaLiVa\source\repos\AdventOfCode2023Day4\Day4Input.txt";

using (StreamReader reader = File.OpenText(file))
{
    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        if(line != null)
            scratchcards.Add(new ScratchCard(line));
    }
    
    Console.WriteLine("Number of Cards at start: " + scratchcards.Count);

    var totalcards = scratchcards.Count;
    for(int c = 0; c < scratchcards.Count; c++)
    {
        var numberofcopies = scratchcards[c].CalculateCopies();

        if (numberofcopies > 0)
        {
            for (int i = 0; i < numberofcopies; i++)
            {
                var newScratchcardIndex = scratchcards[c].CardID + 1 + i;
                scratchcards.Add(new ScratchCard(scratchcards.First(x => x.CardID == newScratchcardIndex)));
            }

        }
    }
    
    Console.WriteLine(scratchcards.Count);
}

public class ScratchCard : IComparable<ScratchCard>
{
    public ScratchCard(string line)
    {
        string cardIDString = Regex.Split(line.Substring(line.IndexOf('d') + 1, line.IndexOf(':') - line.IndexOf('d') - 1), @"\s+").Last();
        string[] cardWinningNumberString = Regex.Split(line.Substring(line.IndexOf(":") + 1, line.IndexOf('|') - line.IndexOf(":") - 1), @"\s+");
        string[] cardPlayingNumberString = Regex.Split(line.Substring(line.IndexOf('|') + 1, line.Length - line.IndexOf('|') - 1), @"\s+");

        CardID = Int32.Parse(cardIDString);

        WinningNumbers = cardWinningNumberString.Where(x => x != String.Empty).Select(int.Parse).ToList();
        CardNumbers = cardPlayingNumberString.Where(x => x != String.Empty).Select(int.Parse).ToList();
    }

    public ScratchCard(ScratchCard card)
    {
        CardID = card.CardID;
        WinningNumbers = card.WinningNumbers;
        CardNumbers = card.CardNumbers;
    }

    public int CardID { get; set; }
    public List<int> WinningNumbers { get; set; }
    public List<int> CardNumbers { get; set; }

    public int CalculateCardPoints()
    {
        var matchingcards = CardNumbers.Intersect(WinningNumbers);
        if (matchingcards != null)
        {
            if (matchingcards.Count() == 1)
                return 1;
            if (matchingcards.Count() > 1)
            {
                int total = 1;
                for(int i =0; i < matchingcards.Count() - 1; i++)
                    total = 2 * total;
                return total;
            }
            else return 0;
        }
        else
            return 0;
    }
    
    public int CalculateCopies()
    {
        var matchingcards = CardNumbers.Intersect(WinningNumbers);
        
        return matchingcards.Count();
    }

    public int CompareTo(ScratchCard compareCard)
    {
        return compareCard == null ? 1 : this.CardID.CompareTo(compareCard.CardID);
    }
}