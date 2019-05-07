using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace ServicesProvider.Behaviors
{
    public class PasswordValidBehavior:Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += BindableOnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= BindableOnTextChanged;
        }

        private void BindableOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var password = e.NewTextValue;
            var pwEntry = sender as Entry;

            if (password.Length>6)
            {
                pwEntry.TextColor = Color.Black;
            }
            else
            {
                pwEntry.TextColor = Color.Red;
            }

        }
    }
}
