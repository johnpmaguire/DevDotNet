using DevDotNet.Models.FizzBuzz;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Reflection;

namespace DevDotNet.Componets.Validators.FizzBuzz
{
    public class FizzBuzzValidator : ComponentBase
    {
        private ValidationMessageStore validationMessageStore;

        [CascadingParameter]
        private EditContext CurrentEditContext { get; set; }

        protected override void OnInitialized()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException("EditContext is not set. Ensure this component is used within an EditForm.");
            }

            validationMessageStore = new ValidationMessageStore(CurrentEditContext);

            CurrentEditContext.OnFieldChanged += OnFieldChanged;
            CurrentEditContext.OnValidationRequested += OnValidationRequested;
        }

        private void OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            ValidateField(e.FieldIdentifier);
        }

        private void OnValidationRequested(object sender, ValidationRequestedEventArgs e)
        {
            validationMessageStore.Clear();

            if (CurrentEditContext.Model is FizzBuzzModel model)
            {
                ValidateField(new FieldIdentifier(model, nameof(FizzBuzzModel.FizzValue)));
                ValidateField(new FieldIdentifier(model, nameof(FizzBuzzModel.BuzzValue)));
                ValidateField(new FieldIdentifier(model, nameof(FizzBuzzModel.StopValue)));
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }

        private void ValidateField(FieldIdentifier fieldIdentifier)
        {
            var fizzBuzz = CurrentEditContext.Model as FizzBuzzModel
                ?? throw new InvalidOperationException("Model must be of type FizzBuzzModel");

            validationMessageStore.Clear(fieldIdentifier);

            switch (fieldIdentifier.FieldName)
            {
                case nameof(FizzBuzzModel.FizzValue):
                    ValidateFizzValue(fieldIdentifier, fizzBuzz);

                    if (fizzBuzz.BuzzValue <= fizzBuzz.FizzValue)
                    {
                        fizzBuzz.BuzzValue = fizzBuzz.FizzValue + 1;
                        validationMessageStore.Clear(fieldIdentifier);
                    }
                        fizzBuzz.StopValue = fizzBuzz.FizzValue * fizzBuzz.BuzzValue;

                    break;

                case nameof(FizzBuzzModel.BuzzValue):
                    ValidateBuzzValue(fieldIdentifier, fizzBuzz);
                    if (fizzBuzz.BuzzValue <= fizzBuzz.FizzValue)
                    {
                        fizzBuzz.FizzValue = fizzBuzz.BuzzValue - 1;
                        validationMessageStore.Clear(fieldIdentifier);
                    }
                        fizzBuzz.StopValue = fizzBuzz.FizzValue * fizzBuzz.BuzzValue;
                    break;

                case nameof(FizzBuzzModel.StopValue):
                    ValidateStopValue(fieldIdentifier, fizzBuzz);
                    break;
            }
        }

        private void ValidateFizzValue(FieldIdentifier fieldIdentifier, FizzBuzzModel fizzBuzz)
        {
            if (fizzBuzz.FizzValue >= fizzBuzz.BuzzValue)
            {
                validationMessageStore.Add(fieldIdentifier, "Fizz Value must be less than the Buzz Value.");
            }
        }

        private void ValidateBuzzValue(FieldIdentifier fieldIdentifier, FizzBuzzModel fizzBuzz)
        {
            if (fizzBuzz.BuzzValue <= fizzBuzz.FizzValue)
            {
                validationMessageStore.Add(fieldIdentifier, "Buzz Value must be more than the Fizz Value.");
            }
        }

        private void ValidateStopValue(FieldIdentifier fieldIdentifier, FizzBuzzModel fizzBuzz)
        {
            int minStopValue = fizzBuzz.FizzValue * fizzBuzz.BuzzValue;
            int maxStopValue = fizzBuzz.FizzValue * fizzBuzz.BuzzValue * 5;

            if (fizzBuzz.StopValue < minStopValue || fizzBuzz.StopValue > maxStopValue)
            {
                validationMessageStore.Add(fieldIdentifier, $"Stop Value must be between {minStopValue} and {maxStopValue}.");
            }
        }
    }
}
