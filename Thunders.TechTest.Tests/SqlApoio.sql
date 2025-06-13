 select 
 (select count(*) from [dbo].[Estados])													Est
,(select count(*) from [dbo].[Cidades])													Cidades
,(select count(*) from [dbo].[Pracas])													Pracas
,(select count(*) from [dbo].[Veiculos])												Veic
,(select count(*) from [dbo].[PedagiosUtilizacoes])										[Utiliz]

,(select count(*) from [dbo].[FaturamentosHorasCidadesReports]	)						[FatHorCid]
,(select count(*) from [dbo].[FaturamentosHorasCidadesReports]									
where processar = 1)																	[FHCP]
,(
	select	datediff(second, min(DataHoraSolicitacao), dateadd(hour, -3, getdate()) )
	from	[dbo].[FaturamentosHorasCidadesReports]					
	where	processar = 1)																[FHCP_SEC]

,(select count(*) from [dbo].[FaturamentosPracasMesesReports]	)						[FatPracMes]	
,(select count(*) from [dbo].[FaturamentosPracasMesesReports]										
where processar = 1)																	[FPMP]
												
	
,(select count(*) from [dbo].[FaturamentosPracasTiposVeiculosReports])					[FatPracTipVeic]
,(select count(*) from [dbo].[FaturamentosPracasTiposVeiculosReports]						
where processar = 1)																	[FPTVP]

SELECT 
    FORMAT(criacao, 'yyyy-MM-dd HH:mm') AS Minuto,
    COUNT(*) AS Quantidade
FROM 
    PedagiosUtilizacoes (nolock)
where criacao >= dateadd(minute,-5,getdate())
GROUP BY 
    FORMAT(criacao, 'yyyy-MM-dd HH:mm')
ORDER BY 
    Minuto DESC;