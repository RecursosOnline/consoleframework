﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Binding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1.Binding
{
    [TestClass]
    public class SimplePropertiesTest
    {
        class TargetClass
        {
            public String Title { get; set; }
            public int TargetInt { get; set; }
            public String TargetStr { get; set; }
        }

        class SourceClass : INotifyPropertyChanged
        {
            public String Text
            {
                get { return text; }
                set
                {
                    if (value != text)
                    {
                        text = value;
                        raisePropertyChanged("Text");
                    }
                }
            }

            public String SourceStr
            {
                get { return sourceStr; }
                set
                {
                    if (value != sourceStr)
                    {
                        sourceStr = value;
                        raisePropertyChanged("SourceStr");
                    }
                }
            }

            public int SourceInt
            {
                get { return sourceInt; }
                set
                {
                    if (value != sourceInt)
                    {
                        sourceInt = value;
                        raisePropertyChanged("SourceInt");
                    }
                }
            }

            private void raisePropertyChanged(String propertyName)
            {
                foreach (IPropertyChangedListener listener in listeners)
                {
                    listener.propertyChanged(propertyName);
                }
            }

            private List<IPropertyChangedListener> listeners = new List<IPropertyChangedListener>();
            private string text;
            private string sourceStr;
            private int sourceInt;

            public void addPropertyChangedListener(IPropertyChangedListener listener)
            {
                listeners.Add(listener);
            }

            public void removePropertyChangedListener(IPropertyChangedListener listener)
            {
                listeners.Remove(listener);
            }
        }

        [TestMethod]
        public void TestString()
        {
            SourceClass source = new SourceClass();
            TargetClass target = new TargetClass();
            BindingBase binding = new BindingBase(target, "Title", source, "Text", BindingMode.OneWay);
            binding.bind();
            source.Text = "Text!";
            Assert.AreEqual( target.Title, source.Text );
        }

        [TestMethod]
        public void TestConversion( ) {
            SourceClass source = new SourceClass();
            TargetClass target = new TargetClass();
            BindingBase binding = new BindingBase(target, "TargetInt", source, "SourceStr", BindingMode.OneWay);
            BindingBase binding2 = new BindingBase(target, "TargetStr", source, "SourceInt", BindingMode.OneWay);
            binding.bind();
            binding2.bind(  );
            source.SourceInt = 5;
            source.SourceStr = "4";
            Assert.AreEqual(target.TargetInt, 4);
            Assert.AreEqual(target.TargetStr, "5");
        }

        [TestMethod]
        public void TestValidation( ) {
            SourceClass source = new SourceClass();
            TargetClass target = new TargetClass();
            BindingBase binding = new BindingBase(target, "TargetStr", source, "SourceInt", BindingMode.OneWay);
            binding.UpdateSourceIfBindingFails = false;
            binding.bind();
            target.TargetInt = 1;
            source.SourceStr = "invalid int";
            Assert.IsTrue( target.TargetInt == 1 );
        }
    }
}