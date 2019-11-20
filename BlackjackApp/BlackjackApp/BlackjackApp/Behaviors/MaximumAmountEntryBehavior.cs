using System;
using Xamarin.Forms;

namespace BlackjackApp.Behaviors
{
    public class MaximumAmountEntryBehavior : PlainNumericEntryBehavior
    {
        public int MaximumAmount { get; set; } = 100;

        public MaximumAmountEntryBehavior()
        {
            AdditionalCheck = (e, ot) =>
            {
                e.Text = Convert.ToInt32(e.Text) > MaximumAmount ? ot : e.Text.ToString();
            };
        }
        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);
        }

        protected override void TextChanged_Handler(object sender, TextChangedEventArgs e)
        {
            base.TextChanged_Handler(sender, e);
        }
    }
}
