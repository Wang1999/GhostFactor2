﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;
using Roslyn.Services;
using warnings.analyzer;
using warnings.refactoring;
using warnings.util;

namespace warnings.conditions
{
    internal partial class InlineMethodConditionCheckersList : RefactoringConditionsList
    {
        /* Singleton the list of conditions for inline method refactoring. */
        private static IRefactoringConditionsList instance;
        public static IRefactoringConditionsList GetInstance()
        {
            if(instance == null)
            {
                instance = new InlineMethodConditionCheckersList();
            }
            return instance;
        }

        protected override IEnumerable<IRefactoringConditionChecker> GetAllConditionCheckers()
        {
            var list = new List<IRefactoringConditionChecker>();
            
            // Add all implemented condition checkers here.
            list.Add(new ChangedVariableValuesChecker());
           // list.Add(new VariableNamesCollisionChecker());

            return list;
        }

        public override RefactoringType RefactoringType
        {
            get { return RefactoringType.INLINE_METHOD; }
        }

        /// <summary>
        /// Abstract class for checkers of inline method refactoring.
        /// </summary>
        private abstract class InlineMethodConditionsChecker : IRefactoringConditionChecker
        {
            public RefactoringType RefactoringType
            {
                get { return RefactoringType.INLINE_METHOD; }
            }

            public IConditionCheckingResult CheckCondition(ManualRefactoring input)
            {
                return CheckInlineMethodCondition((IInlineMethodRefactoring)input);
            }

            public abstract Predicate<SyntaxNode> GetIssuedNodeFilter();

            public abstract IConditionCheckingResult CheckInlineMethodCondition(IInlineMethodRefactoring 
                refactoring);

            public abstract RefactoringConditionType RefactoringConditionType { get; }
        }
    }
}
