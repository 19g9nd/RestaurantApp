USE RestaurantAppDb
GO

-- Проверка наличия задачи с таким именем
IF NOT EXISTS (SELECT * FROM msdb.dbo.sysjobs WHERE name = N'DeleteExpiredDiscountCodeJob')
BEGIN
    -- Создание задачи (job) только если она не существует
    EXEC msdb.dbo.sp_add_job
        @job_name = N'DeleteExpiredDiscountCodeJob',
        @enabled = 1,
        @description = N'Удаляет просроченные коды скидок',
        @owner_login_name = N'sa',
        @notify_level_eventlog = 0,
        @notify_level_email = 0,
        @notify_level_netsend = 0,
        @notify_level_page = 0;

    -- Добавление шага для задачи
    EXEC msdb.dbo.sp_add_jobstep
        @job_name = N'DeleteExpiredDiscountCodeJob',
        @step_id = 1,
        @step_name = N'DeleteExpiredDiscountCodeStep',
        @command = N'DELETE FROM DiscountCodes WHERE ValidTo < GETDATE();',
        @database_name = N'RestaurantAppDb',
        @on_success_action = 3;
END

-- Проверка наличия расписания
IF NOT EXISTS (SELECT * FROM msdb.dbo.sysjobschedules WHERE job_id = (SELECT job_id FROM msdb.dbo.sysjobs WHERE name = N'DeleteExpiredDiscountCodeJob'))
BEGIN
    -- Добавление расписания, если оно не существует
    EXEC msdb.dbo.sp_add_schedule
        @schedule_name = N'DailySchedule',
        @freq_type = 4,
        @freq_interval = 1,
        @active_start_time = 000000;
END

-- Привязка задачи к расписанию
EXEC msdb.dbo.sp_attach_schedule
    @job_name = N'DeleteExpiredDiscountCodeJob',
    @schedule_name = N'DailySchedule';

-- Включение задачи
EXEC msdb.dbo.sp_update_job
    @job_name = N'DeleteExpiredDiscountCodeJob',
    @enabled = 1;
