using System.ComponentModel.DataAnnotations;

namespace DevDotNet.Models.FizzBuzz
{
    public class FizzBuzzModel
    {
        [Required(ErrorMessage = "Please enter a Fizz Value")]
        [Range(2, 20, ErrorMessage = "An integer between 2 and 20")]
        public int FizzValue { get; set; } = 3;

        [Required(ErrorMessage = "Please enter a Buzz Value")]
        [Range(3, 30, ErrorMessage = "An integer between 3 and 30")]
        public int BuzzValue { get; set; } = 5;

        [Required(ErrorMessage = "Please enter a Stop Value")]
        public int StopValue { get; set; }
    }
}
