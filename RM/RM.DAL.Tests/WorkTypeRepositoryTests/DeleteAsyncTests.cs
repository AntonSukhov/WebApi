﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RM.DAL.Abstractions.Models;
using RM.DAL.Abstractions.Repositories;
using RM.DAL.Tests.Fixtures;
using RM.DAL.Tests.TestData;

namespace RM.DAL.Tests.WorkTypeRepositoryTests;

/// <summary>
/// Тесты для метода <see cref="IWorkTypeRepository.Delete"/>.
/// </summary>
/// <param name="fixture">Настройка контекста для тестирования репозитория видов работ.</param>
public class DeleteAsyncTests(WorkTypeRepositoryFixture fixture) : IClassFixture<WorkTypeRepositoryFixture>
{
    #region Поля

    /// <summary>
    /// Репозиторий вида работ, работающий с MS SQL.
    /// </summary>
    private readonly IWorkTypeRepository _repositoryMsSql = fixture.WorkTypeRepositoryMsSql;

    /// <summary>
    /// Репозиторий вида работ, работающий с PostgreSQL.
    /// </summary>
    private readonly IWorkTypeRepository _repositoryPostgreSql = fixture.WorkTypeRepositoryPostgreSql;

    #endregion

    #region Методы

    /// <summary>
    /// Тест удаления вида работ для корректных данных из источника данных. MS SQL.
    /// </summary>
    [Theory]
    [MemberData(nameof(WorkTypeRepositoryTestData.DeleteAsyncForCorrectDataTestData),
                MemberType = typeof(WorkTypeRepositoryTestData))]
    public async Task ForCorrectDataMsSql(WorkTypeModel workTypeModel)
    {      
        await ForCorrectData(workTypeModel, _repositoryMsSql);
    }

    /// <summary>
    /// Тест удаления вида работ для корректных данных из источника данных. MS SQL.
    /// </summary>
    [Theory]
    [MemberData(nameof(WorkTypeRepositoryTestData.DeleteAsyncForCorrectDataTestData),
                MemberType = typeof(WorkTypeRepositoryTestData))]
    public async Task ForCorrectDataPostgreSql(WorkTypeModel workTypeModel)
    {      
        await ForCorrectData(workTypeModel, _repositoryPostgreSql);
    }

    /// <summary>
    /// Тест удаления несуществующего вида работ. MS SQL.
    /// </summary>
    /// <param name="workTypeId">Идентификатор вида работ.</param>
    [Theory]
    [MemberData(nameof(WorkTypeRepositoryTestData.DeleteAsyncNotExistedWorkTypeTestData),
                MemberType = typeof(WorkTypeRepositoryTestData))]

    public async Task NotExistedWorkTypeMsSql(Guid workTypeId)
    {
        await NotExistedWorkType(workTypeId, _repositoryMsSql);  
    }

    /// <summary>
    /// Тест удаления несуществующего вида работ. PostgreSQL.
    /// </summary>
    /// <param name="workTypeId">Идентификатор вида работ.</param>
    [Theory]
    [MemberData(nameof(WorkTypeRepositoryTestData.DeleteAsyncNotExistedWorkTypeTestData),
                MemberType = typeof(WorkTypeRepositoryTestData))]

    public async Task NotExistedWorkTypePostgreSql(Guid workTypeId)
    {
        await NotExistedWorkType(workTypeId, _repositoryPostgreSql);  
    }


    #region Закрытые методы

    /// <summary>
    /// Тест удаления вида работ для корректных данных.
    /// </summary>
    /// <param name="workTypeModel">Модель вида работ.</param>
    /// <param name="repository">Репозиторий вида работ.</param>
    /// <returns/>
    private async Task ForCorrectData(WorkTypeModel workTypeModel, IWorkTypeRepository repository)
    {      
        await repository.CreateAsync(workTypeModel);

        await repository.DeleteAsync(workTypeModel.Id);

        var expected = await repository.GetByIdAsync(workTypeModel.Id);
        
        expected.Should().BeNull();
    }

    /// <summary>
    /// Тест удаления несуществующего вида работ.
    /// </summary>
    /// <param name="workTypeId">Идентификатор вида работ.</param>
    /// <param name="repository">Репозиторий вида работ.</param>
    /// <returns/>
    private async Task NotExistedWorkType(Guid workTypeId, IWorkTypeRepository repository)
    {
        var action = async () => await repository.DeleteAsync(workTypeId);

        await action.Should().ThrowAsync<DbUpdateConcurrencyException>();    
    }

    #endregion

    #endregion
}
