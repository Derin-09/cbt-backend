using System;

namespace Backend.Models;

    public class TriviaResponse
{
    public List<TriviaQuestion> results { get; set; }
}

public class TriviaQuestion
{
    public string question { get; set; }
    public string correct_answer { get; set; }
    public List<string> incorrect_answers { get; set; }
}
