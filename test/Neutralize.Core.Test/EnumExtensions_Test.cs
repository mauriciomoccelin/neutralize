using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Neutralize.Application;
using Neutralize.Extensions;
using Xunit;

namespace Neutralize.Core.Test
{
    public class EnumExtensions_Test
    {
        [Trait("Category", "Core - EnumExtensions")]
        [Fact(DisplayName = "Get description from enum using EnumExtensions")]
        public void EnumExtensions_GetDescription_WithSuccess()
        {
            // Arrange
            var itens = new Collection<SelectionDto<int>>();

            // Act
            foreach (UserTypes item in Enum.GetValues(typeof(UserTypes)))
            {
                itens.Add(
                    new SelectionDto<int>
                    {
                        Value = (int)item,
                        Text = item.GetDescription()
                    }
                );
            }

            // Assert
            Assert.True(itens.All(e => !string.IsNullOrWhiteSpace(e.Text)));
        }
    }

    public enum UserTypes
    {
        [Description("Administrator")]
        A = 1,
        [Description("Basic")]
        B = 2,
        [Description("Customer")]
        C = 3
    }
}