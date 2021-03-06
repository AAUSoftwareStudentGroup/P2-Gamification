﻿using System;
using System.Linq;
using System.Text;
using ThreeOneSevenBee.Model.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreeOneSevenBee.Model.Expression;

namespace ThreeOneSevenBee.ModelTests
{
    [TestClass]
    public class QueueStackandCloneTests
    {

        [TestMethod]
        public void StackConstruct()
        {
            var stack = new Stack<int>();
            Assert.IsInstanceOfType(stack, typeof(Stack<int>), "Stack Constructed");
        }
        [TestMethod]
        public void StackCount()
        {
            var stack = new Stack<int>();
            stack.Push(2);
            stack.Push(3);
            Assert.AreEqual(2, stack.Count());
        }

        [TestMethod]
        public void QueueCount()
        {
            var queue = new Queue<int>();
            queue.Enqueue(2);
            queue.Enqueue(3);
            Assert.AreEqual(2, queue.Count());
        }

        [TestMethod]
        public void StackPushAndPop()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(3);
            stack.Push(5);
            Assert.AreEqual(5, stack.Pop());
            Assert.AreEqual(3, stack.Pop());
            Assert.AreEqual(1, stack.Pop());
            try
            {
                stack.Pop();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {

            }
        }

        [TestMethod]
        public void QueueAndDeque()
        {
            var queue = new Queue<int>();
            queue.Enqueue(2);
            queue.Enqueue(5);
            Assert.AreEqual(2, queue.Dequeue());
            Assert.AreEqual(5, queue.Dequeue());
            try
            {
                queue.Dequeue();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {

            }
        }

        [TestMethod]
        public void CloneTest()
        {
            var parser = new ExpressionSerializer();
            var expressionTree = parser.Deserialize("a*b");
            var complexExpressionTree = parser.Deserialize("sqrt(pi^2)+2-5*(3-2)+pi");
            var crazyComplexExpressionTree = parser.Deserialize("sqrt(pi^2)+(-5)*2^6/--2*sqrt(2*5^5)");
            var cloneExpressionTree = expressionTree.Clone();
            var cloneComplexExpressionTree = complexExpressionTree.Clone();
            var cloneCrazyComplexExpressionTree = crazyComplexExpressionTree.Clone();

            Assert.IsTrue(expressionTree.Equals(cloneExpressionTree));
            Assert.IsTrue(expressionTree == cloneExpressionTree);

            Assert.IsTrue(complexExpressionTree.Equals(cloneComplexExpressionTree));
            Assert.IsTrue(complexExpressionTree == cloneComplexExpressionTree);

            Assert.IsTrue(crazyComplexExpressionTree == cloneCrazyComplexExpressionTree);
            Assert.IsTrue(crazyComplexExpressionTree.Equals(cloneCrazyComplexExpressionTree));
        }

    }
}
