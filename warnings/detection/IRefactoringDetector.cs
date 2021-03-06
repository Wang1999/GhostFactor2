﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roslyn.Compilers.CSharp;
using Roslyn.Services;
using warnings.conditions;
using warnings.detection;
using warnings.source;
using warnings.refactoring;

namespace warnings.refactoring.detection
{
    public interface IRefactoringDetector
    {
        Boolean HasRefactoring();
        IEnumerable<ManualRefactoring> GetRefactorings();
    }

    public interface IBeforeAndAfterSourceKeeper
    {
        void SetSourceBefore(String source);
        string GetSourceBefore();
        void SetSourceAfter(String source);
        string GetSourceAfter();
    }

    public interface IBeforeAndAfterSyntaxNodeKeeper
    {
        void SetSyntaxNodeBefore(SyntaxNode before);
        void SetSyntaxNodeAfter(SyntaxNode after);
    }

    public interface IBeforeAndAfterDocumentKeeper
    {
        void SetDocumentBefore(IDocument docBefore);
        void SetDocumentAfter(IDocument docAfter);
    }

    public interface IExternalRefactoringDetector : IRefactoringDetector, 
        IBeforeAndAfterSourceKeeper, IHasRefactoringType
    {
    }

    internal interface IInternalRefactoringDetector : IRefactoringDetector, 
        IBeforeAndAfterSyntaxNodeKeeper
    {
        
    }


    public static class RefactoringDetectorFactory
    {
        public static IExternalRefactoringDetector GetRefactoringDetectorByType
            (RefactoringType type)
        {
            switch (type)
            {
                case RefactoringType.RENAME:
                    return new RenameDetector();
                case RefactoringType.INLINE_METHOD:
                    return new InlineMethodDetector(InMethodInlineDetectorFactory.GetInlineDetectorByStatement());
                case RefactoringType.EXTRACT_METHOD:
                    return new ExtractMethodDetector(InMethodExtractMethodDetectorFactory.
                        GetInMethodExtractMethodDetectorWithoutInvocation());
                case RefactoringType.CHANGE_METHOD_SIGNATURE:
                    return new ChangeMethodSignatureDetector();
                default:
                    throw new Exception("Unknown refactoring type.");
            }
        }

        public static IExternalRefactoringDetector GetDummyRefactoringDetectorByType
            (RefactoringType type)
        {
            switch (type)
            {
                case RefactoringType.EXTRACT_METHOD:
                    return new SimpleExtractMethodDetector();
                case RefactoringType.INLINE_METHOD:
                    return new InlineMethodDetector(InMethodInlineDetectorFactory.
                        GetDummyDetector());
                default:
                    throw new Exception("Unsupported refactoring type.");
            }
        }
    }
}
