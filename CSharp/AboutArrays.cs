using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace DotNetKoans.CSharp
{
    public class AboutArrays : Koan
    {
        [Koan(1)]
        public void CreatingArrays()
        {
            var empty_array = new object[] { };

            //I guess this is saying that arrays are types of objects?
            Assert.Equal(typeof(object[]), empty_array.GetType());

            //Note that you have to explicitly check for subclasses
            Assert.True(typeof(Array).IsAssignableFrom(empty_array.GetType()));

            //this is asking me if the length of the new empty array === 0.
            Assert.Equal(0, empty_array.Length);
        }

        [Koan(2)]
        public void ArrayLiterals()
        {
            //You don't have to specify a type if the arguments can be inferred
            var array = new [] { 42 };
            Assert.Equal(typeof(int[]), array.GetType());
            Assert.Equal(new int[] { 42 }, array);

            //Are arrays 0-based or 1-based?
            Assert.Equal(42, array[((int)0)]);

            //This is important because...
            Assert.True(array.IsFixedSize);

            //...it means we can't do this: array[1] = 13;
            Assert.Throws(typeof(IndexOutOfRangeException), delegate() { array[1] = 13; });
            //^^ this 'IndexOutOfRange' Exception means that '13' is outside the boundries of our new array, which only has array[0] at 42.
            //remember, arrays are zero indexed!


            //This is because the array is fixed at length 1. You could write a function
            //which created a new array bigger than the last, copied the elements over, and
            //returned the new array. Or you could do this:
            List<int> dynamicArray = new List<int>();
            dynamicArray.Add(42);
            Assert.Equal(array, dynamicArray.ToArray());

            dynamicArray.Add(13);
            Assert.Equal((new int[] { 42, (int)13}), dynamicArray.ToArray());
        }

        [Koan(3)]
        public void AccessingArrayElements()
        {
            var array = new[] { "peanut", "butter", "and", "jelly" };

            Assert.Equal("peanut", array[0]);
            Assert.Equal("jelly", array[3]);
            
            //This doesn't work: Assert.Equal(FILL_ME_IN, array[-1]);
        }

        [Koan(4)]
        public void SlicingArrays()
        {
            var array = new[] { "peanut", "butter", "and", "jelly" };

            //this is array.Take-ing the first(2) from the array
			Assert.Equal(new string[] { (string)"peanut", (string)"butter" }, array.Take(2).ToArray());
            //this is array.Skip-ping the (1) element, then array.Take -ing (2) elements.
			Assert.Equal(new string[] { (string)"butter", (string)"and" }, array.Skip(1).Take(2).ToArray());
        }

        [Koan(5)]
        public void PushingAndPopping()
        {
            //this declares the new array
            var array = new[] { 1, 2 };

            //I have no idea what Stack means yet. I think. But were putting the array in the "Stack"
            Stack stack = new Stack(array);
            
            //now we're pushing "last" into stack
            stack.Push("last");

            //And checking if stack.ToArray() === the array we just pushed into, including the value ("last") we just pushed.
            Assert.Equal(new object[] { (string)"last", (int)2, (int)1 }, stack.ToArray());

            //this assigns a new variable 'poppedValue'
            var poppedValue = stack.Pop();

            //checking if (string)"last" and poppedValue are the same thing. .Pop() returns the value of the popped thing!
            Assert.Equal((string)"last", poppedValue);

            //checking if the array is now back to it's original state since we've popped out of it.
            Assert.Equal(new object[] { (int)2, (int)1 }, stack.ToArray());
        }

        [Koan(6)]
        public void Shifting()
        {
            //Shift == Remove First Element
            //Unshift == Insert Element at Beginning
            //C# doesn't provide this natively. You have a couple
            //of options, but we'll use the LinkedList<T> to implement
            var array = new[] { "Hello", "World" };
            var list = new LinkedList<string>(array);


            //here we're adding "Say" to the front of the array at offset (index) 0. We use list.AddFirst(thingWeWantToAdd) to do this
            list.AddFirst("Say");
            Assert.Equal(new[] {"Say", "Hello", "World" }, list.ToArray());


            //now we're removing the last index, "World", from the array. Uses list.RemoveLast()
            list.RemoveLast();
            Assert.Equal(new[] {"Say", "Hello" }, list.ToArray());


            //then we take away  "Say" with list.RemoveFirst()
            list.RemoveFirst();
            Assert.Equal(new[] { "Hello" }, list.ToArray());

            //now we add back in "World" using list.AddAfter(AFTER list.Find - ing "Hello" and it's respective index).
            list.AddAfter(list.Find("Hello"), "World");
            Assert.Equal(new[] {"Hello", "World" }, list.ToArray());
        }

    }
}
