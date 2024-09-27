using Tiktoken;

namespace ChatGPT_Splitter_Blazor_New.Bll;

public static class TextProcessingService
{

    public static int CalculateTokenCount(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            var encoding = ModelToEncoder.For("gpt-4o");
            return encoding.CountTokens(text);
        }
        else
        {
            return 0;
        }
    }

    public static int CalculateNumberOfChunks(int tokenCount, int chunkSize)
    {
        if (chunkSize > 0)
        {
            return (int)Math.Ceiling((double)tokenCount / chunkSize);
        }

        return 0;
    }


    public static List<int> FindBreakPoints(string text)
    {
        // Aggiungi indice 0 come punto di inizio
        List<int> breakPoints = new List<int> { 0 };

        // Trova tutti i nuovi paragrafi o altri indicatori di spezzatura
        for (int i = 0; i < text.Length; i++)
        {
            // Esempio: Usa il cambio di paragrafo come punto di spezzatura
            if (text[i] == '\n' && i < text.Length - 1 && text[i + 1] == '\n')
            {
                breakPoints.Add(i);
            }
            // Puoi aggiungere altre condizioni per titoli di capitoli, ecc.
        }
        // Aggiungi la fine del testo come ultimo punto di spezzatura
        breakPoints.Add(text.Length);
        return breakPoints;
    }

    public static List<string> SplitTextEqually(string text, int numberOfChunks, List<int> breakPoints)
    {
        var chunks = new List<string>();
        int approximateChunkSize = text.Length / numberOfChunks;

        int startIndex = 0;
        for (int i = 0; i < numberOfChunks; i++)
        {
            int endIndex = startIndex + approximateChunkSize;

            // Trova il punto di spezzatura più vicino
            while (!breakPoints.Contains(endIndex) && endIndex < text.Length)
            {
                endIndex++;
            }

            endIndex = Math.Min(endIndex, text.Length); // Assicurati che endIndex non superi la lunghezza del testo

            chunks.Add(text.Substring(startIndex, endIndex - startIndex));
            startIndex = endIndex;
        }

        return chunks;
    }
}