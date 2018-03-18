﻿using System.CodeDom;
using System.Collections.Generic;
using DslModel.Application;
using DslModel.Domain;
using DslModelToCSharp.Util;

namespace DslModelToCSharp.Application
{
    public class EventStoreBuilder
    {
        private readonly ClassBuilderUtil _classBuilderUtil;
        private readonly ConstructorBuilderUtil _constructorBuilderUtil;
        private readonly ListPropBuilderUtil _listPropBuilderUtil;
        private readonly string _nameSpace;
        private readonly NameSpaceBuilderUtil _nameSpaceBuilderUtil;
        private readonly PropertyBuilderUtil _propertyBuilderUtil;

        public EventStoreBuilder(string nameSpace)
        {
            _nameSpace = nameSpace;
            _nameSpaceBuilderUtil = new NameSpaceBuilderUtil();
            _classBuilderUtil = new ClassBuilderUtil();
            _propertyBuilderUtil = new PropertyBuilderUtil();
            _listPropBuilderUtil = new ListPropBuilderUtil();
            _constructorBuilderUtil = new ConstructorBuilderUtil();
        }

        public CodeNamespace Build(EventStore eventStore, IList<SynchronousDomainHook> hooks)
        {
            var targetClass = _classBuilderUtil.Build(eventStore.Name);
            _nameSpaceBuilderUtil.WithName(_nameSpace).WithDomain().WithTask().WithList().WithLinq();

            _propertyBuilderUtil.BuildWithoutSet(targetClass, eventStore.Properties);
            _listPropBuilderUtil.Build(targetClass, eventStore.ListProperties);
            var constructor = _constructorBuilderUtil.BuildPublic(eventStore.Properties);

            foreach (var hook in hooks)
            {
                var hookName = hook.Name + "Hook";
                _nameSpaceBuilderUtil.WithHookEntityNameSpace(hook.ClassType);
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(hookName, hookName));
                constructor.Statements.Add(new CodeSnippetExpression($"DomainHooks.Add({hookName})"));
            }

            targetClass.Members.Add(constructor);

            var methodList = new List<CodeMemberMethod>();
            foreach (var eventStoreMethod in eventStore.Methods)
            {
                var method = new CodeMemberMethod();
                method.Name = eventStoreMethod.Name;
                method.ReturnType = new CodeTypeReference(eventStoreMethod.ReturnType);
                method.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                foreach (var parameter in eventStoreMethod.Parameters)
                    method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(parameter.Type),
                        parameter.Name));

                methodList.Add(method);
            }

            var codeMemberMethod = methodList[0];
            codeMemberMethod.Statements.Add(new CodeSnippetExpression("var enumerator = domainEvents.GetEnumerator()"));

            var codeWhile = CreateLoopOverDomainEvents();

            codeMemberMethod.Statements.Add(codeWhile);
            codeMemberMethod.Statements.Add(
                new CodeSnippetExpression("await EventStoreRepository.AddEvents(domainEvents)"));
            codeMemberMethod.Statements.Add(
                new CodeSnippetExpression($"return {new HookResultBaseClass().Name}.OkResult()"));

            targetClass.Members.Add(methodList[0]);

            var nameSpace = _nameSpaceBuilderUtil.Build();
            nameSpace.Types.Add(targetClass);
            return nameSpace;
        }

        private static CodeIterationStatement CreateLoopOverDomainEvents()
        {
            var codeWhile = new CodeIterationStatement();
            codeWhile.IncrementStatement = new CodeSnippetStatement("");
            codeWhile.InitStatement = new CodeSnippetStatement("");

            codeWhile.TestExpression = new CodeSnippetExpression("enumerator.MoveNext()");
            codeWhile.Statements.Add(new CodeSnippetExpression("var domainEvent = enumerator.Current"));
            codeWhile.Statements.Add(new CodeSnippetExpression(
                "var domainHooks = DomainHooks.Where(hook => hook.EventType == domainEvent.GetType())"));
            codeWhile.Statements.Add(new CodeSnippetExpression("var enumeratorHook = domainHooks.GetEnumerator()"));

            var codeInnerWhile = CreateLoopOverHooks();

            codeWhile.Statements.Add(codeInnerWhile);
            return codeWhile;
        }

        private static CodeIterationStatement CreateLoopOverHooks()
        {
            var codeInnerWhile = new CodeIterationStatement();
            codeInnerWhile.TestExpression = new CodeSnippetExpression("enumeratorHook.MoveNext()");
            codeInnerWhile.IncrementStatement = new CodeSnippetStatement("");
            codeInnerWhile.InitStatement = new CodeSnippetStatement("");
            codeInnerWhile.Statements.Add(new CodeSnippetExpression("var domainHook = enumeratorHook.Current"));
            codeInnerWhile.Statements.Add(new CodeSnippetExpression(
                $"var validationResult = domainHook.{new DomainHookBaseClass().Methods[0].Name}(domainEvent)"));

            codeInnerWhile.Statements.Add(CreateIfState());
            return codeInnerWhile;
        }

        private static CodeConditionStatement CreateIfState()
        {
            var conditionalStatement = new CodeConditionStatement(
                new CodeSnippetExpression("!validationResult.Ok"),
                new CodeExpressionStatement(new CodeSnippetExpression("return validationResult")));
            return conditionalStatement;
        }
    }
}