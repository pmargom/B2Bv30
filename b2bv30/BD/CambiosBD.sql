ALTER PROCEDURE [SP_ProductosBuscadorV3]
	@idUsuario int,
	@referencia varchar(20)=null,
	@familia varchar(255) = null,
	@modelo varchar(40) = null,
	@tipoNeuma int = null,
	@ic varchar(10) = null,
	@iv varchar(5) = null,
	@referencia2 varchar(20)=null,
	@RegistroInicio int,
	@RegistroFin int,
	@nFilas int output,
	@ordenarPor varchar(20)=null,
	@ordenAscDesc varchar(4)=null
AS
BEGIN
	declare @vc_cliente varchar(255)
	declare @vp_pvp decimal = 0
	declare @sPVP varchar(20)
	
	select @vc_cliente=VC_CLIENTE from V_USUARIOS where idUsuario=@idUsuario
	select @sPVP = 'BP.VP_PVP' + CAST(isnull(VC_PVP,1) as varchar) from V_USUARIOS where idUsuario=@idUsuario
	
	--print @vc_cliente
	
	declare @existeProductosEspeciales smallint=0
	select @existeProductosEspeciales = COUNT(*) 
	from B2B_PRODUCTOSESP where rtrim(ltrim(VP_CLIENTE)) = rtrim(ltrim(@vc_cliente))
	AND (VP_PRODUCTO1 LIKE '%' + @referencia + '%' OR @referencia IS NULL OR VP_PRODUCTO1 LIKE '%' + @referencia2 + '%' OR @referencia2 IS NULL)
	AND (lower(VP_FAMILIA) LIKE '%' + lower(rtrim(ltrim(@familia)))+ '%' OR @familia IS NULL)
	AND (lower(VP_MODELO) = '' + lower(@modelo) + '' OR @modelo IS NULL)
	AND (VP_TIPO_NEUMA =  cast(@tipoNeuma as varchar) OR @tipoNeuma IS NULL)
	AND (lower(VP_IC) = '' + lower(@ic) + '' OR @ic IS NULL)
	AND (lower(VP_IV) = '' + lower(@iv) + '' OR @iv IS NULL)
	
	print @existeProductosEspeciales
	
	declare @sqlStock varchar(1000)
	declare @sqlStock24 varchar(1000)
	set @sqlStock = dbo.F_GetStock(@idUsuario)
	set @sqlStock24 = dbo.F_GetStock24(@idUsuario)
	
	-- PARA FILTRAR SEGÚN LAS PREFERENCIAS DE BÚSQUEDA DEL USUARIO
	declare @myDoc xml
	select @myDoc = cast(TipoBusqueda as xml) from B2B_USUARIOS where idUsuario=@idUsuario
	
	declare @sFiltrosXML varchar(400) = ''
	
	declare @existeTipoLLanta varchar(20) = cast(@myDoc.query(N'for $x in /TipoBusqueda/Llantas return data($x)') as varchar(20))
	declare @existeTipoNeuma varchar(20) = cast(@myDoc.query(N'for $x in /TipoBusqueda/Neumatico return data($x)') as varchar(20))
	declare @existeLubricantes varchar(20) = cast(@myDoc.query(N'for $x in /TipoBusqueda/Lubricante return data($x)') as varchar(20))
	declare @existeOtros varchar(20) = cast(@myDoc.query(N'for $x in /TipoBusqueda/Otros return data($x)') as varchar(20))
	
	if (@existeTipoLLanta = 'true' or @existeTipoNeuma = 'true' or @existeLubricantes = 'true' or @existeOtros = 'true') 
	begin
		set @sFiltrosXML = @sFiltrosXML + ' AND ('
	end
	
	declare @existeFiltro bit = 0
	if (@existeTipoLLanta ='true')
	begin
		set @sFiltrosXML = @sFiltrosXML + '(BP.vp_tipo_articulo = ''LLA'')'
		set @existeFiltro = 1
	end
	
	if (@existeLubricantes ='true')
	begin
		if @existeFiltro=1 set @sFiltrosXML = @sFiltrosXML + 'OR '
		set @sFiltrosXML = @sFiltrosXML + '(BP.vp_tipo_articulo = ''LUB'')'
		set @existeFiltro = 1
	end
	
	if (@existeOtros ='true')
	begin
		if @existeFiltro=1 set @sFiltrosXML = @sFiltrosXML + 'OR '
		set @sFiltrosXML = @sFiltrosXML + '(BP.vp_tipo_articulo = ''OTR'')'
		set @existeFiltro = 1
	end	
		
	if (@existeTipoNeuma ='true')
	begin
		if @existeFiltro=1 set @sFiltrosXML = @sFiltrosXML + 'OR '
		set @sFiltrosXML = @sFiltrosXML + '((BP.vp_tipo_articulo = ''NEU'')'
		
		declare @tiposNematicos varchar(120)
		set @tiposNematicos = cast(@myDoc.query(N'for $x in /TipoBusqueda/TipoNeumatico/TipoNeumatico return data($x)') as varchar(120))
		if (LEN(@tiposNematicos)>0)
		begin
			declare @existeTipoNeumaticoPrevio bit = 0
			
			set @sFiltrosXML = @sFiltrosXML + ' AND ('
			if (@tiposNematicos like '%TURISMO%') 
			begin
				if @existeTipoNeumaticoPrevio=1 set @sFiltrosXML = @sFiltrosXML + 'OR '
				set @sFiltrosXML = @sFiltrosXML + '(BP.VP_TIPO_NEUMA = 1)'
				set @existeTipoNeumaticoPrevio = 1
			end
			if (@tiposNematicos like '%TODOTERRENO%') 
			begin
				if @existeTipoNeumaticoPrevio=1 set @sFiltrosXML = @sFiltrosXML + 'OR '
				set @sFiltrosXML = @sFiltrosXML + '(BP.VP_TIPO_NEUMA = 2)'
				set @existeTipoNeumaticoPrevio = 1
			end
			if (@tiposNematicos like '%COMERCIAL%') 
			begin
				if @existeTipoNeumaticoPrevio=1 set @sFiltrosXML = @sFiltrosXML + 'OR '
				set @sFiltrosXML = @sFiltrosXML + '(BP.VP_TIPO_NEUMA = 3)'
				set @existeTipoNeumaticoPrevio = 1
			end
			if (@tiposNematicos like '%MOTO%') 
			begin
				if @existeTipoNeumaticoPrevio=1 set @sFiltrosXML = @sFiltrosXML + 'OR '
				set @sFiltrosXML = @sFiltrosXML + '(BP.VP_TIPO_NEUMA = 4)'
				set @existeTipoNeumaticoPrevio = 1
			end			
			if (@tiposNematicos like '%CAMION%') 
			begin
				if @existeTipoNeumaticoPrevio=1 set @sFiltrosXML = @sFiltrosXML + 'OR '
				set @sFiltrosXML = @sFiltrosXML + '(BP.VP_TIPO_NEUMA IN (5,6))'
				set @existeTipoNeumaticoPrevio = 1
			end			
			set @sFiltrosXML = @sFiltrosXML + ' ))'
		end
	end

	if (@existeTipoLLanta = 'true' or @existeTipoNeuma = 'true' or @existeLubricantes = 'true' or @existeOtros = 'true')
	begin
		set @sFiltrosXML = @sFiltrosXML + ' )'	
	end
	
	-- FILTROS SEGÚN LOS PARÁMETROS PASADOS AL SP

	declare @sFiltrosNoXML varchar(400) = ''
	
	if (@referencia is not null and @referencia2 is not null) set @sFiltrosNoXML = @sFiltrosNoXML + ' AND ((BP.VP_PRODUCTO1 LIKE ''%' + @referencia + '%'') OR (BP.VP_PRODUCTO1 LIKE ''%' + @referencia2 + '%''))'
	else if (@referencia is not null) set @sFiltrosNoXML = @sFiltrosNoXML + ' AND BP.VP_PRODUCTO1 LIKE ''%' + @referencia + '%'''
	else if (@referencia2 is not null) set @sFiltrosNoXML = @sFiltrosNoXML + ' AND BP.VP_PRODUCTO1 LIKE ''%' + @referencia2 + '%'''
	if (@familia is not null) set @sFiltrosNoXML = @sFiltrosNoXML + ' AND BP.VP_FAMILIA = ''' + @familia + ''''
	if (@modelo is not null) set @sFiltrosNoXML = @sFiltrosNoXML + ' AND lower(BP.VP_MODELO) = ''' + lower(@modelo) + ''''
	if (@tipoNeuma is not null) set @sFiltrosNoXML = @sFiltrosNoXML + ' AND BP.VP_TIPO_NEUMA = ' + cast(@tipoNeuma as varchar)
	if (@ic is not null) set @sFiltrosNoXML = @sFiltrosNoXML + ' AND lower(BP.VP_IC) = ''' + lower(@ic) + '''' 
	if (@iv is not null) set @sFiltrosNoXML = @sFiltrosNoXML + ' AND lower(BP.VP_IV) = ''' + lower(@iv) + '''' 

	declare @sSQL varchar(MAX)
	
	/* FILTRADO CON REGISTRO DE INICIO Y FIN DE LA FORMA	
	SELECT * FROM ( 
		SELECT *, ROW_NUMBER() OVER (ORDER BY name) as row FROM sys.databases 
	) a WHERE a.row > 5 and a.row <= 10	
	*/
	--CRITERIO DE ORDENACIÓN DE REGISTROS SEGÚN FILTROS
	declare @ordenConsulta varchar(50) = 'BP.ID'
	
	if (LOWER(@ordenarPor) = 'nombre') set @ordenConsulta = 'BP.VP_DESCRIPCION'
	if (LOWER(@ordenarPor) = 'precio') set @ordenConsulta = 'BP.VP_PVP2'
	
	if (LOWER(@ordenAscDesc) = 'desc') set @ordenConsulta = @ordenConsulta + ' desc'

	set @sSQL = 'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' + @ordenConsulta + ') as row, ISNULL(BP.ID,-1) ''ID'', BP.VP_PRODUCTO, BP.VP_DESCRIPCION,BP.VP_MODELO, BP.VP_FAMILIA, BP.VP_DESCFAM, BP.VP_IC, BP.VP_IV'
	if (@existeProductosEspeciales>0) set @sSQL = @sSQL + ',BPE.VP_PVP1 ''PrecioUnidad'''
	else set @sSQL = @sSQL + ',' + @sPVP + '''PrecioUnidad'''
	set @sSQL = @sSQL + ',BP.VP_PRODUCTO1,ISNULL(BP.VP_TIPO_NEUMA,0) ''VP_TIPO_NEUMA'', ISNULL(BP.VP_TIPO_OFER, 0) ''VP_TIPO_OFER''
	,BP.VP_COLOR_OFER,BP.VP_CATEGORIA,BP.VP_PORC_IMP
	,isnull(CASE WHEN BP.VP_IMPORTADO=1 THEN (SELECT VT_PVP1 FROM B2B_ECOTASA WHERE VT_CATEGORIA = BP.VP_CATEGORIA AND VT_PVP1 IS NOT NULL)
			ELSE (SELECT VT_PVP1 FROM B2B_ECOTASA WHERE VT_CATEGORIA = BP.VP_CATEGORIA AND VT_PVP1 IS NOT NULL) END	,0) Ecotasa
	,isnull(CASE WHEN BP.VP_IMPORTADO=1 THEN (SELECT VT_PORC_IMP FROM B2B_ECOTASA WHERE VT_CATEGORIA = BP.VP_CATEGORIA AND VT_PVP1 IS NOT NULL)
			ELSE (SELECT VT_PORC_IMP FROM B2B_ECOTASA WHERE VT_CATEGORIA = BP.VP_CATEGORIA  AND VT_PVP1 IS NOT NULL) END	,0) VT_PORC_IMP	'

	set @sSQL = @sSQL + ',isnull((SELECT distinct ' + @sqlStock + ' FROM B2B_STOCK WHERE VS_ARTICULO = BP.VP_PRODUCTO), 0) ''Stock'''
	set @sSQL = @sSQL + ',isnull((SELECT distinct ' + @sqlStock24 + ' FROM B2B_STOCK WHERE VS_ARTICULO = BP.VP_PRODUCTO), 0) ''Stock24'''

	set @sSQL = @sSQL + ',(SELECT case when COUNT(*) > 0 then ''Datos llegada'' else '''' end FROM B2B_PENDIENTES WHERE VL_ARTICULO=BP.VP_PRODUCTO AND [VL_LLEGADA] >= GETDATE()) PteLlegada,
	REPLACE(REPLACE(VP_IMAGEN,''U:\articulos\'',''http://localhost/imagenesneu/''),''\'',''/'') AS ''VP_IMAGEN''
	,(SELECT TOP 1 VT_DETALLES FROM B2B_ECOTASA WHERE VT_CATEGORIA = BP.VP_CATEGORIA AND VT_PVP1 IS NOT NULL ORDER BY VT_PVP1 DESC) ECOTASA_DETALLES
	, BP.VP_IMPORTADO, BP.VP_DESC_TIPO, BF.VF_DESCFAM, BF.VF_LOGO, BP.VP_NIVELRUIDO, BP.VP_EFICOMBUSTIBLE, BP.VP_ADHERENCIA, BP.VP_VALORRUIDO		
	FROM B2B_PRODUCTOS BP'
	
	declare @condiciones varchar(MAX)
	set @condiciones = ''	

	if (@existeProductosEspeciales>0) set @condiciones = @condiciones + ' LEFT JOIN B2B_PRODUCTOSESP BPE ON BP.VP_PRODUCTO = BPE.VP_PRODUCTO'
	set @condiciones = @condiciones + ' LEFT JOIN B2B_FAMILIAS_LOGOS BF ON BP.VP_FAMILIA = BF.VF_FAMILIA'
	set @condiciones = @condiciones + ' WHERE (1=1) '
	if (@existeProductosEspeciales>0) set @condiciones = @condiciones + ' AND RTRIM(LTRIM(BPE.VP_CLIENTE)) = RTRIM(LTRIM(''' + @vc_cliente + '''))' 
	set @condiciones = @condiciones + ' AND 
	(
		(SELECT COUNT(*) FROM B2B_PENDIENTES WHERE VL_ARTICULO=BP.VP_PRODUCTO AND [VL_LLEGADA] >= GETDATE()) >0
		OR
		isnull((SELECT distinct ' + @sqlStock + ' FROM B2B_STOCK WHERE VS_ARTICULO = BP.VP_PRODUCTO), 0)>0
		OR
	    isnull((SELECT distinct ' + @sqlStock24 + ' FROM B2B_STOCK WHERE VS_ARTICULO = BP.VP_PRODUCTO), 0)>0
	)'
	if (@existeProductosEspeciales>0) set @condiciones = @condiciones + ' and (BPE.VP_PVP1 > 0) '
	else set @condiciones = @condiciones + ' and (' + @sPVP + ' > 0) '
	
	declare @orden varchar(100) = '' -- con la consulta registros paginados no se puede poner ORDER BY dos veces ' ORDER BY ID'
	declare @paginacion VARCHAR(50) = ') a WHERE a.row > ' + CAST(@RegistroInicio AS VARCHAR) + ' and a.row <= ' + CAST(@RegistroFin AS VARCHAR) 
	
	set @sSQL = @sSQL + @condiciones + @sFiltrosXML + @sFiltrosNoXML + @orden + @paginacion
	
	print @sSQL	
	print 'SELECT COUNT(*) FROM B2B_PRODUCTOS BP ' + @condiciones + @sFiltrosXML + @sFiltrosNoXML 
	exec (@sSQL)

	-- CUENTA DE TOTAL DE REGISTROS
	declare @consultaCount nvarchar(max) = N'SELECT @nFilas = COUNT(*) FROM B2B_PRODUCTOS BP ' + @condiciones + @sFiltrosXML + @sFiltrosNoXML
	declare @numero int
	execute sp_executesql @consultaCount, N'@nFilas int OUTPUT', @nFilas = @numero output
	SELECT @numero AS nFilas
	--exec ('SELECT @nFilas = CONVERT(VARCHAR(20), COUNT(*)) FROM B2B_PRODUCTOS BP ' + @condiciones + @sFiltrosXML + @sFiltrosNoXML)
		
END