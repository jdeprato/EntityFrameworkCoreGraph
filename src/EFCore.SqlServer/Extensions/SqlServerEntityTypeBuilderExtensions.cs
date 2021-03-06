// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    ///     SQL Server specific extension methods for <see cref="EntityTypeBuilder" />.
    /// </summary>
    public static class SqlServerEntityTypeBuilderExtensions
    {
        /// <summary>
        ///     Configures the table that the entity maps to when targeting SQL Server as memory-optimized.
        /// </summary>
        /// <param name="entityTypeBuilder"> The builder for the entity type being configured. </param>
        /// <param name="memoryOptimized"> A value indicating whether the table is memory-optimized. </param>
        /// <returns> The same builder instance so that multiple calls can be chained. </returns>
        public static EntityTypeBuilder ForSqlServerIsMemoryOptimized(
            [NotNull] this EntityTypeBuilder entityTypeBuilder, bool memoryOptimized = true)
        {
            Check.NotNull(entityTypeBuilder, nameof(entityTypeBuilder));

            entityTypeBuilder.Metadata.SqlServer().IsMemoryOptimized = memoryOptimized;

            return entityTypeBuilder;
        }

        /// <summary>
        ///     Configures the table that the entity maps to when targeting SQL Server as memory-optimized.
        /// </summary>
        /// <typeparam name="TEntity"> The entity type being configured. </typeparam>
        /// <param name="entityTypeBuilder"> The builder for the entity type being configured. </param>
        /// <param name="memoryOptimized"> A value indicating whether the table is memory-optimized. </param>
        /// <returns> The same builder instance so that multiple calls can be chained. </returns>
        public static EntityTypeBuilder<TEntity> ForSqlServerIsMemoryOptimized<TEntity>(
            [NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, bool memoryOptimized = true)
            where TEntity : class
            => (EntityTypeBuilder<TEntity>)ForSqlServerIsMemoryOptimized((EntityTypeBuilder)entityTypeBuilder, memoryOptimized);

        /// <summary>
        ///     Configures an index on the specified properties. If there is an existing index on the given
        ///     set of properties, then the existing index will be returned for configuration.
        /// </summary>
        /// <typeparam name="TEntity"> The entity type being configured. </typeparam>
        /// <param name="entityTypeBuilder"> The builder for the entity type being configured. </param>
        /// <param name="indexExpression">
        ///     <para>
        ///         A lambda expression representing the property(s) to be included in the index
        ///         (<c>blog => blog.Url</c>).
        ///     </para>
        ///     <para>
        ///         If the index is made up of multiple properties then specify an anonymous type including the
        ///         properties (<c>post => new { post.Title, post.BlogId }</c>).
        ///     </para>
        /// </param>
        /// <returns> An object that can be used to configure the index. </returns>
        public static IndexBuilder<TEntity> ForSqlServerHasIndex<TEntity>(
            [NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder, [NotNull] Expression<Func<TEntity, object>> indexExpression)
            where TEntity : class
        {
            Check.NotNull(entityTypeBuilder, nameof(entityTypeBuilder));
            Check.NotNull(indexExpression, nameof(indexExpression));

            var builder = ((IInfrastructure<InternalEntityTypeBuilder>)entityTypeBuilder).GetInfrastructure();

            return new IndexBuilder<TEntity>(
                builder.HasIndex(indexExpression.GetPropertyAccessList(), ConfigurationSource.Explicit));
        }

        /// <summary>
        ///     Configures the table that the entity maps to when targeting SQL Server as a graph node.
        /// </summary>
        /// <typeparam name="TEntity"> The entity type being configured. </typeparam>
        /// <param name="entityTypeBuilder"> The builder for the entity type being configured. </param>
        /// <param name="nodeIdPropertyExpression"> The entity property to be used as the $node_id. </param>
        /// <returns> The same builder instance so that multiple calls can be chained. </returns>
        public static EntityTypeBuilder<TEntity> ForSqlServerIsGraphNode<TEntity>(
            [NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder,
            Expression<Func<TEntity, string>> nodeIdPropertyExpression)
            where TEntity : class
        {
            entityTypeBuilder.Metadata.SqlServer().IsGraphNode = true;

            entityTypeBuilder.Property(nodeIdPropertyExpression).HasColumnName("$node_id");
            entityTypeBuilder.Property(nodeIdPropertyExpression).Metadata.SqlServer().IsPseudoColumn = true;
            entityTypeBuilder.Property(nodeIdPropertyExpression).Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;

            return entityTypeBuilder;
        }

        /// <summary>
        ///     Configures the table that the entity maps to when targeting SQL Server as a graph edge.
        /// </summary>
        /// <typeparam name="TEntity"> The entity type being configured. </typeparam>
        /// <param name="entityTypeBuilder"> The builder for the entity type being configured. </param>
        /// <param name="fromIdPropertyExpression"> The entity property to be used as the $from_id. </param>
        /// <param name="toIdPropertyExpression"> The entity property to be used as the $to_id. </param>
        /// <returns> The same builder instance so that multiple calls can be chained. </returns>
        public static EntityTypeBuilder<TEntity> ForSqlServerIsGraphEdge<TEntity>(
            [NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder,
            Expression<Func<TEntity, string>> fromIdPropertyExpression,
            Expression<Func<TEntity, string>> toIdPropertyExpression)
            where TEntity : class
        {
            Check.NotNull(entityTypeBuilder, nameof(entityTypeBuilder));

            entityTypeBuilder.Metadata.SqlServer().IsGraphEdge = true;

            entityTypeBuilder.Property(fromIdPropertyExpression).HasColumnName("$from_id");
            entityTypeBuilder.Property(fromIdPropertyExpression).Metadata.SqlServer().IsPseudoColumn = true;

            entityTypeBuilder.Property(toIdPropertyExpression).HasColumnName("$to_id");
            entityTypeBuilder.Property(toIdPropertyExpression).Metadata.SqlServer().IsPseudoColumn = true;

            return entityTypeBuilder;
        }

        /// <summary>
        ///     Configures the table that the entity maps to when targeting SQL Server as a graph edge.
        /// </summary>
        /// <typeparam name="TEntity"> The entity type being configured. </typeparam>
        /// <param name="entityTypeBuilder"> The builder for the entity type being configured. </param>
        /// <param name="edgeIdPropertyExpression"> The entity property to be used as the $edge_id. </param>
        /// <param name="fromIdPropertyExpression"> The entity property to be used as the $from_id. </param>
        /// <param name="toIdPropertyExpression"> The entity property to be used as the $to_id. </param>
        /// <returns> The same builder instance so that multiple calls can be chained. </returns>
        public static EntityTypeBuilder<TEntity> ForSqlServerIsGraphEdge<TEntity>(
            [NotNull] this EntityTypeBuilder<TEntity> entityTypeBuilder,
            Expression<Func<TEntity, string>> edgeIdPropertyExpression,
            Expression<Func<TEntity, string>> fromIdPropertyExpression,
            Expression<Func<TEntity, string>> toIdPropertyExpression)
            where TEntity : class
        {
            entityTypeBuilder = ForSqlServerIsGraphEdge(entityTypeBuilder, fromIdPropertyExpression, toIdPropertyExpression);

            entityTypeBuilder.Property(edgeIdPropertyExpression).HasColumnName("$edge_id");
            entityTypeBuilder.Property(edgeIdPropertyExpression).Metadata.SqlServer().IsPseudoColumn = true;
            entityTypeBuilder.Property(edgeIdPropertyExpression).Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;

            return entityTypeBuilder;
        }
    }
}
